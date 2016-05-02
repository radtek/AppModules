using System;
using System.Threading;
using ChipAndDale.Main.Db;
using ChipAndDale.Main.Nsi;
using ChipAndDale.Main.Properties;
using ChipAndDale.Main.UI;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Nsi;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.UI;
using Core.SDK.Db;
using Core.SDK.Log;
using Core.SDK.Composite.Event;
using ChipAndDale.Main.EventMessage;

namespace ChipAndDale.Main
{
    public class MainController : IMainController
    {
        public MainController()
        {            
            IServiceMgr serviceMgr = ServiceMgr.Current;
            _logMgr = serviceMgr.GetInstance<ILogMgr>();
            _connection = serviceMgr.GetInstance<IDbConnection>();
            _messageBoxMgr = serviceMgr.GetInstance<IMessageBoxMgr>();
            _viewFormMgr = serviceMgr.GetInstance<IViewFormMgr>();
            _commonDbAccessor = serviceMgr.GetInstance<ICommonDbAccessor>();
            _nsiController = serviceMgr.GetInstance<INsiController>();
            _eventMgr = serviceMgr.GetInstance<IEventMgr>();
            _logger = _logMgr.GetLogger("MainController");
            _logger.Debug("Create.");

            _isFirstConnect = true;
            _currentUser = UserEntity.Create();
        }       

        public void Init()
        {
            _logger.Debug("Init");
            TryConnectAndAuthorizate("mbazhanov", "123");
            ProcessAfterConnect("mbazhanov");            
        }


        #region For MainForm

        public void OnConnect()
        {
            _logger.Debug("OnConnect");

            try
            {
                using (DbConnectControl connectControl = new DbConnectControl("mbazhanov", "123"))
                {
                    View view = new View(connectControl, "Авторизація користувача", (bool? result) => 
                                    {
                                        if (result == true)
                                        {
                                            bool connResult = false;
                                            ServiceMgr.Current.GetInstance<IMessageBoxMgr>().ShowAndHideWaitScreen(() => { connResult = TryConnectAndAuthorizate(connectControl.Login, connectControl.Password); }, "Зачекайте будь ласка", "Авторизація...");                                            
                                            return connResult;
                                        }
                                        else return true;
                                    });
                    view.Image = Properties.Resources.Ok_18;
                    if (_viewFormMgr.ShowModal(view, DialogFormOption.FixedSize) == System.Windows.Forms.DialogResult.OK)
                        ServiceMgr.Current.GetInstance<IMessageBoxMgr>().ShowAndHideWaitScreen(() => { ProcessAfterConnect(connectControl.Login); }, "Зачекайте будь ласка", "Завантаження даних...");                   
                }
            }                     
            catch (NsiRefreshExceptions nsiEx)
            {
                _messageBoxMgr.ShowErrorWithDetail(nsiEx.Message, nsiEx);
            }
            catch (Exception ex)
            {
                _messageBoxMgr.ShowErrorWithDetail(ex);
            }
        }               

        public void OnReconnect()
        {
            throw new NotImplementedException();
        }

        public void OnDisconnect()
        {
            try
            {
                _connection.CloseConnection();
                _currentUser = null;
            }
            catch (Exception ex)
            {
                _messageBoxMgr.ShowErrorWithDetail(ex);
            }
        }

        public void OnLoadSetting()
        {
            _logger.Debug("OnLoadSetting");
            FireLoadSettingEvent();
        }

        public void OnSaveSetting()
        {
            _logger.Debug("OnSaveSetting");
            FireSaveSettingEvent();
        }

        #endregion For MainForm


        #region ICommonService

        public void SetParam(string name, string value)
        {
            _commonDbAccessor.SetParam(name, value);
        }

        public void ClearAllParams(bool isCommit) 
        { }

        public void SendMessage(MessageEntity message, bool isCommit) 
        { }

        public bool IsConnect 
        { 
            get { return _connection.IsOpenConnection; } 
        }

        public bool IsConnectWithMessage
        {
            get 
            {                 
                bool result = _connection.IsOpenConnection;
                if (!result) _messageBoxMgr.ShowError(Properties.Resources.NoDbConnectionMessage);
                return result;
            }
        }

        public UserEntity User 
        { 
            get { return _currentUser; }
        }

        public string Login
        {
            get { return _currentUser.Login; }
        }

        #endregion ICommonService


        #region private

        IDbConnection _connection;
        ILogMgr _logMgr;
        ILogger _logger;        
        ICommonDbAccessor _commonDbAccessor;
        INsiController _nsiController;
        IMessageBoxMgr _messageBoxMgr;
        IViewFormMgr _viewFormMgr;
        IEventMgr _eventMgr;

        bool _isFirstConnect;

        UserEntity _currentUser;


        #region Connect and Authorizate

        private void DomainAuthorization(string userName, string pasword)
        {
            throw new NotImplementedException();
        }

        bool TryConnectAndAuthorizate(string userName, string pasword)
        {
            try
            {
                _connection.OpenConnection(Settings.Default.DbUser, Settings.Default.DbPassword, Settings.Default.DbAliase);
                //DomainAuthorization(userName, pasword);
                _commonDbAccessor.AuthorizateUser(userName);
                return true;
            }
            catch (ConnectDbException oraEx)
            {
                _messageBoxMgr.ShowErrorWithDetail("Помилка під час підключення до БД", oraEx);
            }
            catch (AuthorizateExceptions authEx)
            {
                _messageBoxMgr.ShowErrorWithDetail(authEx.Message, authEx);
            }
            return false;
        }

        private void ProcessAfterConnect(string login)
        {
            _nsiController.Refresh();
            _currentUser = _nsiController.GetUserByLogin(login);

            if (_isFirstConnect)
            {
                _isFirstConnect = false;
                FireFirstDbConnectEvent();
            }
        }

        #endregion Connect and Authorizate


        #region Fired events

        void FireLoadSettingEvent()
        {
            if (!IsConnectWithMessage) return;

            ServiceMgr.Current.GetInstance<IMessageBoxMgr>().ShowAndHideWaitScreen(() =>
            { _eventMgr.GetEvent<MainFormLoadSettingEvent>().Publish(null); },
                "Зачекайте, будь ласка",
                "Завантаження настроювань");
        }

        void FireSaveSettingEvent()
        {
            if (!IsConnectWithMessage) return;

            ServiceMgr.Current.GetInstance<IMessageBoxMgr>().ShowAndHideWaitScreen(() =>
            { _eventMgr.GetEvent<MainFormSaveSettingEvent>().Publish(null); },
                "Зачекайте, будь ласка",
                "Збереження настроювань");
        }

        void FireFirstDbConnectEvent()
        {
            _eventMgr.GetEvent<FirstDbConnectEvent>().Publish(new FirstDbConnectMessage());
        }

        #endregion Fired events


        

        #endregion private
    }
}
