namespace ChipAndDale.Request.UI
{
    partial class RequestListControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestListControl));
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.colStateId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsPersist = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BarManager = new DevExpress.XtraBars.BarManager(this.components);
            this.MainToolBar = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.AddBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.EditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.CloneBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.RemoveBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.AuditBarButton = new DevExpress.XtraBars.BarButtonItem();
            this.FilterListBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.FiltersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RequestListViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AddFilterBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.EditFilterBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.DelFilterBarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.ReqListImageCollection = new DevExpress.Utils.ImageCollection(this.components);
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RequestGridControl = new DevExpress.XtraGrid.GridControl();
            this.RequestBndingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RequestGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreateDateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReqDateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSubject = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComments = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colContact = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrganization = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInfoSourceType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApplication = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTagsString = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTags = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAttaches = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBugNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCMVersion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComponentVersion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInfoSourceTypeId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsImportantString = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsNewEntity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatorUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colResponseUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsImportant = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FiltersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestListViewModelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReqListImageCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestBndingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // colStateId
            // 
            this.colStateId.FieldName = "StateId";
            this.colStateId.Name = "colStateId";
            this.colStateId.OptionsColumn.AllowShowHide = false;
            this.colStateId.OptionsColumn.ReadOnly = true;
            // 
            // colIsPersist
            // 
            this.colIsPersist.Caption = "colIsPersist";
            this.colIsPersist.FieldName = "IsPersisit";
            this.colIsPersist.Name = "colIsPersist";
            // 
            // colState
            // 
            this.colState.FieldName = "State";
            this.colState.Name = "colState";
            this.colState.Visible = true;
            this.colState.VisibleIndex = 6;
            this.colState.Width = 146;
            // 
            // BarManager
            // 
            this.BarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.MainToolBar});
            this.BarManager.DockControls.Add(this.barDockControlTop);
            this.BarManager.DockControls.Add(this.barDockControlBottom);
            this.BarManager.DockControls.Add(this.barDockControlLeft);
            this.BarManager.DockControls.Add(this.barDockControlRight);
            this.BarManager.Form = this;
            this.BarManager.Images = this.ReqListImageCollection;
            this.BarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.AddBarButton,
            this.EditBarButton,
            this.CloneBarButton,
            this.RemoveBarButton,
            this.AuditBarButton,
            this.barButtonItem1,
            this.FilterListBarEditItem,
            this.EditFilterBarButtonItem,
            this.AddFilterBarButtonItem,
            this.DelFilterBarButtonItem});
            this.BarManager.MaxItemId = 11;
            this.BarManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemTextEdit1});
            // 
            // MainToolBar
            // 
            this.MainToolBar.BarName = "Tools";
            this.MainToolBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.MainToolBar.DockCol = 0;
            this.MainToolBar.DockRow = 0;
            this.MainToolBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.MainToolBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.AddBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.CloneBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.RemoveBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.AuditBarButton),
            new DevExpress.XtraBars.LinkPersistInfo(this.FilterListBarEditItem, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.AddFilterBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.EditFilterBarButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.DelFilterBarButtonItem)});
            this.MainToolBar.OptionsBar.AllowQuickCustomization = false;
            this.MainToolBar.OptionsBar.RotateWhenVertical = false;
            this.MainToolBar.Text = "Tools";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Refresh";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.ImageIndex = 7;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RefreshButtonItem_ItemClick);
            // 
            // AddBarButton
            // 
            this.AddBarButton.Caption = "Add";
            this.AddBarButton.Id = 0;
            this.AddBarButton.ImageIndex = 1;
            this.AddBarButton.Name = "AddBarButton";
            this.AddBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AddBarButton_ItemClick);
            // 
            // EditBarButton
            // 
            this.EditBarButton.Caption = "Edit";
            this.EditBarButton.Id = 1;
            this.EditBarButton.ImageIndex = 4;
            this.EditBarButton.Name = "EditBarButton";
            this.EditBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.EditBarButton_ItemClick);
            // 
            // CloneBarButton
            // 
            this.CloneBarButton.Caption = "Clone";
            this.CloneBarButton.Id = 2;
            this.CloneBarButton.ImageIndex = 3;
            this.CloneBarButton.Name = "CloneBarButton";
            this.CloneBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.CloneBarButton_ItemClick);
            // 
            // RemoveBarButton
            // 
            this.RemoveBarButton.Caption = "Del";
            this.RemoveBarButton.Id = 3;
            this.RemoveBarButton.ImageIndex = 9;
            this.RemoveBarButton.Name = "RemoveBarButton";
            this.RemoveBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RemoveBarButton_ItemClick);
            // 
            // AuditBarButton
            // 
            this.AuditBarButton.Caption = "Audit";
            this.AuditBarButton.Id = 4;
            this.AuditBarButton.ImageIndex = 2;
            this.AuditBarButton.Name = "AuditBarButton";
            // 
            // FilterListBarEditItem
            // 
            this.FilterListBarEditItem.Caption = "FilterList";
            this.FilterListBarEditItem.Edit = this.repositoryItemLookUpEdit1;
            this.FilterListBarEditItem.Id = 6;
            this.FilterListBarEditItem.ItemAppearance.Normal.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FilterListBarEditItem.ItemAppearance.Normal.Options.UseFont = true;
            this.FilterListBarEditItem.Name = "FilterListBarEditItem";
            this.FilterListBarEditItem.Width = 175;
            this.FilterListBarEditItem.EditValueChanged += new System.EventHandler(this.FilterListBarEditItem_EditValueChanged);
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.repositoryItemLookUpEdit1.Appearance.Options.UseFont = true;
            this.repositoryItemLookUpEdit1.AppearanceDropDown.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.repositoryItemLookUpEdit1.AppearanceDropDown.Options.UseFont = true;
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FilterName", "Назва фільтру", 77, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.repositoryItemLookUpEdit1.DataSource = this.FiltersBindingSource;
            this.repositoryItemLookUpEdit1.DisplayMember = "FilterName";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.EditValueChanged += new System.EventHandler(this.FilterListRepositoryLookUpEdit_EditValueChanged);
            // 
            // FiltersBindingSource
            // 
            this.FiltersBindingSource.DataMember = "Filters";
            this.FiltersBindingSource.DataSource = this.RequestListViewModelBindingSource;
            // 
            // RequestListViewModelBindingSource
            // 
            this.RequestListViewModelBindingSource.DataSource = typeof(ChipAndDale.Request.ViewModel.RequestListViewModel);
            // 
            // AddFilterBarButtonItem
            // 
            this.AddFilterBarButtonItem.Caption = "AddFilter";
            this.AddFilterBarButtonItem.Id = 9;
            this.AddFilterBarButtonItem.ImageIndex = 0;
            this.AddFilterBarButtonItem.Name = "AddFilterBarButtonItem";
            this.AddFilterBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AddFilterButtonItem_ItemClick);
            // 
            // EditFilterBarButtonItem
            // 
            this.EditFilterBarButtonItem.Caption = "EditFilter";
            this.EditFilterBarButtonItem.Id = 7;
            this.EditFilterBarButtonItem.ImageIndex = 5;
            this.EditFilterBarButtonItem.Name = "EditFilterBarButtonItem";
            this.EditFilterBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.FilterEditBarButtonItem_ItemClick);
            // 
            // DelFilterBarButtonItem
            // 
            this.DelFilterBarButtonItem.Caption = "DelFilter";
            this.DelFilterBarButtonItem.Id = 10;
            this.DelFilterBarButtonItem.ImageIndex = 8;
            this.DelFilterBarButtonItem.Name = "DelFilterBarButtonItem";
            this.DelFilterBarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.DelFilterBarButtonItem_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1036, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 434);
            this.barDockControlBottom.Size = new System.Drawing.Size(1036, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 405);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1036, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 405);
            // 
            // ReqListImageCollection
            // 
            this.ReqListImageCollection.ImageSize = new System.Drawing.Size(20, 20);
            this.ReqListImageCollection.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("ReqListImageCollection.ImageStream")));
            this.ReqListImageCollection.Images.SetKeyName(0, "AddFilterRequest.png");
            this.ReqListImageCollection.Images.SetKeyName(1, "AddRequest.png");
            this.ReqListImageCollection.Images.SetKeyName(2, "AudirRequest.png");
            this.ReqListImageCollection.Images.SetKeyName(3, "CloneRequest.png");
            this.ReqListImageCollection.Images.SetKeyName(4, "EditRequest.png");
            this.ReqListImageCollection.Images.SetKeyName(5, "FilterRequest.png");
            this.ReqListImageCollection.Images.SetKeyName(6, "Recycle.png");
            this.ReqListImageCollection.Images.SetKeyName(7, "Refresh.png");
            this.ReqListImageCollection.Images.SetKeyName(8, "RemoveFilterRequest.png");
            this.ReqListImageCollection.Images.SetKeyName(9, "RemoveRequest.png");
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // RequestGridControl
            // 
            this.RequestGridControl.DataSource = this.RequestBndingSource;
            this.RequestGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RequestGridControl.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RequestGridControl.Location = new System.Drawing.Point(0, 29);
            this.RequestGridControl.MainView = this.RequestGridView;
            this.RequestGridControl.MenuManager = this.BarManager;
            this.RequestGridControl.Name = "RequestGridControl";
            this.RequestGridControl.Size = new System.Drawing.Size(1036, 405);
            this.RequestGridControl.TabIndex = 4;
            this.RequestGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.RequestGridView});
            // 
            // RequestBndingSource
            // 
            this.RequestBndingSource.DataSource = typeof(ChipAndDale.SDK.Request.RequestEntity);
            // 
            // RequestGridView
            // 
            this.RequestGridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RequestGridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.RequestGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.RequestGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.RequestGridView.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.RequestGridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.RequestGridView.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RequestGridView.Appearance.Row.Options.UseFont = true;
            this.RequestGridView.Appearance.Row.Options.UseTextOptions = true;
            this.RequestGridView.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.RequestGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colCreateDateTime,
            this.colReqDateTime,
            this.colSubject,
            this.colComments,
            this.colContact,
            this.colOrganization,
            this.colInfoSourceType,
            this.colApplication,
            this.colTagsString,
            this.colTags,
            this.colAttaches,
            this.colBugNumber,
            this.colCMVersion,
            this.colComponentVersion,
            this.colStateId,
            this.colInfoSourceTypeId,
            this.colIsImportantString,
            this.colNumId,
            this.colIsNewEntity,
            this.colCreatorUser,
            this.colResponseUser,
            this.colState,
            this.colIsImportant,
            this.colIsPersist});
            this.RequestGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            styleFormatCondition1.Appearance.Options.HighPriority = true;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Column = this.colStateId;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = 0;
            styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.LightYellow;
            styleFormatCondition2.Appearance.Options.HighPriority = true;
            styleFormatCondition2.Appearance.Options.UseBackColor = true;
            styleFormatCondition2.Column = this.colStateId;
            styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition2.Value1 = 1;
            styleFormatCondition3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic);
            styleFormatCondition3.Appearance.Options.HighPriority = true;
            styleFormatCondition3.Appearance.Options.UseFont = true;
            styleFormatCondition3.Column = this.colIsPersist;
            styleFormatCondition3.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition3.Value1 = false;
            this.RequestGridView.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2,
            styleFormatCondition3});
            this.RequestGridView.GridControl = this.RequestGridControl;
            this.RequestGridView.Name = "RequestGridView";
            this.RequestGridView.OptionsBehavior.Editable = false;
            this.RequestGridView.OptionsBehavior.ReadOnly = true;
            this.RequestGridView.OptionsFilter.ShowAllTableValuesInFilterPopup = true;
            this.RequestGridView.OptionsFilter.UseNewCustomFilterDialog = true;
            this.RequestGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.RequestGridView.OptionsView.ShowDetailButtons = false;
            this.RequestGridView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.RequestGridView.OptionsView.ShowGroupPanel = false;
            this.RequestGridView.DoubleClick += new System.EventHandler(this.RequestGridView_DoubleClick);
            // 
            // colId
            // 
            this.colId.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.colId.AppearanceCell.Options.UseFont = true;
            this.colId.Caption = "№";
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = true;
            this.colId.VisibleIndex = 0;
            this.colId.Width = 97;
            // 
            // colCreateDateTime
            // 
            this.colCreateDateTime.FieldName = "CreateDateTime";
            this.colCreateDateTime.Name = "colCreateDateTime";
            this.colCreateDateTime.Visible = true;
            this.colCreateDateTime.VisibleIndex = 1;
            this.colCreateDateTime.Width = 146;
            // 
            // colReqDateTime
            // 
            this.colReqDateTime.FieldName = "ReqDateTime";
            this.colReqDateTime.Name = "colReqDateTime";
            // 
            // colSubject
            // 
            this.colSubject.FieldName = "Subject";
            this.colSubject.Name = "colSubject";
            // 
            // colComments
            // 
            this.colComments.FieldName = "Comments";
            this.colComments.Name = "colComments";
            this.colComments.OptionsColumn.AllowShowHide = false;
            // 
            // colContact
            // 
            this.colContact.FieldName = "Contact";
            this.colContact.Name = "colContact";
            this.colContact.Visible = true;
            this.colContact.VisibleIndex = 4;
            this.colContact.Width = 146;
            // 
            // colOrganization
            // 
            this.colOrganization.Caption = "Організація";
            this.colOrganization.FieldName = "Organization";
            this.colOrganization.Name = "colOrganization";
            this.colOrganization.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colOrganization.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.colOrganization.Visible = true;
            this.colOrganization.VisibleIndex = 2;
            this.colOrganization.Width = 146;
            // 
            // colInfoSourceType
            // 
            this.colInfoSourceType.FieldName = "InfoSourceType";
            this.colInfoSourceType.Name = "colInfoSourceType";
            this.colInfoSourceType.Width = 112;
            // 
            // colApplication
            // 
            this.colApplication.Caption = "Компонент";
            this.colApplication.FieldName = "Application";
            this.colApplication.Name = "colApplication";
            this.colApplication.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colApplication.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.colApplication.Visible = true;
            this.colApplication.VisibleIndex = 3;
            this.colApplication.Width = 146;
            // 
            // colTagsString
            // 
            this.colTagsString.FieldName = "TagsString";
            this.colTagsString.Name = "colTagsString";
            // 
            // colTags
            // 
            this.colTags.FieldName = "Tags";
            this.colTags.Name = "colTags";
            this.colTags.OptionsColumn.AllowShowHide = false;
            // 
            // colAttaches
            // 
            this.colAttaches.FieldName = "Attaches";
            this.colAttaches.Name = "colAttaches";
            this.colAttaches.OptionsColumn.AllowShowHide = false;
            // 
            // colBugNumber
            // 
            this.colBugNumber.FieldName = "BugNumber";
            this.colBugNumber.Name = "colBugNumber";
            // 
            // colCMVersion
            // 
            this.colCMVersion.FieldName = "CMVersion";
            this.colCMVersion.Name = "colCMVersion";
            // 
            // colComponentVersion
            // 
            this.colComponentVersion.FieldName = "ComponentVersion";
            this.colComponentVersion.Name = "colComponentVersion";
            // 
            // colInfoSourceTypeId
            // 
            this.colInfoSourceTypeId.FieldName = "InfoSourceTypeId";
            this.colInfoSourceTypeId.Name = "colInfoSourceTypeId";
            this.colInfoSourceTypeId.OptionsColumn.AllowShowHide = false;
            this.colInfoSourceTypeId.OptionsColumn.ReadOnly = true;
            // 
            // colIsImportantString
            // 
            this.colIsImportantString.FieldName = "IsImportantString";
            this.colIsImportantString.Name = "colIsImportantString";
            this.colIsImportantString.OptionsColumn.AllowShowHide = false;
            this.colIsImportantString.OptionsColumn.ReadOnly = true;
            // 
            // colNumId
            // 
            this.colNumId.FieldName = "NumId";
            this.colNumId.Name = "colNumId";
            this.colNumId.OptionsColumn.AllowShowHide = false;
            this.colNumId.OptionsColumn.ReadOnly = true;
            // 
            // colIsNewEntity
            // 
            this.colIsNewEntity.FieldName = "IsNewEntity";
            this.colIsNewEntity.Name = "colIsNewEntity";
            this.colIsNewEntity.OptionsColumn.AllowShowHide = false;
            this.colIsNewEntity.OptionsColumn.ReadOnly = true;
            // 
            // colCreatorUser
            // 
            this.colCreatorUser.Caption = "Створив";
            this.colCreatorUser.FieldName = "CreatorUser";
            this.colCreatorUser.Name = "colCreatorUser";
            this.colCreatorUser.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colCreatorUser.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.colCreatorUser.Width = 101;
            // 
            // colResponseUser
            // 
            this.colResponseUser.Caption = "Відподівальний";
            this.colResponseUser.FieldName = "ResponseUser";
            this.colResponseUser.Name = "colResponseUser";
            this.colResponseUser.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colResponseUser.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.colResponseUser.Visible = true;
            this.colResponseUser.VisibleIndex = 5;
            this.colResponseUser.Width = 146;
            // 
            // colIsImportant
            // 
            this.colIsImportant.Caption = "!";
            this.colIsImportant.FieldName = "IsImportant";
            this.colIsImportant.Name = "colIsImportant";
            this.colIsImportant.Visible = true;
            this.colIsImportant.VisibleIndex = 7;
            this.colIsImportant.Width = 20;
            // 
            // RequestListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RequestGridControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "RequestListControl";
            this.Size = new System.Drawing.Size(1036, 434);
            ((System.ComponentModel.ISupportInitialize)(this.BarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FiltersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestListViewModelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReqListImageCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestBndingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager BarManager;
        private DevExpress.XtraBars.Bar MainToolBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem AddBarButton;
        private DevExpress.XtraBars.BarButtonItem EditBarButton;
        private DevExpress.XtraBars.BarButtonItem CloneBarButton;
        private DevExpress.XtraBars.BarButtonItem RemoveBarButton;
        private DevExpress.XtraGrid.GridControl RequestGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView RequestGridView;
        private DevExpress.XtraBars.BarButtonItem AuditBarButton;
        private System.Windows.Forms.BindingSource RequestBndingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colReqDateTime;
        private DevExpress.XtraGrid.Columns.GridColumn colCreateDateTime;
        private DevExpress.XtraGrid.Columns.GridColumn colSubject;
        private DevExpress.XtraGrid.Columns.GridColumn colComments;
        private DevExpress.XtraGrid.Columns.GridColumn colContact;
        private DevExpress.XtraGrid.Columns.GridColumn colOrganization;
        private DevExpress.XtraGrid.Columns.GridColumn colResponseUser;
        private DevExpress.XtraGrid.Columns.GridColumn colInfoSourceType;
        private DevExpress.XtraGrid.Columns.GridColumn colApplication;
        private DevExpress.XtraGrid.Columns.GridColumn colTagsString;
        private DevExpress.XtraGrid.Columns.GridColumn colTags;
        private DevExpress.XtraGrid.Columns.GridColumn colAttaches;
        private DevExpress.XtraGrid.Columns.GridColumn colState;
        private DevExpress.XtraGrid.Columns.GridColumn colBugNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colCMVersion;
        private DevExpress.XtraGrid.Columns.GridColumn colComponentVersion;
        private DevExpress.XtraGrid.Columns.GridColumn colCreatorUser;
        private DevExpress.XtraGrid.Columns.GridColumn colIsImportant;
        private DevExpress.XtraGrid.Columns.GridColumn colStateId;
        private DevExpress.XtraGrid.Columns.GridColumn colInfoSourceTypeId;
        private DevExpress.XtraGrid.Columns.GridColumn colIsImportantString;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colNumId;
        private DevExpress.XtraGrid.Columns.GridColumn colIsNewEntity;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarEditItem FilterListBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraBars.BarButtonItem EditFilterBarButtonItem;
        private DevExpress.XtraGrid.Columns.GridColumn colIsPersist;
        private System.Windows.Forms.BindingSource FiltersBindingSource;
        private System.Windows.Forms.BindingSource RequestListViewModelBindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem AddFilterBarButtonItem;
        private DevExpress.XtraBars.BarButtonItem DelFilterBarButtonItem;
        private DevExpress.Utils.ImageCollection ReqListImageCollection;
    }
}
