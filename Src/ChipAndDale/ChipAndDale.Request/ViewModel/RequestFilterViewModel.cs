using System;
using System.Collections.Generic;
using ChipAndDale.Request.UI;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Nsi;
using ChipAndDale.SDK.Request;
using Core.SDK.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Dom;
using Core.SDK.Log;
using Core.UtilsModule;

namespace ChipAndDale.Request.ViewModel
{    
    internal class RequestFilterViewModel : ViewBase<RequestFilterControl>, IDisposable
    {
        public RequestFilterViewModel(RequestListFilterEntity filter, IMainController mainController)
        {
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();
            _commonDbService = ServiceMgr.Current.GetInstance<ICommonService>();
            _nsiService = ServiceMgr.Current.GetInstance<INsiService>();
            _messageBoxMgr = ServiceMgr.Current.GetInstance<IMessageBoxMgr>();
            _logger = _logMgr.GetLogger("RequestListFilterViewModel");
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: ICommonDbService = {0}; INsiService = {1}; IMessageBoxMgr = {2}.", _commonDbService.ToStateString(), _nsiService.ToStateString(), _messageBoxMgr.ToStateString());                       
            
            if (mainController == null) throw new ArgumentNullException("MainController param can not be null.");                      
            _mainController = mainController;
            _filterOrigin = filter;  
               
            InitView();            
        }        

        #region IView

        public override bool CanClose
        {
            get { return Validate(); }
        }

        public override bool OnClosing(bool? result)
        {
            DialogResult = result;
            return true;            
        }

        public override void OnAfterClose()
        {            
        }

        public override void OnActivate(bool isActive)
        { }                

        #endregion IView
        
        
        #region Model
        
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

        public BindingCollection<AppEntity> Applications
        {
            get { return _appList; }
        }

        public BindingCollection<OrgEntity> Organizations
        {
            get { return _orgList; }
        }

        public BindingCollection<UserEntity> Users
        {
            get { return _userList; }
        }

        public BindingCollection<TagEntity> Tags
        {
            get { return _tagList; }
        }

        public IList<KeyValuePair<Int32, string>> RequestStates
        {
            get { return EnumHelper.EnumToPairInt32IList(typeof(RequestState)); }
        }

        public IList<KeyValuePair<Enum, string>> InfoSourceTypes
        {
            get { return _nsiService.InfoSourceTypes; }
        }

        bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            private set { _dialogResult = value; }
        }

        public void OnCloneFilter(RequestListFilterEntity filter)
        {
            if (filter == null) return;

            RequestListFilterEntity newFilter = filter.Clone();
            newFilter.FilterName = Filter.FilterName;
            newFilter.CloneKey = Filter.CloneKey;
            newFilter.Id = Filter.Id;
            Filter = newFilter;
        }

        #endregion model


        #region IDispose
        
        public void Dispose()
        {
            using (_control) { }
        }

        #endregion IDispose


        #region private

        ILogMgr _logMgr;
        ILogger _logger;
        ICommonService _commonDbService;
        INsiService _nsiService;
        IMessageBoxMgr _messageBoxMgr;
        IMainController _mainController;
        
        RequestListFilterEntity _filter;
        RequestListFilterEntity _filterOrigin;     

        BindingCollection<AppEntity> _appList;
        BindingCollection<OrgEntity> _orgList;
        BindingCollection<UserEntity> _userList;
        BindingCollection<TagEntity> _tagList;



        private void InitView()
        {
            _appList = _nsiService.Applications;
            _appList.Add(AppEntity.Create());

            _orgList = _nsiService.Organizations;
            _orgList.Add(OrgEntity.Create());

            _userList = _nsiService.Users;
            _userList.Add(UserEntity.Create());

            _tagList = _nsiService.Tags;            

            _dialogResult = null;
            _control = new RequestFilterControl(this);

            _caption = string.Format("Параметри пошуку звернень", "");
            _hint = string.Format("Параметри пошуку звернень", "");

            _image = Properties.Resources.Request;

            _filterList = new List<RequestListFilterEntity>();
            foreach (RequestListFilterEntity filter in _mainController.Filters)
            {
                _filterList.Add(filter.Clone());
            }
            if (_filterOrigin == null) Filter = RequestListFilterEntity.Create();
            else Filter = _filterOrigin.Clone();
        }        

        private bool Validate()
        {
            return true;
        }

        string GenerateName()
        {
            return string.Format("RequestViewModel{0}", DateTime.Now.ToString("ddMMyyHHmmssss"));
        }

        #endregion private       
    }
}
