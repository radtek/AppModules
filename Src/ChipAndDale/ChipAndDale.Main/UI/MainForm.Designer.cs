namespace ChipAndDale.Main.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
             

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainBarManager = new DevExpress.XtraBars.BarManager();
            this.MainToolBar = new DevExpress.XtraBars.Bar();
            this.ConnectBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DisconnectBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.MainMenu = new DevExpress.XtraBars.Bar();
            this.MainBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.NsiBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.UserNsiBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.OrgNsiBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.AppNsiBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.TagNsiBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.PlugginBarButtonItem = new DevExpress.XtraBars.BarSubItem();
            this.ViewBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.LogPanelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.AutoScrollLogPanelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.ClearLogPanelBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.SettingBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.SkinBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.SaveSettingBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.LoadSettingBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.AboutBarSubItem = new DevExpress.XtraBars.BarSubItem();
            this.AboutBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.MainStatusBar = new DevExpress.XtraBars.Bar();
            this.MainTaskBar = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.MainDockManager = new DevExpress.XtraBars.Docking.DockManager();
            this.LogDockPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.LogRichEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.MainFormImageCollection = new DevExpress.Utils.ImageCollection();
            this.MainDocumentManager = new DevExpress.XtraBars.Docking2010.DocumentManager();
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView();
            this.DefaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            ((System.ComponentModel.ISupportInitialize)(this.MainBarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainDockManager)).BeginInit();
            this.LogDockPanel.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainFormImageCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainDocumentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainBarManager
            // 
            this.MainBarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.MainToolBar,
            this.MainMenu,
            this.MainStatusBar,
            this.MainTaskBar});
            this.MainBarManager.DockControls.Add(this.barDockControlTop);
            this.MainBarManager.DockControls.Add(this.barDockControlBottom);
            this.MainBarManager.DockControls.Add(this.barDockControlLeft);
            this.MainBarManager.DockControls.Add(this.barDockControlRight);
            this.MainBarManager.DockManager = this.MainDockManager;
            this.MainBarManager.Form = this;
            this.MainBarManager.Images = this.MainFormImageCollection;
            this.MainBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.AboutBarButtonItem,
            this.ConnectBarButtonItem,
            this.NsiBarSubItem,
            this.UserNsiBarButton,
            this.AppNsiBarButton,
            this.TagNsiBarButton,
            this.OrgNsiBarButton,
            this.MainBarSubItem,
            this.PlugginBarButtonItem,
            this.DisconnectBarButtonItem,
            this.ViewBarSubItem,
            this.LogPanelBarButton,
            this.AutoScrollLogPanelBarButton,
            this.ClearLogPanelBarButton,
            this.SkinBarSubItem,
            this.SettingBarSubItem,
            this.SaveSettingBarButtonItem,
            this.LoadSettingBarButtonItem,
            this.AboutBarSubItem,
            this.barButtonItem1});
            this.MainBarManager.MainMenu = this.MainMenu;
            this.MainBarManager.MaxItemId = 23;
            this.MainBarManager.StatusBar = this.MainStatusBar;
            // 
            // MainToolBar
            // 
            this.MainToolBar.BarName = "Tools";
            this.MainToolBar.DockCol = 0;
            this.MainToolBar.DockRow = 1;
            this.MainToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.MainToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ConnectBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.DisconnectBarButtonItem)});
            this.MainToolBar.Text = "Tools";
            // 
            // ConnectBarButtonItem
            // 
            this.ConnectBarButtonItem.Caption = "Підключитися";
            this.ConnectBarButtonItem.Id = 2;
            this.ConnectBarButtonItem.ImageIndex = 0;
            this.ConnectBarButtonItem.Name = "ConnectBarButtonItem";
            this.ConnectBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ConnectBarButtonItem_ItemClick);
            // 
            // DisconnectBarButtonItem
            // 
            this.DisconnectBarButtonItem.Caption = "Відключитися";
            this.DisconnectBarButtonItem.Id = 12;
            this.DisconnectBarButtonItem.ImageIndex = 1;
            this.DisconnectBarButtonItem.Name = "DisconnectBarButtonItem";
            this.DisconnectBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DisconnectBarButtonItem_ItemClick);
            // 
            // MainMenu
            // 
            this.MainMenu.BarAppearance.Disabled.Font = new System.Drawing.Font("Verdana", 9F);
            this.MainMenu.BarAppearance.Disabled.Options.UseFont = true;
            this.MainMenu.BarAppearance.Hovered.Font = new System.Drawing.Font("Verdana", 9F);
            this.MainMenu.BarAppearance.Hovered.Options.UseFont = true;
            this.MainMenu.BarAppearance.Normal.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenu.BarAppearance.Normal.Options.UseFont = true;
            this.MainMenu.BarAppearance.Pressed.Font = new System.Drawing.Font("Verdana", 9F);
            this.MainMenu.BarAppearance.Pressed.Options.UseFont = true;
            this.MainMenu.BarName = "Main menu";
            this.MainMenu.DockCol = 0;
            this.MainMenu.DockRow = 0;
            this.MainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.MainMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MainBarSubItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.NsiBarSubItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.PlugginBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.ViewBarSubItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.SettingBarSubItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.AboutBarSubItem)});
            this.MainMenu.OptionsBar.MultiLine = true;
            this.MainMenu.OptionsBar.UseWholeRow = true;
            this.MainMenu.Text = "Main menu";
            // 
            // MainBarSubItem
            // 
            this.MainBarSubItem.Caption = "Головне";
            this.MainBarSubItem.Id = 10;
            this.MainBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.ConnectBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.DisconnectBarButtonItem)});
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Font = new System.Drawing.Font("Verdana", 9F);
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Options.UseFont = true;
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Font = new System.Drawing.Font("Verdana", 9F);
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Options.UseFont = true;
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Normal.Options.UseFont = true;
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Font = new System.Drawing.Font("Verdana", 9F);
            this.MainBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Options.UseFont = true;
            this.MainBarSubItem.Name = "MainBarSubItem";
            // 
            // NsiBarSubItem
            // 
            this.NsiBarSubItem.Caption = "Довідники";
            this.NsiBarSubItem.Id = 4;
            this.NsiBarSubItem.ItemAppearance.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.NsiBarSubItem.ItemAppearance.Normal.Options.UseFont = true;
            this.NsiBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.UserNsiBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.OrgNsiBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.AppNsiBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.TagNsiBarButton)});
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Font = new System.Drawing.Font("Verdana", 9F);
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Options.UseFont = true;
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Font = new System.Drawing.Font("Verdana", 9F);
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Options.UseFont = true;
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Normal.Options.UseFont = true;
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Font = new System.Drawing.Font("Verdana", 9F);
            this.NsiBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Options.UseFont = true;
            this.NsiBarSubItem.Name = "NsiBarSubItem";
            // 
            // UserNsiBarButton
            // 
            this.UserNsiBarButton.Caption = "Користувачі";
            this.UserNsiBarButton.Id = 5;
            this.UserNsiBarButton.Name = "UserNsiBarButton";
            this.UserNsiBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.UserNsiBarButton_ItemClick);
            // 
            // OrgNsiBarButton
            // 
            this.OrgNsiBarButton.Caption = "Організації";
            this.OrgNsiBarButton.Id = 8;
            this.OrgNsiBarButton.Name = "OrgNsiBarButton";
            this.OrgNsiBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.OrgNsiBarButton_ItemClick);
            // 
            // AppNsiBarButton
            // 
            this.AppNsiBarButton.Caption = "Додатки";
            this.AppNsiBarButton.Id = 6;
            this.AppNsiBarButton.Name = "AppNsiBarButton";
            this.AppNsiBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AppNsiBarButton_ItemClick);
            // 
            // TagNsiBarButton
            // 
            this.TagNsiBarButton.Caption = "Теги";
            this.TagNsiBarButton.Id = 7;
            this.TagNsiBarButton.Name = "TagNsiBarButton";
            this.TagNsiBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.TagNsiBarButton_ItemClick);
            // 
            // PlugginBarButtonItem
            // 
            this.PlugginBarButtonItem.Caption = "Плагіни";
            this.PlugginBarButtonItem.Id = 11;
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Disabled.Font = new System.Drawing.Font("Verdana", 9F);
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Disabled.Options.UseFont = true;
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Hovered.Font = new System.Drawing.Font("Verdana", 9F);
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Hovered.Options.UseFont = true;
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Normal.Options.UseFont = true;
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Pressed.Font = new System.Drawing.Font("Verdana", 9F);
            this.PlugginBarButtonItem.MenuAppearance.AppearanceMenu.Pressed.Options.UseFont = true;
            this.PlugginBarButtonItem.Name = "PlugginBarButtonItem";
            // 
            // ViewBarSubItem
            // 
            this.ViewBarSubItem.Caption = "Вигляд";
            this.ViewBarSubItem.Id = 13;
            this.ViewBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.LogPanelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.AutoScrollLogPanelBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.ClearLogPanelBarButton)});
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Font = new System.Drawing.Font("Verdana", 9F);
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Options.UseFont = true;
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Font = new System.Drawing.Font("Verdana", 9F);
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Options.UseFont = true;
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Normal.Options.UseFont = true;
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Font = new System.Drawing.Font("Verdana", 9F);
            this.ViewBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Options.UseFont = true;
            this.ViewBarSubItem.Name = "ViewBarSubItem";
            // 
            // LogPanelBarButton
            // 
            this.LogPanelBarButton.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.LogPanelBarButton.Caption = "Панель логів";
            this.LogPanelBarButton.Id = 14;
            this.LogPanelBarButton.Name = "LogPanelBarButton";
            this.LogPanelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.LogPanelBarButton_ItemClick);
            // 
            // AutoScrollLogPanelBarButton
            // 
            this.AutoScrollLogPanelBarButton.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.AutoScrollLogPanelBarButton.Caption = "Авто-скрол в панелі логів";
            this.AutoScrollLogPanelBarButton.Id = 15;
            this.AutoScrollLogPanelBarButton.Name = "AutoScrollLogPanelBarButton";
            this.AutoScrollLogPanelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AutoScrollLogPanelBarButton_ItemClick);
            // 
            // ClearLogPanelBarButton
            // 
            this.ClearLogPanelBarButton.Caption = "Очистити панель логів";
            this.ClearLogPanelBarButton.Id = 16;
            this.ClearLogPanelBarButton.Name = "ClearLogPanelBarButton";
            this.ClearLogPanelBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ClearLogPanelBarButton_ItemClick);
            // 
            // SettingBarSubItem
            // 
            this.SettingBarSubItem.Caption = "Настроювання";
            this.SettingBarSubItem.Id = 18;
            this.SettingBarSubItem.ItemAppearance.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.SettingBarSubItem.ItemAppearance.Normal.Options.UseFont = true;
            this.SettingBarSubItem.ItemInMenuAppearance.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.SettingBarSubItem.ItemInMenuAppearance.Normal.Options.UseFont = true;
            this.SettingBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.SkinBarSubItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.SaveSettingBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.LoadSettingBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Font = new System.Drawing.Font("Verdana", 9F);
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Disabled.Options.UseFont = true;
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Font = new System.Drawing.Font("Verdana", 9F);
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Options.UseFont = true;
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Normal.Options.UseFont = true;
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Font = new System.Drawing.Font("Verdana", 9F);
            this.SettingBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Options.UseFont = true;
            this.SettingBarSubItem.Name = "SettingBarSubItem";
            // 
            // SkinBarSubItem
            // 
            this.SkinBarSubItem.Caption = "Обкладини";
            this.SkinBarSubItem.Id = 17;
            this.SkinBarSubItem.Name = "SkinBarSubItem";
            // 
            // SaveSettingBarButtonItem
            // 
            this.SaveSettingBarButtonItem.Caption = "Зберегти робочий стол";
            this.SaveSettingBarButtonItem.Id = 19;
            this.SaveSettingBarButtonItem.Name = "SaveSettingBarButtonItem";
            this.SaveSettingBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveSettingBarButtonItem_ItemClick);
            // 
            // LoadSettingBarButtonItem
            // 
            this.LoadSettingBarButtonItem.Caption = "Завантажити робочий стол";
            this.LoadSettingBarButtonItem.Id = 20;
            this.LoadSettingBarButtonItem.Name = "LoadSettingBarButtonItem";
            this.LoadSettingBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.LoadSettingBarButtonItem_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Настроювання за-замовченням";
            this.barButtonItem1.Id = 22;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // AboutBarSubItem
            // 
            this.AboutBarSubItem.Caption = "О програмі";
            this.AboutBarSubItem.Id = 21;
            this.AboutBarSubItem.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.AboutBarButtonItem)});
            this.AboutBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Font = new System.Drawing.Font("Verdana", 9F);
            this.AboutBarSubItem.MenuAppearance.AppearanceMenu.Hovered.Options.UseFont = true;
            this.AboutBarSubItem.MenuAppearance.AppearanceMenu.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.AboutBarSubItem.MenuAppearance.AppearanceMenu.Normal.Options.UseFont = true;
            this.AboutBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Font = new System.Drawing.Font("Verdana", 9F);
            this.AboutBarSubItem.MenuAppearance.AppearanceMenu.Pressed.Options.UseFont = true;
            this.AboutBarSubItem.Name = "AboutBarSubItem";
            // 
            // AboutBarButtonItem
            // 
            this.AboutBarButtonItem.Caption = "О програмі";
            this.AboutBarButtonItem.Id = 1;
            this.AboutBarButtonItem.Name = "AboutBarButtonItem";
            // 
            // MainStatusBar
            // 
            this.MainStatusBar.BarName = "Status bar";
            this.MainStatusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.MainStatusBar.DockCol = 0;
            this.MainStatusBar.DockRow = 0;
            this.MainStatusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.MainStatusBar.OptionsBar.AllowQuickCustomization = false;
            this.MainStatusBar.OptionsBar.DrawDragBorder = false;
            this.MainStatusBar.OptionsBar.UseWholeRow = true;
            this.MainStatusBar.Text = "Status bar";
            // 
            // MainTaskBar
            // 
            this.MainTaskBar.BarName = "TaskBar";
            this.MainTaskBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.MainTaskBar.DockCol = 0;
            this.MainTaskBar.DockRow = 1;
            this.MainTaskBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.MainTaskBar.FloatLocation = new System.Drawing.Point(314, 694);
            this.MainTaskBar.OptionsBar.AllowQuickCustomization = false;
            this.MainTaskBar.OptionsBar.DrawDragBorder = false;
            this.MainTaskBar.OptionsBar.UseWholeRow = true;
            this.MainTaskBar.Text = "TaskBar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1100, 55);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 654);
            this.barDockControlBottom.Size = new System.Drawing.Size(1100, 46);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 55);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 599);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1100, 55);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 599);
            // 
            // MainDockManager
            // 
            this.MainDockManager.Form = this;
            this.MainDockManager.HiddenPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.LogDockPanel});
            this.MainDockManager.MenuManager = this.MainBarManager;
            this.MainDockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // LogDockPanel
            // 
            this.LogDockPanel.Controls.Add(this.dockPanel1_Container);
            this.LogDockPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.LogDockPanel.ID = new System.Guid("9e9db4b3-a0d3-4533-bbd9-c846b95c7d66");
            this.LogDockPanel.Location = new System.Drawing.Point(0, 423);
            this.LogDockPanel.Name = "LogDockPanel";
            this.LogDockPanel.Options.AllowDockAsTabbedDocument = false;
            this.LogDockPanel.OriginalSize = new System.Drawing.Size(200, 231);
            this.LogDockPanel.Size = new System.Drawing.Size(1100, 231);
            this.LogDockPanel.Text = "Панель логів";
            this.LogDockPanel.ClosedPanel += new DevExpress.XtraBars.Docking.DockPanelEventHandler(this.LogDockPanel_ClosedPanel);
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.LogRichEditControl);
            this.dockPanel1_Container.Location = new System.Drawing.Point(6, 26);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1088, 199);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // LogRichEditControl
            // 
            this.LogRichEditControl.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.LogRichEditControl.Appearance.Text.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LogRichEditControl.Appearance.Text.ForeColor = System.Drawing.Color.White;
            this.LogRichEditControl.Appearance.Text.Options.UseFont = true;
            this.LogRichEditControl.Appearance.Text.Options.UseForeColor = true;
            this.LogRichEditControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.LogRichEditControl.CausesValidation = false;
            this.LogRichEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogRichEditControl.Location = new System.Drawing.Point(0, 0);
            this.LogRichEditControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.LogRichEditControl.MenuManager = this.MainBarManager;
            this.LogRichEditControl.Name = "LogRichEditControl";
            this.LogRichEditControl.Options.Behavior.CreateNew = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.Behavior.Drag = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.Behavior.OfficeScrolling = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
            this.LogRichEditControl.Options.DocumentCapabilities.CharacterStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.FloatingObjects = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.HeadersFooters = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.Hyperlinks = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.InlinePictures = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.Numbering.Simple = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
            this.LogRichEditControl.Options.DocumentCapabilities.ParagraphFormatting = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.Paragraphs = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.ParagraphStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.ParagraphTabs = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.Sections = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.Tables = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.TableStyle = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.TabSymbol = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentCapabilities.Undo = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.LogRichEditControl.Options.DocumentSaveOptions.CurrentFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.LogRichEditControl.Options.DocumentSaveOptions.DefaultFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.LogRichEditControl.Options.HorizontalRuler.ShowLeftIndent = false;
            this.LogRichEditControl.Options.HorizontalRuler.ShowRightIndent = false;
            this.LogRichEditControl.Options.HorizontalRuler.ShowTabs = false;
            this.LogRichEditControl.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
            this.LogRichEditControl.Options.HorizontalScrollbar.Visibility = DevExpress.XtraRichEdit.RichEditScrollbarVisibility.Hidden;
            this.LogRichEditControl.Options.RangePermissions.BracketsColor = System.Drawing.Color.Lime;
            this.LogRichEditControl.Options.VerticalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
            this.LogRichEditControl.ReadOnly = true;
            this.LogRichEditControl.Size = new System.Drawing.Size(1088, 199);
            this.LogRichEditControl.TabIndex = 0;
            this.LogRichEditControl.Text = "Початок роботи програми ...";
            this.LogRichEditControl.Views.SimpleView.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.LogRichEditControl.Views.SimpleView.Padding = new System.Windows.Forms.Padding(5, 4, 4, 0);
            // 
            // MainFormImageCollection
            // 
            this.MainFormImageCollection.ImageSize = new System.Drawing.Size(20, 20);
            this.MainFormImageCollection.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("MainFormImageCollection.ImageStream")));
            this.MainFormImageCollection.Images.SetKeyName(0, "ConnectToDb.png");
            this.MainFormImageCollection.Images.SetKeyName(1, "DisConnectFromDb.png");
            // 
            // MainDocumentManager
            // 
            this.MainDocumentManager.MdiParent = this;
            this.MainDocumentManager.MenuManager = this.MainBarManager;
            this.MainDocumentManager.View = this.tabbedView1;
            this.MainDocumentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // tabbedView1
            // 
            this.tabbedView1.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tabbedView1.AppearancePage.Header.Options.UseFont = true;
            this.tabbedView1.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.tabbedView1.AppearancePage.HeaderActive.Options.UseFont = true;
            this.tabbedView1.AppearancePage.HeaderHotTracked.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline);
            this.tabbedView1.AppearancePage.HeaderHotTracked.Options.UseFont = true;
            this.tabbedView1.AppearancePage.HeaderSelected.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tabbedView1.AppearancePage.HeaderSelected.Options.UseFont = true;
            // 
            // DefaultLookAndFeel
            // 
            this.DefaultLookAndFeel.LookAndFeel.SkinName = "Sharp";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "Чип и Дейл";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            ((System.ComponentModel.ISupportInitialize)(this.MainBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainDockManager)).EndInit();
            this.LogDockPanel.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainFormImageCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainDocumentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        internal DevExpress.XtraBars.BarManager MainBarManager;
        internal DevExpress.XtraBars.Docking.DockManager MainDockManager;
        internal DevExpress.XtraBars.Docking2010.DocumentManager MainDocumentManager;
        internal DevExpress.LookAndFeel.DefaultLookAndFeel DefaultLookAndFeel;
        private DevExpress.XtraBars.BarButtonItem AboutBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem ConnectBarButtonItem;
        internal DevExpress.XtraBars.Bar MainToolBar;
        internal DevExpress.XtraBars.Bar MainMenu;
        internal DevExpress.XtraBars.Bar MainStatusBar;
        internal DevExpress.XtraBars.Bar MainTaskBar;
        private DevExpress.XtraBars.BarSubItem NsiBarSubItem;
        private DevExpress.XtraBars.BarButtonItem UserNsiBarButton;
        private DevExpress.XtraBars.BarButtonItem AppNsiBarButton;
        private DevExpress.XtraBars.BarButtonItem TagNsiBarButton;
        private DevExpress.XtraBars.BarButtonItem OrgNsiBarButton;
        private DevExpress.XtraBars.BarSubItem MainBarSubItem;
        internal DevExpress.XtraBars.BarSubItem PlugginBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DisconnectBarButtonItem;
        private DevExpress.XtraBars.Docking.DockPanel LogDockPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.BarSubItem ViewBarSubItem;
        private DevExpress.XtraBars.BarButtonItem LogPanelBarButton;
        private DevExpress.XtraRichEdit.RichEditControl LogRichEditControl;
        private DevExpress.XtraBars.BarButtonItem AutoScrollLogPanelBarButton;
        private DevExpress.XtraBars.BarButtonItem ClearLogPanelBarButton;
        private DevExpress.XtraBars.BarSubItem SkinBarSubItem;
        private DevExpress.XtraBars.BarSubItem SettingBarSubItem;
        private DevExpress.XtraBars.BarButtonItem SaveSettingBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem LoadSettingBarButtonItem;
        private DevExpress.XtraBars.BarSubItem AboutBarSubItem;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.Utils.ImageCollection MainFormImageCollection;


    }
}
