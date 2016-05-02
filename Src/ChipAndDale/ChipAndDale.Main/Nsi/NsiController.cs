using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ChipAndDale.Main.Db;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Nsi;
using Core.SDK.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Dom;
using Core.SDK.Log;
using Core.UtilsModule;

namespace ChipAndDale.Main.Nsi
{    
    internal class NsiController : INsiController
    {
        internal NsiController()
        {
            _logMgr =  ServiceMgr.Current.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger("NsiController");            
            _viewFormMgr = ServiceMgr.Current.GetInstance<IViewFormMgr>();
            _messageBoxMgr = ServiceMgr.Current.GetInstance<IMessageBoxMgr>();
            _nsiDbAccessor = ServiceMgr.Current.GetInstance<INsiDbAccessor>();
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: IViewFormMgr = {0}; IMessageBoxMgr = {1}; INsiDbAccessor = {2};", _viewFormMgr.ToStateString(), _messageBoxMgr.ToStateString(), _nsiDbAccessor.ToStateString());

            Init();
        }


        public void Refresh()
        {
            _logger.Debug("Refresh");

            try
            {
                _appList.Fill(_nsiDbAccessor.GetApps());
                _orgList.Fill(_nsiDbAccessor.GetOrgs());
                _tagList.Fill(_nsiDbAccessor.GetTags());
                _userList.Fill(_nsiDbAccessor.GetUsers());
            }
            catch (Exception ex)
            {
                throw new NsiRefreshExceptions(ex);
            }
        }
        
        #region users

        public void OpenUsersEditDialog()
        {
            _logger.Debug("OpenUsersEditDialog");

            try
            {
                _userList.Fill(_nsiDbAccessor.GetUsers());
                EntityList<UserEntity> newUserList = _userList.Clone();
                _userControl.SetUserEntityList(newUserList);
                _userControl.ReadOnly = false;
                _viewFormMgr.ShowModal(_userView, null);
            }
            catch (Exception ex)
            {                
                _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником користувачів.", ex);
            }
        }

        public IEnumerable<UserEntity> OpenUsersDialog()
        {
            _logger.Debug("OpenUsersDialog");

            _userList.Fill(_nsiDbAccessor.GetUsers());
            List<UserEntity> list = new List<UserEntity>();
            _userControl.SetUserEntityList(_userList);
            _userControl.ReadOnly = true;
            DialogResult result = _viewFormMgr.ShowModal(_userView, null);
            if (result == DialogResult.OK && _userList.Current != null) list.Add(_userList.Current);                        
            return list;
        }

        public BindingCollection<UserEntity> Users
        {
            get { return _userList.Clone().Entities; }
        }

        public IEnumerable<UserEntity> GetUserByName(string name)
        {
            return from user in _userList.Entities
                   where string.Equals(user.Name, name)
                   select user;
        }

        public UserEntity GetUserById(string id)
        {
            var result = from user in _userList.Entities where string.Equals(user.Id, id) select user;

            foreach (UserEntity user in result) return user;
            return null;
        }

        public UserEntity GetUserByLogin(string login)
        {
            if (string.IsNullOrEmpty(login)) return null;

            var result = from user in _userList.Entities where string.Equals(user.Login.ToUpper(), login.ToUpper()) select user;

            foreach (UserEntity user in result) return user;
            return null;
        }

        #endregion users


        #region app

        public void OpenAppsEditDialog()
        {
            _logger.Debug("OpenAppsEditDialog");

            try
            {
                EntityList<AppEntity> newAppList = _appList.Clone();
                _appControl.SetAppEntityList(newAppList);
                _viewFormMgr.ShowModal(_appView, null);                    
            }
            catch (Exception ex)
            {
                _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником програмних систем.", ex);
            }
        }

        public IEnumerable<AppEntity> OpenAppsDialog()
        {
            _logger.Debug("OpenAppsDialog");

            List<AppEntity> list = new List<AppEntity>();
            _appControl.SetAppEntityList(_appList);
            _appControl.ReadOnly = true;
            DialogResult result = _viewFormMgr.ShowModal(_appView, null);
            if (result == DialogResult.OK && _appList.Current != null) list.Add(_appList.Current);
            return list;
        }

        public BindingCollection<AppEntity> Applications
        {
            get { return _appList.Clone().Entities; }
        }

        public IEnumerable<AppEntity> GetAppByName(string name)
        {
            return from app in _appList.Entities
                   where string.Equals(app.Name, name)
                   select app;
        }

        public AppEntity GetAppById(string id)
        {
            var result = from app in _appList.Entities where string.Equals(app.Id, id) select app;

            foreach (AppEntity app in result) return app;
            return null;
        }

        #endregion app


        #region org

        public void OpenOrgsEditDialog()
        {
            _logger.Debug("OpenOrgsEditDialog");

            try
            {
                EntityList<OrgEntity> newOrgList = _orgList.Clone();
                _orgControl.SetOrgEntityList(newOrgList);
                _orgControl.ReadOnly = false;
                _viewFormMgr.ShowModal(_orgView, null); 
            }
            catch (Exception ex)
            {
                _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником організацій.", ex);
            }
        }

        public IEnumerable<OrgEntity> OpenOrgsDialog()
        {
            _logger.Debug("OpenOrgsDialog");

            List<OrgEntity> list = new List<OrgEntity>();
            _orgControl.SetOrgEntityList(_orgList);
            _orgControl.ReadOnly = true;
            DialogResult result = _viewFormMgr.ShowModal(_orgView, null);
            if (result == DialogResult.OK && _orgList.Current != null) list.Add(_orgList.Current);
            return list;
        }

        public BindingCollection<OrgEntity> Organizations
        {
            get { return _orgList.Clone().Entities; }
        }

        public IEnumerable<OrgEntity> GetOrgByName(string name)
        {
            return from org in _orgList.Entities
                   where string.Equals(org.Name, name)
                   select org;
        }

        public OrgEntity GetOrgById(string id)
        {
            var result = from org in _orgList.Entities where string.Equals(org.Id, id) select org;

            foreach (OrgEntity org in result) return org;
            return null;
        }        

        #endregion org


        #region tag

        public void OpenTagsEditDialog()
        {
            _logger.Debug("OpenTagsEditDialog");

            try
            {
                EntityList<TagEntity> newTagList = _tagList.Clone();
                _tagControl.SetTagEntityList(newTagList);
                _tagControl.ReadOnly = false;
                _viewFormMgr.ShowModal(_tagView, null); 
            }
            catch (Exception ex)
            {
                _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником тегів.", ex);
            }
        }

        public IEnumerable<TagEntity> OpenTagsDialog()
        {
            _logger.Debug("OpenTagsDialog");

            List<TagEntity> list = new List<TagEntity>();
            _tagControl.SetTagEntityList(_tagList);
            _tagControl.ReadOnly = true;
            DialogResult result = _viewFormMgr.ShowModal(_tagView, null);
            if (result == DialogResult.OK && _tagList.Current != null) list.Add(_tagList.Current);
            return list;
        }

        public BindingCollection<TagEntity> Tags
        {
            get { return _tagList.Clone().Entities; }
        }

        public IEnumerable<TagEntity> GetTagByName(string name)
        {
            return from tag in _tagList.Entities
                   where string.Equals(tag.Name, name)
                   select tag;
        }

        public TagEntity GetTagById(string id)
        {
            var result = from tag in _tagList.Entities where string.Equals(tag.Id, id) select tag;

            foreach (TagEntity tag in result) return tag;
            return null;
        }        

        #endregion tag


        public IList<KeyValuePair<Enum, string>> InfoSourceTypes
        {
            get
            {
                return typeof(InfoSourceType).EnumToPairIList();
            }
        }


        #region private

        INsiDbAccessor _nsiDbAccessor;
        ILogMgr _logMgr;
        ILogger _logger;
        IViewFormMgr _viewFormMgr; 
        IMessageBoxMgr _messageBoxMgr;
        EntityList<UserEntity> _userList;
        EntityList<AppEntity> _appList;
        EntityList<OrgEntity> _orgList;
        EntityList<TagEntity> _tagList;        
        UserListControl _userControl;
        AppListControl _appControl;
        OrgListControl _orgControl;
        TagListControl _tagControl;
        Core.SDK.Composite.UI.View _userView;
        Core.SDK.Composite.UI.View _appView;
        Core.SDK.Composite.UI.View _orgView;
        Core.SDK.Composite.UI.View _tagView;

        Dictionary<InfoSourceType, string> _infoSourceTypesDict;

        private void Init()
        {
            _logger.Debug("Init");

            _userList = new EntityList<UserEntity>();
            _userList.Caption = "Довідник користувачів";
            _userControl = new UserListControl();
            _userView = new Core.SDK.Composite.UI.View(_userControl, _userList.Caption, (state) =>
            {
                _userList.Current = _userControl.UserEntityList.Current;
                if (state != true) return true;
                try
                {
                    BindingCollection<UserEntity> forAdd = EntityList<UserEntity>.DiffTwoListAndGetNew(_userList, _userControl.UserEntityList);
                    BindingCollection<UserEntity> forUpd = EntityList<UserEntity>.DiffTwoListAndGetUpd(_userList, _userControl.UserEntityList);
                    BindingCollection<UserEntity> forDel = EntityList<UserEntity>.DiffTwoListAndGetDel(_userList, _userControl.UserEntityList);

                    if (forAdd.Count == 0 && forUpd.Count == 0 && forDel.Count == 0) return true;

                    _nsiDbAccessor.ProccessUserChanges(forAdd, forUpd, forDel);
                    _userList = _userControl.UserEntityList;
                    return true;
                }
                catch (Exception ex)
                {
                    _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником користувачів.", ex);
                    return false;
                }
            });
            _userView.Image = Properties.Resources.NsiUser;

            _appList = new EntityList<AppEntity>();
            _appList.Caption = "Довідник додатків";
            _appControl = new AppListControl();
            _appView = new Core.SDK.Composite.UI.View(_appControl, _appList.Caption, (state) =>
            {
                _appList.Current = _appControl.AppEntityList.Current;
                if (state != true) return true;
                try
                {
                    BindingCollection<AppEntity> forAdd = EntityList<AppEntity>.DiffTwoListAndGetNew(_appList, _appControl.AppEntityList);
                    BindingCollection<AppEntity> forUpd = EntityList<AppEntity>.DiffTwoListAndGetUpd(_appList, _appControl.AppEntityList);
                    BindingCollection<AppEntity> forDel = EntityList<AppEntity>.DiffTwoListAndGetDel(_appList, _appControl.AppEntityList);

                    if (forAdd.Count == 0 && forUpd.Count == 0 && forDel.Count == 0) return true;

                    _nsiDbAccessor.ProccessAppChanges(forAdd, forUpd, forDel);
                    _appList = _appControl.AppEntityList;                    
                    return true;
                }
                catch (Exception ex)
                {
                    _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником програмних систем.", ex);
                    return false;
                }
            });
            _appView.Image = Properties.Resources.NsiGeneral;

            _orgList = new EntityList<OrgEntity>();
            _orgList.Caption = "Довідник організацій";
            _orgControl = new OrgListControl();
            _orgView = new Core.SDK.Composite.UI.View(_orgControl, _orgList.Caption, (state) =>
            {
                _orgList.Current = _orgControl.OrgEntityList.Current;
                if (state != true) return true;
                try
                {
                    BindingCollection<OrgEntity> forAdd = EntityList<OrgEntity>.DiffTwoListAndGetNew(_orgList, _orgControl.OrgEntityList);
                    BindingCollection<OrgEntity> forUpd = EntityList<OrgEntity>.DiffTwoListAndGetUpd(_orgList, _orgControl.OrgEntityList);
                    BindingCollection<OrgEntity> forDel = EntityList<OrgEntity>.DiffTwoListAndGetDel(_orgList, _orgControl.OrgEntityList);

                    if (forAdd.Count == 0 && forUpd.Count == 0 && forDel.Count == 0) return true;

                    _nsiDbAccessor.ProccessOrgChanges(forAdd, forUpd, forDel);
                    _orgList = _orgControl.OrgEntityList;
                    return true;
                }
                catch (Exception ex)
                {
                    _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником організацій.", ex);
                    return false;
                }
            });
            _orgView.Image = Properties.Resources.NsiGeneral;
            
            _tagControl = new TagListControl();
            _tagList = new EntityList<TagEntity>();
            _tagList.Caption = "Довідник тегів";
            _tagView = new Core.SDK.Composite.UI.View(_tagControl, _tagList.Caption, (state) =>
            {
                _tagList.Current = _tagControl.TagEntityList.Current;
                if (state != true) return true;
                try
                {
                    BindingCollection<TagEntity> forAdd = EntityList<TagEntity>.DiffTwoListAndGetNew(_tagList, _tagControl.TagEntityList);
                    BindingCollection<TagEntity> forUpd = EntityList<TagEntity>.DiffTwoListAndGetUpd(_tagList, _tagControl.TagEntityList);
                    BindingCollection<TagEntity> forDel = EntityList<TagEntity>.DiffTwoListAndGetDel(_tagList, _tagControl.TagEntityList);

                    if (forAdd.Count == 0 && forUpd.Count == 0 && forDel.Count == 0) return true;

                    _nsiDbAccessor.ProccessTagChanges(forAdd, forUpd, forDel);
                    _tagList = _tagControl.TagEntityList;
                    return true;
                }
                catch (Exception ex)
                {
                    _messageBoxMgr.ShowErrorWithDetail("Помилка під час роботи з довідником тегів.", ex);
                    return false;
                }
            });
            _tagView.Image = Properties.Resources.NsiGeneral;

            _infoSourceTypesDict = new Dictionary<InfoSourceType, string>();
            _infoSourceTypesDict.Add(InfoSourceType.Call, Properties.Resources.PhoneInfoSourceType);
            _infoSourceTypesDict.Add(InfoSourceType.Email, Properties.Resources.MailInfoSourceType);
            _infoSourceTypesDict.Add(InfoSourceType.Forum, Properties.Resources.ForumInfoSourceType);
            _infoSourceTypesDict.Add(InfoSourceType.Jabber, Properties.Resources.JabberSourceType);
            _infoSourceTypesDict.Add(InfoSourceType.Letter, Properties.Resources.LetterInfoSourceType);
        }        

        #endregion private
    }
}
