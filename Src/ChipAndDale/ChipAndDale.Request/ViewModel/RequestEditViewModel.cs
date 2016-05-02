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
using System.ComponentModel;
using Core.SDK.Composite.Event;
using ChipAndDale.SDK.Request.EventMessage;
using System.Windows.Forms;
using Core.UtilsModule;
using System.IO;

namespace ChipAndDale.Request.ViewModel
{
    internal class RequestEditViewModel : ViewBase<RequestEditControl>, IDisposable, INotifyPropertyChanged
    {
        public RequestEditViewModel(RequestEntity request, IMainController mainController, bool readOnly, bool isProcess, bool isCommit)
        {
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();
            _commonService = ServiceMgr.Current.GetInstance<ICommonService>();
            _nsiService = ServiceMgr.Current.GetInstance<INsiService>();
            _viewFormMgr = ServiceMgr.Current.GetInstance<IViewFormMgr>();
            _eventMgr = ServiceMgr.Current.GetInstance<IEventMgr>();
            _messageBoxMgr = ServiceMgr.Current.GetInstance<IMessageBoxMgr>();
            _logger = _logMgr.GetLogger("RequestViewModel");
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: ICommonDbService = {0}; INsiService = {1}; IViewFormMgr = {2}; IEventMgr = {3}; IMessageBoxMgr= {4}", _commonService.ToStateString(), _nsiService.ToStateString(), _viewFormMgr.ToStateString(), _eventMgr.ToStateString(), _messageBoxMgr.ToStateString());                       
            
            if (mainController == null) throw new ArgumentNullException("MainController param can not be null.");

            Request = request;
            /*_request = request != null ? request.Clone() : RequestEntity.Create();
            _request.ResetCloneKey();   // Разрываем зависимость для выявления изменений обращения в других формах редактирования*/
            Request.CreatorUser = _commonService.User;
            _mainController = mainController;
            _readOnly = readOnly;
            _isProcess = isProcess;
            _isCommit = isCommit;

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
            if (result != true) return true;
            else return (_mainController.ProcessRequestViewModel(this));            
        }

        public override void OnAfterClose()
        {
            _mainController.OnAfterCloseRequestViewModel(this);
        }

        public override void OnActivate(bool isActive)
        { }                

        #endregion IView
        
        
        #region Specific
        
        public RequestEntity Request
        {
            get { return _request; }
            protected set 
            {                
                _requestOrigin = value;
                RequestEntity oldRequest = _request;
                _request = _requestOrigin != null ? _requestOrigin.Clone() : RequestEntity.Create();
                _request.ResetCloneKey();

                if (oldRequest != null)
                {
                    oldRequest.Attaches.MoveBindingSourceTo(_request.Attaches);
                    oldRequest.Tags.MoveBindingSourceTo(_request.Tags);
                }

                FirePropertyChanged("Request");
            }
        }

        public RequestEntity RequestOrigin
        {
            get { return _requestOrigin; }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
        }

        public bool IsProcess
        {
            get { return _isProcess; }
        }

        public bool IsCommit
        {
            get { return _isCommit; }
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

        public IList<KeyValuePair<Enum, string>> RequestStates
        {
            get { return _mainController.RequestStates; }
        }

        public IList<KeyValuePair<Enum, string>> InfoSourceTypes
        {
            get { return _nsiService.InfoSourceTypes; }
        }

        string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            protected set
            {
                SetPropertyValue<string>("StatusMessage", ref _statusMessage, ref value);
            }
        }

        bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            private set { _dialogResult = value; }
        }

        public void OnApplyRequest()
        {
            _logger.Debug("OnApplyRequest");

            try
            {
                if (Validate())
                    if (!_mainController.ProcessRequestViewModel(this))
                        SetStatusMessage(LogLevel.Error, Properties.Resources.RequestApplyError);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час збереження зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }

        public void OnRefreshRequest()
        {
            _logger.Debug("OnRefreshRequest");

            if (Request.IsNewEntity) return;

            try
            {
                if (!Request.FullEquals(RequestOrigin))
                    if (_messageBoxMgr.ShowDialog("Внесені зміни будуть загублені. Бажаєте продовжити?") != true) return;

                RequestEntity request = _mainController.GetRequestById(Request.Id);
                if (request == null)
                    SetStatusMessage(LogLevel.Error, Properties.Resources.RequestNotFound);
                else
                {
                    Request = request;
                    SetStatusMessage(LogLevel.Info, Properties.Resources.RequestRefreshed);
                }            
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час оновлення зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }

        public void OnAddAttaches(bool moveToArchive)
        {
            _logger.Debug("OnAddAttaches");

            try
            {
                using (OpenFileDialog openDialog = new OpenFileDialog())
                {
                    openDialog.Multiselect = true;
                    openDialog.RestoreDirectory = true;
                    System.Windows.Forms.DialogResult result = openDialog.ShowDialog();
                    if (result != System.Windows.Forms.DialogResult.OK || openDialog.FileNames == null || openDialog.FileNames.Length == 0) return;

                    List<AttachEntity> attaches = new List<AttachEntity>();
                    List<string> fileNameList = new List<string>();
                    foreach (string fileName in openDialog.FileNames)
                    {
                        byte[] bytes = FileHlp.ReadFile(fileName);

                        AttachEntity attach = new AttachEntity();
                        attach.DateCreate = DateTime.Now;
                        attach.Name = Path.GetFileName(fileName);
                        attach.Blob = bytes;
                        attaches.Add(attach);

                        fileNameList.Add(fileName);
                    }
                    foreach (AttachEntity attach in attaches)
                    {
                        Request.Attaches.Entities.Add(attach);
                    }

                    if (moveToArchive)
                    {
                        AttachEntity attach = new AttachEntity();
                        attach.DateCreate = DateTime.Now;
                        attach.Name = "Archive_req_" + Request.Id + ".zip";
                        attach.Blob = FileHlp.AddToArchive(fileNameList);
                        Request.Attaches.Entities.Add(attach);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час додавання додатків до зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }  
        }

        public void OnSaveAttaches(bool saveAllFiles)
        {
            _logger.Debug("OnSaveAttaches");

            try
            {
                if (Request.Attaches.Entities.Count == 0) return;                

                if (saveAllFiles)
                {
                    using (FolderBrowserDialog directoryDialog = new FolderBrowserDialog())
                    {
                        if (directoryDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

                        string dirPath = directoryDialog.SelectedPath;
                        foreach (AttachEntity attach in Request.Attaches.Entities)
                        {
                            if (File.Exists(Path.Combine(dirPath, attach.Name)))
                                SaveAttachWithDialog(dirPath, attach);
                            else File.WriteAllBytes(dirPath, attach.Blob);
                        }
                    }
                }
                else
                {
                    if (Request.Attaches.Current != null)
                        SaveAttachWithDialog(Path.GetTempPath(), Request.Attaches.Current);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час збереження додатків зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }
        

        public void OnRemoveAttaches()
        {
            _logger.Debug("OnSaveAttaches");

            try
            {
                if (Request.Attaches.Current == null) return;
                Request.Attaches.Entities.Remove(Request.Attaches.Current);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час видалення додатків зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }

        public void OnExecuteAttach()
        {
            _logger.Debug("OnExecuteAttaches");

            try
            {
                if (Request.Attaches.Current == null) return;
                string path = System.IO.Path.GetTempPath() + Request.Attaches.Current.Name;
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + DateTime.Now.ToString("_dd_MM_hh_mm_ss") + Path.GetExtension(path);
                }
                using (System.IO.Stream s = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    s.Write(Request.Attaches.Current.Blob, 0, Request.Attaches.Current.Blob.Length);                    
                }               

                if (File.Exists(path))
                {
                    System.Diagnostics.Process.Start(path);
                }
                else
                    throw new ArgumentException("Can not find file: " + path);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час відкриття додатку зверенння.", Hlp.GetExceptionText(ex), "Помилка", null);
            }
        }

        public bool OnAddComment(string newComment)
        {
            _logger.Debug("OnAddComment");

            try
            {
                string login = _commonService.Login;
                Request.Comments = string.Format("{0}[{1}   {2}]{3}{4}{5}{6}", Request.Comments, DateTime.Now.ToString("dd.MM.yyyy    HH:mm:ss"), login, Environment.NewLine, newComment, Environment.NewLine, Environment.NewLine);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _messageBoxMgr.ShowMessageWithDetail(LogLevel.Error, "Помилка під час додавання нового коменту.", Hlp.GetExceptionText(ex), "Помилка", null);
                return false;
            }
        }

        #endregion Specific        


        #region IDispose
        
        public void Dispose()
        {
            Request.Attaches.SetBindingSource(null);
            Request.Tags.SetBindingSource(null);

            using (_control) { }

            _eventMgr.GetEvent<RequestChangedEvent>().Unsubscribe(OnRequestChanged);
        }

        #endregion IDispose


        #region private

        ILogMgr _logMgr;
        ILogger _logger;
        ICommonService _commonService;
        INsiService _nsiService;
        IViewFormMgr _viewFormMgr;
        IMessageBoxMgr _messageBoxMgr;
        IMainController _mainController;
        IEventMgr _eventMgr;

        RequestEntity _requestOrigin;
        RequestEntity _request;

        bool _readOnly;
        bool _isProcess;
        bool _isCommit;               

        BindingCollection<AppEntity> _appList;
        BindingCollection<OrgEntity> _orgList;
        BindingCollection<UserEntity> _userList;
        BindingCollection<TagEntity> _tagList;

        private void InitView()
        {
            _image = Properties.Resources.Request;
            _appList = _nsiService.Applications;
            _appList.Add(AppEntity.Create());

            _orgList = _nsiService.Organizations;
            _orgList.Add(OrgEntity.Create());

            _userList = _nsiService.Users;
            _userList.Add(UserEntity.Create());

            _tagList = _nsiService.Tags;
            _tagList.Add(TagEntity.Create());

            _dialogResult = null;
            _control = new RequestEditControl(this);            

            SetCaption();

            _eventMgr.GetEvent<RequestChangedEvent>().Subscribe(OnRequestChanged);
        }

        private void SetCaption()
        {
            if (_request.IsNewEntity)
            {
                _caption = "Створення звернення";
                _hint = "Створення нового зверненя";
            }
            else
            {
                _caption = string.Format("Звернення №{0}", _request.Id);
                _hint = string.Format("Редагування зверненя №{0}", _request.Id);
            }
        }

        private bool Validate()
        {
            bool result = _request.IsValid;
            if (result == false)
                SetStatusMessage(LogLevel.Warn, Properties.Resources.RequestNotValid);

            return result;
        }

        string GenerateName()
        {
            return string.Format("RequestViewModel{0}", DateTime.Now.ToString("ddMMyyHHmmssss"));
        }

        void OnRequestChanged(RequestChangedMessage message)
        {
            // Нажатие "ОК" или "Применить" при создании обращения
            if (message.ChangeType == RequestChangeType.Create &&
                Request.IsMyClone(message.Request))
            {
                Request = message.Request;
                SetCaption();
                SetStatusMessage(LogLevel.Info, Properties.Resources.RequestCreateSuccess);
            }

            else if (message.Request.Equals(Request))
            {                
                if (message.ChangeType == RequestChangeType.Update)
                {
                    // Нажатие "ОК" или "Применить" при редактировании обращения
                    if (Request.IsMyClone(message.Request))
                    {
                        Request = message.Request;
                        SetStatusMessage(LogLevel.Info, Properties.Resources.RequestSaveSuccess);
                    }
                    // Обращение было изменено в другой форме
                    else
                    {
                        SetStatusMessage(LogLevel.Warn, StatusMessage = Properties.Resources.RequestAlreadyChanged);
                    }
                }
                // Обращение было удалено
                else if (message.ChangeType == RequestChangeType.Delete)
                {
                    SetStatusMessage(LogLevel.Error, Properties.Resources.RequestAlreadyRemoved);
                }
            }            
        }

        void SetStatusMessage(LogLevel level, string text)
        {
            StatusMessage = level.ToString() + "###" + text;
        }

        private void SaveAttachWithDialog(string dirPath, AttachEntity attach)
        {
            string fullPath = Path.Combine(dirPath, attach.Name);
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.FileName = fullPath;
                saveDialog.InitialDirectory = dirPath;
                saveDialog.OverwritePrompt = true;
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (System.IO.Stream s = saveDialog.OpenFile())
                    {
                        s.Write(attach.Blob, 0, attach.Blob.Length);
                    }                    
                }
            }            
        }

        #endregion private       
    }
}
