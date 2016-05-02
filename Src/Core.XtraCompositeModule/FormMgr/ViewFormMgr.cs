using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Core.SDK.Common;
using Core.SDK.Composite.Common;
using Core.SDK.Composite.UI;
using Core.SDK.Log;
using Core.UtilsModule;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace Core.XtraCompositeModule.DialogMgr
{
    public class ViewFormMgr : IViewFormMgr
    {
        public ViewFormMgr(XtraForm parentForm, Bar statusBar, ILogMgr logMgr)
        {
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("ViewFormMgr");
            _logger.Info("Create.");

            if (parentForm == null) throw new ArgumentNullException("ParentForm can not be null.");
            if (statusBar == null) throw new ArgumentNullException("StatusBar can not be null.");
            if (logMgr == null) throw new ArgumentNullException("LogMgr can not be null.");

            _statusBar = statusBar;
            _statusBarButtonController = new StatusBarButtonController(statusBar);
            _statusBarButtonController.ButtonClickEvent += new ButtonClickDelegate(OnStatusBarButtonClick);
            _parentForm = parentForm;
            _views = new Dictionary<IdentKey, ViewData>();            
        }

        #region IViewFormMgr
                
        public DialogResult ShowModal(IView view)
        {
            return ShowModal(view, DialogFormOptionsHelper.Default, null);
        }

        public DialogResult ShowModal(IView view, DialogSetting settings)
        {
            return ShowModal(view, DialogFormOptionsHelper.Default, settings);
        }

        public DialogResult ShowModal(IView view, DialogFormOption options)
        {
            return ShowModal(view, options, null);
        }

        public DialogResult ShowModal(IView view, DialogFormOption options, DialogSetting settings)
        {
            _logger.Debug("ShowModal.");
            IdentKey key = null;
            ViewData viewData = null;

            key = CreateViewForm(view, options, settings);
            viewData = GetViewByKey(key);
            viewData.Form.ShowInTaskbar = false; // Hack: ShowDialog и ShowInTaskbar = true не работают вместе
            viewData.IsModal = true;            
            viewData.Form.ShowDialog(_parentForm);            
            return viewData.DialogResult;
        }



        public IdentKey ShowNoModal(IView view)
        {
            return ShowNoModal(view, DialogFormOptionsHelper.Default, null);
        }

        public IdentKey ShowNoModal(IView view, DialogSetting settings)
        {
            return ShowNoModal(view, DialogFormOptionsHelper.Default, settings);            
        }

        public IdentKey ShowNoModal(IView view, DialogFormOption options)
        {
            return ShowNoModal(view, options, null);
        }

        public IdentKey ShowNoModal(IView view, DialogFormOption options, DialogSetting settings)
        {
            _logger.Debug("ShowNoModal.");
            IdentKey key = CreateViewForm(view, options, settings);
            ViewData viewData = GetViewByKey(key);
            viewData.IsModal = false;            
            viewData.Form.Show(_parentForm);
            viewData.Form.Focus();
            return key;
        }



        public void CloseNoModal(IdentKey key)
        {
            _logger.Debug("CloseNoModal.");
            ViewData viewData = GetViewByKey(key);

            if (viewData.IsClose == true) throw new InvalidOperationException("View already close.");
            viewData.Form.Close();
        }

        public void ChangeStateNoModal(IdentKey key, bool isActivate)
        {
            _logger.Debug("ChangeStateNoModal.");
            if (IsActiveNoModal(key) == isActivate) return;

            ViewData viewData = GetViewByKey(key);
            ChangeFormState(viewData, isActivate);
            OnChangeState(key, isActivate);
        }

        public bool IsActiveNoModal(IdentKey key)
        {
            _logger.Debug("IsActiveNoModal.");
            ViewData viewData = GetViewByKey(key);
            return viewData.IsActive;
        }        

        public DialogSetting GetDialogSetting(IdentKey key)
        {
            _logger.Debug("GetDialogSetting.");
            ViewData viewData = GetViewByKey(key);
            return viewData.Form.DialogSetting;
        }
        #endregion IViewFormMgr


        #region private

        class ViewData
        {
            public ViewData(IView view)
            { 
                View = view;
                Form = null;                
                IsActive = false;
                IsModal = false;
                IsClose = false;                
                DialogResult = System.Windows.Forms.DialogResult.None;
                FormWindowState = FormWindowState.Normal;
            }            

            public IView View;
            public FormBase Form;            
            public bool IsActive;
            public bool IsModal;
            public bool IsClose;            
            public DialogResult DialogResult;
            public FormWindowState FormWindowState;
        }

        Bar _statusBar;
        StatusBarButtonController _statusBarButtonController;
        XtraForm _parentForm;
        ILogMgr _logMgr;
        ILogger _logger;
        Dictionary<IdentKey, ViewData> _views;


        #region Create and clear

        private IdentKey CreateViewForm(IView view, DialogFormOption options, DialogSetting settings)
        {
            _logger.Debug("CreateViewForm." + view.ToStringExt());
            CheckView(view);

            IdentKey key = new IdentKey();
            ViewData viewData = new ViewData(view);
            FormBase form = new FormBase(options);            
            form.Owner = _parentForm; //TODO: разобраться на что может повлиять                        
            form.Tag = key;
            form.Text = view.Caption;
            
            form.Icon = Icon.FromHandle((view.Image as Bitmap).GetHicon());
            Control control = viewData.View.UserControl as Control;
            Size size = Size.Add(control.Size, new Size(0, GetFormHeaderHeigth(form)));
            form.Size = size;
            if (settings != null)
            {
                if (settings.Position != null) form.Location = settings.Position;
                if (settings.Size != null) form.Size = settings.Size;
            }
            else
            {
                form.Location = new Point(_parentForm.Location.X + (_parentForm.Width - form.Width) / 2, 
                    _parentForm.Location.Y + (_parentForm.Height - form.Height) / 2);
            }
            view.DialogSetting = form.DialogSetting;

            try
            {                
                form.Controls.Add(control);                
                control.Dock = DockStyle.Fill;
                control.Parent = form;

                IDialogResult dialogButtons = view.UserControl as IDialogResult;
                if (dialogButtons != null)
                {
                    IButtonControl okButton = dialogButtons.OkButton as IButtonControl;
                    if (okButton != null) form.AcceptButton = okButton;
                    IButtonControl cancelButton = dialogButtons.CancelButton as IButtonControl;                    
                    if (cancelButton != null) form.CancelButton = cancelButton;                    
                }
                _statusBarButtonController.AddButton(key, viewData.View.Caption, viewData.View.Hint, viewData.View.Image);
                
                viewData.Form = form;
                _views.Add(key, viewData);
                viewData.View.Ident = key;
                SubscibeToForm(form);              
            }
            catch (Exception ex)
            {
                ClearFormResourceSuppress(key, viewData);
                throw ex;
            }

            return key;
        }        

        private void ClearFormResourceSuppress(IdentKey key, ViewData viewData)
        {            
            InvokerHlp.WithExceptionSuppress(() => { UnsubscribeFromForm(viewData.Form); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => {
                                                    viewData.Form.AcceptButton = null;
                                                    viewData.Form.CancelButton = null;
                                                    viewData.View.Ident = null;
                                                }, _logger);    
            InvokerHlp.WithExceptionSuppress(() => { viewData.Form.Controls.Remove((Control)viewData.View.UserControl); }, _logger);            
            InvokerHlp.WithExceptionSuppress(() => { ((Control)viewData.View.UserControl).Parent = null; }, _logger);            
            InvokerHlp.WithExceptionSuppress(() => { _statusBarButtonController.RemoveButton(key); }, _logger);
            InvokerHlp.WithExceptionSuppress(() => { _views.Remove(key); }, _logger);                    
            InvokerHlp.WithExceptionSuppress(() => 
                { 
                    viewData.Form = null;
                    viewData.IsActive = false;
                    viewData.IsClose = true;
                    viewData.View = null;
                }, _logger);       
        }

        #endregion Create and clear

        #region Form events handler

        private void SubscibeToForm(FormBase form)
        {
            form.FormClosing += new FormClosingEventHandler(OnFormClosing);
            form.FormClosed += new FormClosedEventHandler(OnFormClosed);
            form.Activated += new EventHandler(OnActivated);
            form.Deactivate += new EventHandler(OnDeactivate);
            form.SizeChanged += new EventHandler(OnSizeChanged);
            form.LocationChanged += new EventHandler(OnLocationChanged);
        }                  

        private void UnsubscribeFromForm(FormBase form)
        {
            form.FormClosing -= new FormClosingEventHandler(OnFormClosing);
            form.FormClosed -= new FormClosedEventHandler(OnFormClosed);  
            form.Activated -= new EventHandler(OnActivated);
            form.Deactivate -= new EventHandler(OnDeactivate);
            form.SizeChanged -= new EventHandler(OnSizeChanged);
            form.LocationChanged -= new EventHandler(OnLocationChanged);
        }

        void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _logger.Debug("OnFormClosing");
            if (e.CloseReason != CloseReason.UserClosing && e.CloseReason != CloseReason.None) return;
            ViewData viewData = null;
            try
            {
                IdentKey key = GetKeyOfFormBase(sender);
                viewData = GetViewByKey(key);
                viewData.DialogResult = viewData.Form.DialogResult;
                if ((viewData.DialogResult != DialogResult.Cancel && viewData.DialogResult != DialogResult.None && !viewData.View.CanClose) || !viewData.View.OnClosing(ConvertDialogResultToNBool(viewData.DialogResult))) 
                {
                    viewData.Form.DialogResult = DialogResult.None;
                    e.Cancel = true;                    
                }                
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                viewData.Form.DialogResult = DialogResult.None;
                e.Cancel = true;
            }
        }

        void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _logger.Debug("OnFormClosed");
            if (e.CloseReason != CloseReason.UserClosing && e.CloseReason != CloseReason.None) return;
            IdentKey key = GetKeyOfFormBase(sender);
            ViewData viewData = GetViewByKey(key);
            InvokerHlp.WithExceptionSuppress(() => { viewData.View.OnAfterClose(); }, _logger);
            ClearFormResourceSuppress(key, viewData);
        }               

        void OnDeactivate(object sender, EventArgs e)
        {            
            IdentKey key = GetKeyOfFormBase(sender);
            OnChangeState(key, false);
        }        

        void OnActivated(object sender, EventArgs e)
        {            
            IdentKey key = GetKeyOfFormBase(sender);
            OnChangeState(key, true);
        }

        private void OnChangeState(IdentKey key, bool isActivate)
        {
            ViewData viewData = GetViewByKey(key);            
            viewData.IsActive = isActivate;            
            if (viewData.Form.WindowState != FormWindowState.Minimized) viewData.FormWindowState = viewData.Form.WindowState;            
            _statusBarButtonController.ChangeButtonState(key, isActivate);
            viewData.View.OnActivate(isActivate);            
        }               

        void OnSizeChanged(object sender, EventArgs e)
        {
            IdentKey key = GetKeyOfFormBase(sender);
            ViewData viewData = GetViewByKey(key);
            FormBase form = sender as FormBase;            
            if ((viewData.Form.DialogFormOption & DialogFormOption.ShowInOSTaskBar) == 0 && viewData.Form.WindowState == FormWindowState.Minimized && !viewData.IsModal) viewData.Form.Hide();
            viewData.View.DialogSetting = form.DialogSetting;
        }

        void OnLocationChanged(object sender, EventArgs e)
        {
            FormBase form = sender as FormBase;
            IdentKey key = form.Tag as IdentKey;           
            ViewData viewData = GetViewByKey(key);
            viewData.View.DialogSetting = form.DialogSetting;
        }  

        void OnStatusBarButtonClick(IdentKey key, bool isActivate)
        {            
            ViewData viewData = GetViewByKey(key);           
            viewData.IsActive = isActivate;
            ChangeFormState(viewData, isActivate);            
            viewData.View.OnActivate(isActivate);
        }

        private void ChangeFormState(ViewData viewData, bool isActivate)
        {
            if (isActivate)
            {                
                if ((viewData.Form.DialogFormOption & DialogFormOption.ShowInOSTaskBar) == 0 && !viewData.IsModal)
                    viewData.Form.Show();
                viewData.Form.WindowState = viewData.FormWindowState;
                viewData.Form.Focus();
            }
            else
            {
                if (viewData.Form.WindowState != FormWindowState.Minimized) viewData.FormWindowState = viewData.Form.WindowState;
                viewData.Form.WindowState = FormWindowState.Minimized;
                if ((viewData.Form.DialogFormOption & DialogFormOption.ShowInOSTaskBar) == 0) viewData.Form.Hide();
            }
        }

        #endregion Form events handler

        #region helper methods

        int GetFormHeaderHeigth(FormBase form)
        {
           Rectangle screenRectangle = form.RectangleToScreen(form.ClientRectangle);
           return (screenRectangle.Top - form.Top);
        }

        private ViewData GetViewByKey(IdentKey key)
        {
            if (key == null) throw new ArgumentNullException("Key can not be null.");
            ViewData viewData = null;
            if (!_views.TryGetValue(key, out viewData)) throw new InvalidOperationException("View with such key is not find.");            
            return viewData;
        }

        IdentKey GetKeyOfFormBase(object sender)
        {
            FormBase form = sender as FormBase;
            if (form == null) throw new InvalidOperationException("Sender is not a FormBase."); ;
            IdentKey key = form.Tag as IdentKey;
            return key;
        }

        private void CheckView(IView view)
        {
            if (view == null) throw new ArgumentNullException("View can not be null.");
            if (view.UserControl == null) throw new ArgumentNullException("View.UserControl can not be null.");
            if (!(view.UserControl is Control)) throw new ArgumentNullException("View.UserControl must be inherited from Control class.");
        }

        private bool? ConvertDialogResultToNBool(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.OK) return true;
            else if (dialogResult == DialogResult.Cancel) return false;
            else return null;
        }
        #endregion helper methods

        #endregion private
    }
}
