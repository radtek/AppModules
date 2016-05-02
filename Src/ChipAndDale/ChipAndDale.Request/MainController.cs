using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ChipAndDale.Request.Db;
using ChipAndDale.Request.ViewModel;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Nsi;
using ChipAndDale.SDK.Request;
using Core.SDK.Common;
using Core.SDK.Composite.Common;
using Core.SDK.Composite.Event;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Db;
using Core.SDK.Dom;
using Core.SDK.Log;
using Core.UtilsModule;
using Core.SDK.Composite.Event.EventMessage;
using ChipAndDale.SDK.Request.EventMessage;
using ChipAndDale.Request.Setting;
using ChipAndDale.SDK;

namespace ChipAndDale.Request
{
    internal class MainController : IMainController
    {
        public MainController(IDbAccessor dbAccessor)
        {
            _dbAccessor = dbAccessor;
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _logMgr = serviceMgr.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("Request.MainController");
            _dialogMgr = serviceMgr.GetInstance<IViewFormMgr>();
            _regionMgr = serviceMgr.GetInstance<IRegionMgr>();
            _commonService = serviceMgr.GetInstance<ICommonService>();
            _dbMgr = serviceMgr.GetInstance<IDbMgr>();
            _eventMgr = serviceMgr.GetInstance<IEventMgr>();            
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: IDBMgr = {0}; ICommonDbService = {1}; _dialogMgr = {2}; IRegionMgr = {3}; IEventMgr = {4}.", _dbMgr.ToStateString(), _commonService.ToStateString(), _dialogMgr.ToStateString(), _regionMgr.ToStateString(), _eventMgr.ToStateString());

            Init();           
        }

        #region IRequestService

        public RequestEntity OpenRequestEditDialog(RequestEntity request, bool readOnly, bool isProcess, bool isCommit)
        {
            _logger.Debug("OpenRequestEditDialog.");
            _logger.Debug("Params: Request = {0}; ReadOnly = {1}; IsProcess = {2}; IsCommit = {3}", request.ToStateString(), Hlp.BoolToStr(readOnly), Hlp.BoolToStr(isProcess), Hlp.BoolToStr(isCommit));                        
            if (request == null || request.IsNewEntity) _logger.Debug("Creating new request."); else _logger.Debug("Request = {0}; ", request.ToInternalString());

            RequestEntity result = null;
            using (RequestEditViewModel viewModel = CreateRequestEditViewModel(request, readOnly, isProcess, isCommit))
            {
                if (_dialogMgr.ShowModal(viewModel) == DialogResult.OK && !readOnly) 
                        result = viewModel.Request;                
            }

            return result;
        }       

        public IEnumerable<RequestEntity> OpenRequstListDialog(RequestListFilterEntity filter)
        {
            _logger.Debug("OpenRequstListDialog.");
            _logger.Debug("Params: Filter = {0}.", filter.ToStateString());

            using (RequestListViewModel viewModel = CreateRequestListViewModel(filter, true))
            {
                if (_dialogMgr.ShowModal(viewModel) == DialogResult.OK)
                    return viewModel.SelectRequestList;
                else return new List<RequestEntity>();
            }
        }

        public RequestListFilterEntity OpenRequestListFilterEditDialog(RequestListFilterEntity filter)
        {
            _logger.Debug("OpenRequestListFilterEditDialog.");
            _logger.Debug("Params: Filter = {0}.", filter.ToStateString());

            using (RequestFilterViewModel viewModel = CreateRequestFilterViewModel(filter))
            {
                if (_dialogMgr.ShowModal(viewModel) == DialogResult.OK)
                    return viewModel.Filter;
                else return null;
            }
        }

        public BindingCollection<RequestEntity> GetRequstList(RequestListFilterEntity filter)
        {
            if (filter == null) throw new ArgumentNullException("Filter can not be null.");
            _logger.Debug("Params: Filter = {0};", filter.ToInternalString());

            return _dbAccessor.GetRequests(filter);           
        }

        public RequestEntity GetRequestById(string requestId) 
        {
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId can not be null.");
            _logger.Debug("Params: requestId = {0};", requestId);
            
            return _dbAccessor.GetRequestById(requestId);            
        }

        public void AddRequest(RequestEntity request, bool isCommit)
        {
            if (request == null) throw new ArgumentNullException("Request can not be null.");
            _logger.Debug("Params: isCommit = {0};", Hlp.BoolToStr(isCommit));
            
            Execute(() => { AddRequestInternal(request); }, isCommit);
        }

        public void RemoveRequest(string requestId, bool isCommit)
        {
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId can not be null.");
            _logger.Debug("Params: requestId = {0}; isCommit = {1};", requestId, Hlp.BoolToStr(isCommit));

            Execute(() => { RemoveRequestInternal(requestId); }, isCommit);            
        }        

        public void UpdateRequest(RequestEntity requestBefore, RequestEntity requestAfter, bool isCommit)
        {
            if (requestBefore == null) throw new ArgumentNullException("RequestBefore can not be null.");
            if (requestAfter == null) throw new ArgumentNullException("RequestAfter can not be null.");
            if (!string.Equals(requestBefore.Id, requestAfter.Id)) throw new InvalidOperationException("Id field of RequestBefore and RequestAfter params have to be equal.");
            _logger.Debug("Params: requestBefore.Id = {0}; isCommit = {1};", requestBefore.Id, Hlp.BoolToStr(isCommit));

            Execute(() => { UpdRequestInternal(requestBefore, requestAfter); }, isCommit);
        }

        public IList<KeyValuePair<Enum, string>> RequestStates
        {
            get { return typeof(RequestState).EnumToPairIList(); }
        }

        public bool IsRequestFiltered(RequestEntity request, RequestListFilterEntity filter)
        {
            if (request == null) return true;
            if (filter == null) return false;
            return !(StrHlp.Contains(request.Id, filter.RequestId) &&
                DateHlp.Between(request.ReqDateTime, filter.StartDateTime, filter.StopDateTime) &&
                Hlp.EqualsFilter<UserEntity>(request.CreatorUser, filter.CreatorUser) &&
                Hlp.EqualsFilter<UserEntity>(request.ResponseUser, filter.ResponseUser) &&
                Hlp.EqualsFilter<OrgEntity>(request.Organization, filter.Organization) &&
                Hlp.EqualsFilter<AppEntity>(request.Application, filter.Application) &&
                StrHlp.Contains(request.Subject, filter.Subject) &&
                StrHlp.Contains(request.Comments, filter.Comments) &&
                StrHlp.Contains(request.Contact, filter.Contact) &&
                (string.IsNullOrEmpty(filter.StatusIdList) || StrHlp.Contains(filter.StatusIdList, request.StateId.ToStateString())) );
            //(string.IsNullOrEmpty(filter.TagList) || StrHlp.Contains(filter.TagList, request.Ta.ToStateString()));
        }

        public string SaveRequestToSetting(RequestEntity request)
        {
            return _settingController.SaveRequestToSetting(request);
        }

        public RequestEntity LoadRequestFromSetting(string settingName)
        {
            return _settingController.LoadRequestFromSetting(settingName);
        }

        #endregion IRequestService

        
        #region IMainController

        public void Prepocess()
        {
            InitFilter();
        }
        
        public void OpenRequestListPage(RequestListFilterEntity filter)
        {
            _logger.Debug("OpenRequestListPage.");
            _logger.Debug("Params: Filter = {0}.", filter.ToStateString());

            RequestListViewModel viewModel = null;
            try
            {
                viewModel = CreateRequestListViewModel(filter, false);
                ShowRequestListViewModel(viewModel);
            }
            catch (Exception ex)
            {
                InvokerHlp.WithExceptionSuppress(() => { _requestListViewModelList.Remove(viewModel.Ident); }, _logger);                
                throw ex;
            }
        }        

        public void OpenRequestEditForm(RequestEntity request, bool readOnly)
        {
            _logger.Debug("OpenRequestEditForm.");
            _logger.Debug("Params: Request = {0}; ReadOnly = {1}.", request.ToStateString(), Hlp.BoolToStr(readOnly));
            if (request == null || request.IsNewEntity) _logger.Debug("Creating new request."); else _logger.Debug("Request = {0}; ", request.ToInternalString());

            RequestEditViewModel viewModel = null;
            viewModel = new RequestEditViewModel(request, this, readOnly, true, true);                            
            _dialogMgr.ShowNoModal(viewModel);
            _requestViewModelList.Add(viewModel.Ident, viewModel);
        }

        public bool ProcessRequestViewModel(RequestEditViewModel viewModel)
        {
            try
            {
                if (!viewModel.ReadOnly && viewModel.IsProcess)
                {                    
                    if (viewModel.RequestOrigin == null || viewModel.RequestOrigin.IsNewEntity)
                        AddRequest(viewModel.Request, viewModel.IsCommit);                                            
                    else
                        UpdateRequest(viewModel.RequestOrigin, viewModel.Request, viewModel.IsCommit);                                            
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }

        public void OnAfterCloseRequestViewModel(RequestEditViewModel viewModel)
        {
            RequestEditViewModel viewModelinList = null;
            if (_requestViewModelList.TryGetValue(viewModel.Ident, out viewModelinList))
            {
                _requestViewModelList.Remove(viewModel.Ident);                
                viewModel.Dispose();
            }
        }


        public void OnAfterCloseRequesListViewModel(RequestListViewModel viewModel)
        {
            RequestListViewModel viewModelinList = null;
            if (_requestListViewModelList.TryGetValue(viewModel.Ident, out viewModelinList))
            {
                _requestListViewModelList.Remove(viewModel.Ident);                
                viewModel.Dispose();
            }
        }


        public RequestEditViewModel CreateRequestEditViewModel(RequestEntity request, bool readOnly, bool isProcess, bool isCommit)
        {
            RequestEditViewModel viewModel = new RequestEditViewModel(request, this, readOnly, isProcess, isCommit);
            return viewModel;
        }

        public RequestListViewModel CreateRequestListViewModel(RequestListFilterEntity filter, bool readOnly) 
        {
            RequestListViewModel viewModel = new RequestListViewModel(filter, this, readOnly);
            return viewModel;
        }

        public RequestFilterViewModel CreateRequestFilterViewModel(RequestListFilterEntity filter)
        {
            RequestFilterViewModel viewModel = new RequestFilterViewModel(filter, this);
            return viewModel;
        }



        public void LoadSetting()
        {                        
            _settingController.Load();
            Clear();

            if (_settingController.Filters != null && _settingController.Filters.Count > 0)
            {                
                _requestFilterList = _settingController.Filters;
            }

            foreach (RequestListViewModel viewModel in _settingController.ListViewModels)
            {
                try
                {
                    ShowRequestListViewModel(viewModel);
                }
                catch (Exception ex)
                {
                    InvokerHlp.WithExceptionSuppress(() => { _requestListViewModelList.Remove(viewModel.Ident); }, _logger);                
                    throw ex;
                }
            }

            foreach (RequestEditViewModel viewModel in _settingController.EditViewModels)
            {
                try
                {                    
                    _dialogMgr.ShowNoModal(viewModel, viewModel.DialogSetting);
                    _requestViewModelList.Add(viewModel.Ident, viewModel);
                }
                catch (Exception ex)
                {
                    InvokerHlp.WithExceptionSuppress(() => { _requestViewModelList.Remove(viewModel.Ident); }, _logger);
                    throw ex;
                }
            }
        }

        public void SaveSetting()
        {
            _settingController.Filters = new List<RequestListFilterEntity>(_requestFilterList);
            _settingController.ListViewModels = new List<RequestListViewModel>(_requestListViewModelList.Values);
            _settingController.EditViewModels = new List<RequestEditViewModel>(_requestViewModelList.Values); 
            _settingController.Save();
        }

        public List<RequestListFilterEntity> Filters
        {
            get { return _requestFilterList; }
        }

        public RequestListFilterEntity OnSaveFilter(RequestListFilterEntity filter)
        {
            if (filter.IsNewEntity)
            {
                RequestListFilterEntity newFilter = filter.Clone();                
                newFilter.Id = "-";
                newFilter.IsPersisit = true;
                _requestFilterList.Add(newFilter);
                return newFilter;
            }
            else
            {
                int i = _requestFilterList.IndexOf(filter);
                if (i == -1) throw new ChipAndDaleException("Фільтр не знайдений");
                _requestFilterList[i] = filter.Clone();
                _requestFilterList[i].IsPersisit = true;
                return _requestFilterList[i];
            }
        }

        public void OnDeleteFilter(RequestListFilterEntity filter)
        {
            int i = _requestFilterList.IndexOf(filter);
            if (i == -1) throw new ChipAndDaleException("Фільтр не знайдений");
            _requestFilterList.Remove(filter);
        }

        #endregion IMainController        


        #region private

        
        class ReqChangeData
        {            
            public RequestChangeType ChangeType { get; set; }
            public RequestEntity Request { get; set; }
        }

        ILogMgr _logMgr;
        ILogger _logger;
        IDbAccessor _dbAccessor;
        IViewFormMgr _dialogMgr;
        IRegionMgr _regionMgr;
        ICommonService _commonService;
        IDbMgr _dbMgr;
        IEventMgr _eventMgr;
        ISettingController _settingController;

        Dictionary<IdentKey, RequestEditViewModel> _requestViewModelList;
        Dictionary<IdentKey, RequestListViewModel> _requestListViewModelList;
        List<RequestListFilterEntity> _requestFilterList;        

        List<RequestChangedMessage> _requestChangeCashe;

        private void Init()
        {
            _requestViewModelList = new Dictionary<IdentKey, RequestEditViewModel>();
            _requestListViewModelList = new Dictionary<IdentKey, RequestListViewModel>();
            
            InitFilter();

            _requestChangeCashe = new List<RequestChangedMessage>();

            _settingController = new SettingController(this);

            _eventMgr.GetEvent<DbTransactionFinishEvent>().Subscribe(OnDbTransactionFinished);
        }

        void InitFilter()
        {
            _requestFilterList = new List<RequestListFilterEntity>();           

            RequestListFilterEntity iResponseFilter = new RequestListFilterEntity();
            iResponseFilter.Id = "-";
            iResponseFilter.FilterName = "Я відповідальний";
            iResponseFilter.ResponseUser = _commonService.User;
            iResponseFilter.StartDateTime = DateTime.Now.Date.AddDays(-30);
            iResponseFilter.StatusIdList = string.Join(",", new string[] { ((int)RequestState.Open).ToString(),
                                                         ((int)RequestState.Analyze).ToString(),
                                                         ((int)RequestState.InWork).ToString(),
                                                         ((int)RequestState.Done).ToString() });
            _requestFilterList.Add(iResponseFilter);


            RequestListFilterEntity iCreatorFilter = new RequestListFilterEntity();
            iCreatorFilter.Id = "-";
            iCreatorFilter.FilterName = "Я створив";
            iCreatorFilter.CreatorUser = _commonService.User;
            iCreatorFilter.StartDateTime = DateTime.Now.Date.AddDays(-30);
            iCreatorFilter.StatusIdList = string.Join(",", new string[] { ((int)RequestState.Open).ToString(),
                                                         ((int)RequestState.Analyze).ToString(),
                                                         ((int)RequestState.InWork).ToString(),
                                                         ((int)RequestState.Done).ToString(),
                                                         ((int)RequestState.Done).ToString() });
            _requestFilterList.Add(iCreatorFilter);
        }

        void Clear()
        {
            List<RequestEditViewModel> _list = new List<RequestEditViewModel>(_requestViewModelList.Values);
            foreach (RequestEditViewModel viewModel in _list)
            {
                _dialogMgr.CloseNoModal(viewModel.Ident);
            }
            _requestViewModelList.Clear();

            foreach (RequestListViewModel viewModel in _requestListViewModelList.Values)
            {
                _regionMgr.GetViewRegion(SDK.RegionName.DocumentRegion).RemoveView(viewModel.Ident);
            }
            _requestListViewModelList.Clear();
        }

        void Execute(Action action, bool isCommit)
        {
            if (isCommit)
            {
                using (DbTransaction transaction = new DbTransaction(_dbMgr))
                {
                    action();
                    transaction.Success = true;
                }
            }
            else action();
        }

        void AddRequestInternal(RequestEntity request)
        {
            string reqId = _dbAccessor.InsRequest(request);
            
            if (request.Tags != null)
                foreach(TagEntity tag in request.Tags.Entities)
                    _dbAccessor.InsReqTag(reqId, tag);
            
            if (request.Attaches != null)
                foreach(AttachEntity attach in request.Attaches.Entities)
                    _dbAccessor.InsReqAttach(reqId, attach);

            _requestChangeCashe.Add(new RequestChangedMessage(RequestChangeType.Create, request));
        }

        void UpdRequestInternal(RequestEntity requestBefore, RequestEntity requestAfter)
        {
            _dbAccessor.UpdRequest(requestAfter);
            BindingCollection<TagEntity> forAddTag = null;
            BindingCollection<TagEntity> forUpdTag = null;
            BindingCollection<TagEntity> forDelTag = null;
            EntityList<TagEntity>.DiffTwoList(requestBefore.Tags, requestAfter.Tags, out forAddTag, out forUpdTag, out forDelTag);
            foreach (TagEntity tag in forAddTag)
                _dbAccessor.InsReqTag(requestBefore.Id, tag);           
            foreach (TagEntity tag in forDelTag)
                _dbAccessor.DelReqTag(requestBefore.Id, tag.Id);

            BindingCollection<AttachEntity> forAddAttach = null;
            BindingCollection<AttachEntity> forUpdAttach = null;
            BindingCollection<AttachEntity> forDelAttach = null;
            EntityList<AttachEntity>.DiffTwoList(requestBefore.Attaches, requestAfter.Attaches, out forAddAttach, out forUpdAttach, out forDelAttach);
            foreach (AttachEntity attach in forAddAttach)
                _dbAccessor.InsReqAttach(requestBefore.Id, attach);
            foreach (AttachEntity attach in forDelAttach)
                _dbAccessor.DelReqAttach(attach.Id);

            _requestChangeCashe.Add(new RequestChangedMessage(RequestChangeType.Update, requestAfter));
        }

        void RemoveRequestInternal(string requestId)
        {
            _dbAccessor.DelRequest(requestId);
            
            RequestEntity dummyRequest = RequestEntity.Create();
            dummyRequest.Id = requestId;
            _requestChangeCashe.Add(new RequestChangedMessage(RequestChangeType.Delete, dummyRequest));
        }

        private void ShowRequestListViewModel(RequestListViewModel viewModel)
        {
            viewModel.Ident = _regionMgr.GetViewRegion(SDK.RegionName.DocumentRegion).AddView(viewModel);
            viewModel.Init();
            _requestListViewModelList.Add(viewModel.Ident, viewModel);
        }

        void OnDbTransactionFinished(DbTransactionFinishMessage message)
        {
            try
            {
                if (message.Result == TransactionResult.Commit)
                    foreach (RequestChangedMessage changedMessage in _requestChangeCashe)
                    {
                        FireRequestChangedEvent(changedMessage);
                    }
            }
            finally
            {
                _requestChangeCashe.Clear();
            }
        }

        void FireRequestChangedEvent(RequestChangedMessage changedMessage)
        {
            _eventMgr.GetEvent<RequestChangedEvent>().Publish(changedMessage);
        }
 
        #endregion private        
    }
}
