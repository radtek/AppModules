using System;
using System.Collections.Generic;
using System.ComponentModel;
using ChipAndDale.Request.UI;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Nsi;
using ChipAndDale.SDK.Request;
using Core.SDK.Common;
using Core.SDK.Composite.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Dom;
using Core.SDK.Log;
using Core.UtilsModule;
using Core.SDK.Composite.Event;
using ChipAndDale.SDK.Request.EventMessage;

namespace ChipAndDale.Request.ViewModel
{
    internal class RequestListViewModel : ViewBase<RequestListControl>, IDisposable
    {
        public RequestListViewModel(RequestListFilterEntity filter, IMainController mainController, bool readOnly)
            : base() 
        {
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();
            _commonDbService = ServiceMgr.Current.GetInstance<ICommonService>();
            _nsiService = ServiceMgr.Current.GetInstance<INsiService>();
            _viewFormMgr = ServiceMgr.Current.GetInstance<IViewFormMgr>();
            _messageBoxMgr = ServiceMgr.Current.GetInstance<IMessageBoxMgr>();
            _eventMgr = ServiceMgr.Current.GetInstance<IEventMgr>();
            _logger = _logMgr.GetLogger("RequestListViewModel");
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: ICommonDbService = {0}; INsiService = {1}; IViewFormMgr = {2}; IMessageBoxMgr = {3}; IEventMgr = {4}", _commonDbService.ToStateString(), _nsiService.ToStateString(), _viewFormMgr.ToStateString(), _messageBoxMgr.ToStateString(), _eventMgr.ToStateString());                       
            
            if (mainController == null) throw new ArgumentNullException("MainController param can not be null.");
            _mainController = mainController;            
            _readOnly = readOnly;
            _filterList = new List<RequestListFilterEntity>();            

            InitView();

            if (filter == null) Filter = Filters[0];
            else Filter = filter;            
        }
        

        #region IView

        public override bool CanClose
        {
            get { return Validate(); }
        }

        public override bool OnClosing(bool? result)
        {            
            return true;
        }

        public override void OnAfterClose()
        {
            _mainController.OnAfterCloseRequesListViewModel(this);
        }

        public override void OnActivate(bool isActive)
        { }          

        #endregion IView
        
        
        #region Specific

        public bool ReadOnly
        {
            get { return _readOnly; }
        }

        string _initialCurrentRequestId;
        public string InitialCurrentRequestId
        {
            get { return _initialCurrentRequestId; }
            set { _initialCurrentRequestId = value; }
        }

        public byte[] GridData
        {
            get { return _control.GetGridLayoutData(); }

            set { _control.SetGridLayoutData(value); }
        }

        public EntityList<RequestEntity> RequestList
        {
            get { return _requestList; }           
        }

        public IEnumerable<RequestEntity> SelectRequestList
        {
            get { return _control.SelectedRows; }
        }

        public RequestListFilterEntity Filter
        {
            get { return _filter; }
            set { SetPropertyValue<RequestListFilterEntity>("Filter", ref _filter, ref value, false); }
        }

        List<RequestListFilterEntity> _filterList;
        public List<RequestListFilterEntity> Filters
        {
            get { return _filterList; }            
        }

        public void Init()
        { 
            OnRefreshRequestList();

            if (!string.IsNullOrEmpty(InitialCurrentRequestId))
            {
                foreach (RequestEntity request in RequestList.Entities)
                {
                    if (string.Equals(request.Id, InitialCurrentRequestId))
                    {
                        RequestList.Current = request;
                        return;
                    }
                }
            }
        }

        public void OnAddRequest()
        {
            _logger.Debug("OnAddRequest");

            RequestEntity request = null;
            try
            {
                request = RequestEntity.Create();
                OpenRequestEditForm(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);                
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час створення зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }

        public void OnEditRequest()
        {
            _logger.Debug("OnEditRequest");

            if (RequestList.Current == null) return;
            try
            {
                RequestEntity request = RefreshCurrentRequestFromDb();
                if (request == null) return;                
                OpenRequestEditForm(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);                
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час редагування зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }

        public void OnRemoveRequest()
        {
            _logger.Debug("OnRemoveRequest");

            RequestEntity request = RequestList.Current;
            if (request == null) return;
            if (_messageBoxMgr.ShowDialog("Ви бажаєте видалити зверення?") == true)
            {
                try
                {                    
                    _mainController.RemoveRequest(request.Id, true);                    
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час видалення зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
                }
            }
        }

        public void OnCloneRequest()
        {
            _logger.Debug("OnCloneRequest");

            if (RequestList.Current == null) return;
            try
            {
                RequestEntity request = RefreshCurrentRequestFromDb();
                if (request == null) return;

                request = request.Clone();
                request.MarkAsNew();                
                OpenRequestEditForm(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час клонування зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }        

        public void OnRefreshRequestList()
        {
            _logger.Debug("OnRefreshRequestList");

            try
            {
                BindingCollection<RequestEntity> requestList =  _mainController.GetRequstList(Filter);
                RequestList.Fill(requestList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час запиту реєстра зверенннь.", Hlp.GetExceptionText(ex), "Помилка", null);
            }            
        }

        public void OnEditFilter()
        {
            _logger.Debug("OnEditFilter");

            EditFilter(Filter);
        }        

        public void OnAddFilter()
        {
            _logger.Debug("OnAddFilter");

            EditFilter(null);   
        }       

        public void OnDeleteFilter()
        {
            _logger.Debug("OnDeleteFilter");

            if (Filter.IsNewEntity) return;

            if (_messageBoxMgr.ShowDialog(string.Format("{0} {1}", Properties.Resources.DeleteFilterMessage, Filter.FilterName)) == true)
            {
                try
                {
                    _mainController.OnDeleteFilter(Filter);
                    InitFilterList();
                    Filter = Filters[0];
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    _messageBoxMgr.ShowErrorWithDetail("Помилка під час видалення фільтру", ex);
                }
            }
        }

        #endregion Specific


        #region IDispose

        public void Dispose()
        {
            using (_control) { }

            _eventMgr.GetEvent<RequestChangedEvent>().Unsubscribe(OnRequestChanged);
        }
        
        #endregion IDispose



        #region private
        
        ILogMgr _logMgr;
        ILogger _logger;
        ICommonService _commonDbService;
        INsiService _nsiService;
        IViewFormMgr _viewFormMgr;
        IMessageBoxMgr _messageBoxMgr;
        IMainController _mainController;
        IEventMgr _eventMgr;

        EntityList<RequestEntity> _requestList;
        RequestListFilterEntity _filter;
        bool _readOnly;        
        List<IdentKey> _requestIdentCash;



        private void InitView()
        {            
            _caption = "Звернення";
            _hint = "Реєстр зверненнь";
            _image = Properties.Resources.Request;
            _requestIdentCash = new List<IdentKey>();
            _requestList = new EntityList<RequestEntity>();
            _control = new RequestListControl(this, _readOnly);

            _eventMgr.GetEvent<RequestChangedEvent>().Subscribe(OnRequestChanged);

            InitFilterList();
        }

        RequestEntity RefreshCurrentRequestFromDb()
        {
            RequestEntity request = _mainController.GetRequestById(RequestList.Current.Id);
            if (request == null)
            {
                _messageBoxMgr.ShowError(string.Format("Звернення з номером №{0} відсутнє в базі даних."));
                return null;
            }
            RequestList.Current = request;
            return request;
        }

        void OpenRequestEditForm(RequestEntity request)
        {
            _mainController.OpenRequestEditForm(request, false);
            _requestIdentCash.Add(request.CloneKey);
        }

        void OnRequestChanged(RequestChangedMessage message)
        {
            RequestEntity request = null;
            if (message.ChangeType == RequestChangeType.Create)
            {
                request = message.Request.Clone();
                request.IsPersisit = !_mainController.IsRequestFiltered(request, _filter);
                RequestList.Entities.Add(request);
            }
            
            else if (message.ChangeType == RequestChangeType.Update)
            {
                if (_mainController.IsRequestFiltered(message.Request, _filter))
                {
                    int index = RequestList.Entities.IndexOf(message.Request);
                    if (index != -1)
                    {
                        request = message.Request.Clone();
                        RequestList.Entities[index] = request;
                        RequestList.Current = request;
                    }
                }
            }

            if (message.ChangeType == RequestChangeType.Delete)
            {
                RequestList.Entities.Remove(message.Request);
            }
            
            _requestIdentCash.Remove(message.Request.CloneKey);
        }

        private bool Validate()
        {
            return true;
        }
        
        void InitFilterList()
        {
            Filters.Clear();
            foreach (RequestListFilterEntity flt in _mainController.Filters)
            {
                Filters.Add(flt);
            }
        }

        void EditFilter(RequestListFilterEntity filter)
        {
            try
            {
                RequestListFilterEntity newFilter = null;
                newFilter = _mainController.OpenRequestListFilterEditDialog(filter);
                if (newFilter != null)
                {
                    _mainController.OnSaveFilter(newFilter);
                    InitFilterList();
                    Filter = newFilter;
                    OnRefreshRequestList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час редагування фільтру.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }

        #endregion private
    }
}
