using System;
using System.Collections.Generic;
using ChipAndDale.SDK.Nsi;
using ChipAndDale.SDK.Request;
using Core.SDK.Composite.Service;
using Core.SDK.Common;
using Core.SDK.Log;
using Core.SDK.Composite.Event;
using ChipAndDale.Request.ViewModel;
using Core.SDK.Setting;
using ChipAndDale.SDK.Common;
using Core.SDK.Db;
using Core.SDK.Composite.Common;

namespace ChipAndDale.Request.Setting
{
    internal class SettingController : ISettingController
    {
        internal SettingController(IMainController mainController)
        {
            _mainController = mainController;
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("Request.SettingController");
            _eventMgr = ServiceMgr.Current.GetInstance<IEventMgr>();                        
            _nsiService = ServiceMgr.Current.GetInstance<INsiService>();
            _settingMgr = ServiceMgr.Current.GetInstance<ISettingMgr>();
            _commonService = ServiceMgr.Current.GetInstance<ICommonService>();
            _dbMgr = ServiceMgr.Current.GetInstance<IDbMgr>();
            _logger.Debug("Create.");
            //_logger.Debug("Interfaces: IEventMgr = {0}; ICommonDbService = {1}; INsiService = {2}; ISettingMgr = {3}; IRequestService = {4}.", _dbMgr.ToStateString(), _commonDbService.ToStateString(), _dialogMgr.ToStateString(), _regionMgr.ToStateString(), _eventMgr.ToStateString());
        }        


        #region ISettingController
        readonly string RootSettingName = "RootSetting";
        public void Load()
        {
            Init();

            RootSetting rootSetting = new RootSetting();
            LoadSetting<RootSetting>(RootSettingName, rootSetting);

            if (rootSetting.FilterList != null)
                foreach (string filterName in rootSetting.FilterList)
                {
                    FilterEntitySetting filterSetting = new FilterEntitySetting();
                    LoadSetting<FilterEntitySetting>(filterName, filterSetting);
                    _filterEntityList.Add(SettingToFilter(filterSetting));
                }

            if (rootSetting.EditViewModelList != null)
                foreach (string editVMName in rootSetting.EditViewModelList)
                {
                    EditViewModelSetting editVMSetting = new EditViewModelSetting();
                    LoadSetting<EditViewModelSetting>(editVMName, editVMSetting);
                    _requestEditVMList.Add(SettingToRequestEditVM(editVMSetting));
                }

            if (rootSetting.ListViewModelList != null)
                foreach (string ListVMName in rootSetting.ListViewModelList)
                {
                    ListViewModelSetting listVMSetting = new ListViewModelSetting();
                    LoadSetting<ListViewModelSetting>(ListVMName, listVMSetting);
                    RequestListViewModel viewModel = SettingToRequestListVM(listVMSetting);
                    viewModel.Name = ListVMName;
                    _requestListVMList.Add(viewModel);
                }
        }

        public void Save()
        {
            using (DbTransaction transaction = new DbTransaction(_dbMgr))
            {
                Clear();

                RootSetting rootSetting = new RootSetting();
                
                if (_filterEntityList != null)
                    foreach (RequestListFilterEntity filter in _filterEntityList)
                    {
                        FilterEntitySetting filterSetting = FilterToSetting(filter);
                        SaveSetting(filter.InternalName, filterSetting);
                        rootSetting.FilterList.Add(filter.InternalName);
                    }

                if (_requestEditVMList != null)
                    foreach (RequestEditViewModel viewModel in _requestEditVMList)
                    {
                        EditViewModelSetting viewModelSetting = RequstEditVMToSetting(viewModel);
                        SaveSetting(viewModel.Name, viewModelSetting);
                        rootSetting.EditViewModelList.Add(viewModel.Name);
                    }
                if (_requestListVMList != null)
                    foreach (RequestListViewModel viewModel in _requestListVMList)
                    {
                        ListViewModelSetting viewModelSetting = RequestListVMToSetting(viewModel);
                        SaveSetting(viewModel.Name, viewModelSetting);
                        rootSetting.ListViewModelList.Add(viewModel.Name);
                    }

                SaveSetting(RootSettingName, rootSetting);
                transaction.Success = true;
            }
        }

        public void SetToDefault()
        {
            throw new NotImplementedException();
        }

        public List<RequestListFilterEntity> Filters
        {
            get { return _filterEntityList; }
            set { _filterEntityList = value; }
        }

        public List<RequestListViewModel> ListViewModels
        {
            get { return _requestListVMList; }
            set { _requestListVMList = value; }
        }

        public List<RequestEditViewModel> EditViewModels
        {
            get { return _requestEditVMList; }
            set { _requestEditVMList = value;}
        }

        public string SaveRequestToSetting(RequestEntity request)
        {
            return "";
        }

        public RequestEntity LoadRequestFromSetting(string settingName)
        {
            return null;
        }

        #endregion ISettingController



        

        #region Private

        IMainController _mainController;
        ILogMgr _logMgr;
        ILogger _logger;        
        IEventMgr _eventMgr;        
        INsiService _nsiService;
        ISettingMgr _settingMgr;
        ICommonService _commonService;
        IDbMgr _dbMgr;

        List<RequestEditViewModel> _requestEditVMList;
        List<RequestListViewModel> _requestListVMList;
        List<RequestListFilterEntity> _filterEntityList;


        #region Converters

        RequestEntitySetting RequestToSetting(RequestEntity request)
        {
            RequestEntitySetting setting = new RequestEntitySetting();
            setting.Id = request.Id;
            setting.State = request.State;
            setting.CreateDateTime = request.CreateDateTime;
            setting.RequestDateTime = request.ReqDateTime;
            setting.OrganizationId = request.Organization.Id;
            setting.ApplicationId = request.Application.Id;
            setting.ResponseId = request.ResponseUser.Id;
            setting.CreatorId = request.CreatorUser.Id;
            setting.TagIdList = request.TagIdList;
            setting.Subject = request.Subject;
            setting.Comments = request.Comments;
            setting.Contact = request.Contact;
            setting.InfoSourceType = request.InfoSourceType;
            setting.BugNumber = request.BugNumber;
            setting.CMVersion = request.CMVersion;
            setting.ComponentVersion = request.ComponentVersion;
            setting.IsImportant = request.IsImportant;

            return setting;
        }

        RequestEntity SettingToRequest(RequestEntitySetting setting)
        {
            RequestEntity request = RequestEntity.Create();
            request.Id = setting.Id;
            request.ReqDateTime = setting.RequestDateTime;
            request.CreateDateTime = setting.CreateDateTime;
            request.Subject = setting.Subject;
            request.Comments = setting.Comments;
            request.Contact = setting.Contact;

            AppEntity app = _nsiService.GetAppById(setting.ApplicationId);
            request.Application = app == null ? AppEntity.Create() : app;

            OrgEntity org = _nsiService.GetOrgById(setting.OrganizationId);
            request.Organization = org == null ? OrgEntity.Create() : org;

            UserEntity user = _nsiService.GetUserById(setting.ResponseId);
            request.ResponseUser = user == null ? UserEntity.Create() : user;

            user = _nsiService.GetUserById(setting.CreatorId);
            request.CreatorUser = user == null ? UserEntity.Create() : user;

            if (setting.TagIdList != null)
                foreach (string id in setting.TagIdList)
                {
                    TagEntity tag = _nsiService.GetTagById(id);
                    if (tag != null) request.Tags.Entities.Add(tag);
                    else _logger.Warn("Can not find tag with such id = {0}", id);
                }

            //request.Attaches = setting.Attaches.Clone();
            request.InfoSourceType = setting.InfoSourceType;
            request.State = setting.State;
            request.BugNumber = setting.BugNumber;
            request.CMVersion = setting.CMVersion;
            request.ComponentVersion = setting.ComponentVersion;
            request.IsImportant = setting.IsImportant;

            return request;
        }


        FilterEntitySetting FilterToSetting(RequestListFilterEntity filter)
        {
            FilterEntitySetting setting = new FilterEntitySetting();
            setting.CloneKey = filter.CloneKey.ToGuid();
            setting.FilterName = filter.FilterName;
            setting.StartDateTime = filter.StartDateTime;
            setting.StopDateTime = filter.StopDateTime;
            setting.OrganizationId = filter.Organization.Id;
            setting.ApplicationId = filter.Application.Id;
            setting.ResponseId = filter.ResponseUser.Id;
            setting.CreatorId = filter.CreatorUser.Id;
            setting.TagIdList = filter.TagIdList;
            setting.Subject = filter.Subject;
            setting.Comments = filter.Comments;
            setting.Contact = filter.Contact;
            setting.StatusIdList = filter.StatusIdList;

            return setting;
        }

        RequestListFilterEntity SettingToFilter(FilterEntitySetting setting)
        {
            RequestListFilterEntity filter = RequestListFilterEntity.Create();
            filter.CloneKey = new IdentKey(setting.CloneKey);
            filter.FilterName = setting.FilterName;
            filter.StartDateTime = setting.StartDateTime;
            filter.StopDateTime = setting.StopDateTime;

            UserEntity user = _nsiService.GetUserById(setting.ResponseId);
            filter.ResponseUser = user == null ? UserEntity.Create() : user;

            user = _nsiService.GetUserById(setting.CreatorId);
            filter.CreatorUser = user == null ? UserEntity.Create() : user;

            AppEntity app = _nsiService.GetAppById(setting.ApplicationId);
            filter.Application = app == null ? AppEntity.Create() : app;

            OrgEntity org = _nsiService.GetOrgById(setting.OrganizationId);
            filter.Organization = org == null ? OrgEntity.Create() : org;

            filter.Comments = setting.Comments;
            filter.Subject = setting.Subject;
            filter.Contact = setting.Contact;
            filter.TagIdList = setting.TagIdList;
            filter.StatusIdList = setting.StatusIdList;

            return filter;
        }


        EditViewModelSetting RequstEditVMToSetting(RequestEditViewModel viewModel)
        {
            EditViewModelSetting setting = new EditViewModelSetting();
            setting.Size = viewModel.DialogSetting.Size;
            setting.Position = viewModel.DialogSetting.Position;
            setting.IsCommit = viewModel.IsCommit;
            setting.IsProcess = viewModel.IsProcess;
            setting.ReadOnly = viewModel.ReadOnly;
            setting.RequestName = viewModel.Request.InternalName;
            RequestEntitySetting reqSetting = RequestToSetting(viewModel.Request);
            SaveSetting(setting.RequestName, reqSetting);

            return setting;
        }

        RequestEditViewModel SettingToRequestEditVM(EditViewModelSetting setting)
        {
            RequestEntitySetting requestSetting = new RequestEntitySetting();
            LoadSetting<RequestEntitySetting>(setting.RequestName, requestSetting);

            RequestEntity request = SettingToRequest(requestSetting);
            RequestEditViewModel viewModel = _mainController.CreateRequestEditViewModel(request, setting.ReadOnly, setting.IsProcess, setting.IsCommit);
            viewModel.DialogSetting = new Core.SDK.Composite.UI.DialogSetting(setting.Position, setting.Size);
            return viewModel;
        }



        ListViewModelSetting RequestListVMToSetting(RequestListViewModel viewModel)
        {
            ListViewModelSetting setting = new ListViewModelSetting();
            setting.CurrentRequestId = viewModel.RequestList.Current != null ? viewModel.RequestList.Current.Id : null;
            setting.ReadOnly = viewModel.ReadOnly;
            setting.GridData = viewModel.GridData;
            setting.CurrentFilterName = viewModel.Filter.InternalName;
            FilterEntitySetting filterSetting = FilterToSetting(viewModel.Filter);
            SaveSetting(setting.CurrentFilterName, filterSetting);

            return setting;
        }

        RequestListViewModel SettingToRequestListVM(ListViewModelSetting setting)
        {
            FilterEntitySetting filterSetting = new FilterEntitySetting();
            LoadSetting<FilterEntitySetting>(setting.CurrentFilterName, filterSetting);

            RequestListFilterEntity filter = SettingToFilter(filterSetting);
            RequestListViewModel viewModel = _mainController.CreateRequestListViewModel(filter, setting.ReadOnly);
            viewModel.InitialCurrentRequestId = setting.CurrentRequestId;
            
            return viewModel;
        }

        #endregion Converters


        void Init()
        {
            _requestEditVMList = new List<RequestEditViewModel>();
            _requestListVMList = new List<RequestListViewModel>();
            _filterEntityList = new List<RequestListFilterEntity>();
        }

        void SaveSetting(string settingName, object setting)
        {
            _settingMgr.SaveSetting(_commonService.Login, Properties.Settings.Default.RequestSection, settingName, setting, false);
        }

        void LoadSetting<T>(string settingName, T setting) where T : class
        {
            _settingMgr.LoadSetting(_commonService.Login, Properties.Settings.Default.RequestSection, settingName, setting);            
        }

        void Clear()
        {
            _settingMgr.ClearSettings(_commonService.Login, Properties.Settings.Default.RequestSection, false);            
        }

        #endregion Private
    }
}
