﻿namespace ChipAndDale.Main.Nsi
{
    partial class UserListControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NsiBindingSource = new System.Windows.Forms.BindingSource();
            this.NsiBarManager = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.AddButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.RemoveButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.FieldBarListItem = new DevExpress.XtraBars.BarListItem();
            this.FilterBarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.RecordCountStatusBar = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.OkButton = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.CancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.UserGridControl = new DevExpress.XtraGrid.GridControl();
            this.UserGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colLogin = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPhone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInfo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBirthDay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrganization = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsNewEntity = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.NsiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NsiBarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // NsiBindingSource
            // 
            this.NsiBindingSource.DataSource = typeof(ChipAndDale.SDK.Nsi.UserEntity);
            this.NsiBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.NsiBindingSource_ListChanged);
            // 
            // NsiBarManager
            // 
            this.NsiBarManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3});
            this.NsiBarManager.DockControls.Add(this.barDockControlTop);
            this.NsiBarManager.DockControls.Add(this.barDockControlBottom);
            this.NsiBarManager.DockControls.Add(this.barDockControlLeft);
            this.NsiBarManager.DockControls.Add(this.barDockControlRight);
            this.NsiBarManager.Form = this;
            this.NsiBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.AddButtonItem,
            this.RemoveButtonItem,
            this.FieldBarListItem,
            this.FilterBarEditItem,
            this.RecordCountStatusBar});
            this.NsiBarManager.MaxItemId = 5;
            this.NsiBarManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.NsiBarManager.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarAppearance.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.bar1.BarAppearance.Normal.Options.UseFont = true;
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.AddButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.RemoveButtonItem),
            new DevExpress.XtraBars.LinkPersistInfo(this.FieldBarListItem, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.FilterBarEditItem)});
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // AddButtonItem
            // 
            this.AddButtonItem.Caption = "Add";
            this.AddButtonItem.Glyph = global::ChipAndDale.Main.Properties.Resources.Add;
            this.AddButtonItem.Id = 0;
            this.AddButtonItem.Name = "AddButtonItem";
            this.AddButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AddButtonItem_ItemClick);
            // 
            // RemoveButtonItem
            // 
            this.RemoveButtonItem.Caption = "Remove";
            this.RemoveButtonItem.Glyph = global::ChipAndDale.Main.Properties.Resources.Remove;
            this.RemoveButtonItem.Id = 1;
            this.RemoveButtonItem.Name = "RemoveButtonItem";
            this.RemoveButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.RemoveButtonItem_ItemClick);
            // 
            // FieldBarListItem
            // 
            this.FieldBarListItem.Caption = "Поле";
            this.FieldBarListItem.Id = 2;
            this.FieldBarListItem.Name = "FieldBarListItem";
            // 
            // FilterBarEditItem
            // 
            this.FilterBarEditItem.Caption = "value";
            this.FilterBarEditItem.Edit = this.repositoryItemTextEdit1;
            this.FilterBarEditItem.Id = 3;
            this.FilterBarEditItem.ItemAppearance.Normal.Font = new System.Drawing.Font("Verdana", 9F);
            this.FilterBarEditItem.ItemAppearance.Normal.Options.UseFont = true;
            this.FilterBarEditItem.Name = "FilterBarEditItem";
            this.FilterBarEditItem.Width = 200;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.RecordCountStatusBar)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // RecordCountStatusBar
            // 
            this.RecordCountStatusBar.Id = 4;
            this.RecordCountStatusBar.Name = "RecordCountStatusBar";
            this.RecordCountStatusBar.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(543, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 474);
            this.barDockControlBottom.Size = new System.Drawing.Size(543, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 445);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(543, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 445);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.OkButton);
            this.panelControl2.Controls.Add(this.panelControl4);
            this.panelControl2.Controls.Add(this.CancelButton);
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 445);
            this.panelControl2.MaximumSize = new System.Drawing.Size(0, 29);
            this.panelControl2.MinimumSize = new System.Drawing.Size(0, 29);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(543, 29);
            this.panelControl2.TabIndex = 11;
            // 
            // OkButton
            // 
            this.OkButton.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OkButton.Appearance.Options.UseFont = true;
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.OkButton.Image = global::ChipAndDale.Main.Properties.Resources.Ok_18;
            this.OkButton.Location = new System.Drawing.Point(314, 2);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(100, 25);
            this.OkButton.TabIndex = 5;
            this.OkButton.Text = "Ок";
            // 
            // panelControl4
            // 
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl4.Location = new System.Drawing.Point(414, 2);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(10, 25);
            this.panelControl4.TabIndex = 4;
            // 
            // CancelButton
            // 
            this.CancelButton.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelButton.Appearance.Options.UseFont = true;
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CancelButton.Image = global::ChipAndDale.Main.Properties.Resources.Cancel_18;
            this.CancelButton.Location = new System.Drawing.Point(424, 2);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(5);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Padding = new System.Windows.Forms.Padding(5);
            this.CancelButton.Size = new System.Drawing.Size(100, 25);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Відмінити";
            // 
            // panelControl3
            // 
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl3.Location = new System.Drawing.Point(524, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(17, 25);
            this.panelControl3.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.UserGridControl);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 29);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(543, 416);
            this.panelControl1.TabIndex = 16;
            // 
            // UserGridControl
            // 
            this.UserGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserGridControl.Location = new System.Drawing.Point(2, 2);
            this.UserGridControl.MainView = this.UserGridView;
            this.UserGridControl.MenuManager = this.NsiBarManager;
            this.UserGridControl.Name = "UserGridControl";
            this.UserGridControl.Size = new System.Drawing.Size(539, 412);
            this.UserGridControl.TabIndex = 0;
            this.UserGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.UserGridView});
            // 
            // UserGridView
            // 
            this.UserGridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UserGridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.UserGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.UserGridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UserGridView.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.UserGridView.Appearance.Row.Options.UseFont = true;
            this.UserGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colId,
            this.colLogin,
            this.colName,
            this.colPhone,
            this.colInfo,
            this.colBirthDay,
            this.colOrganization,
            this.colIsNewEntity});
            this.UserGridView.GridControl = this.UserGridControl;
            this.UserGridView.Name = "UserGridView";
            this.UserGridView.OptionsView.EnableAppearanceEvenRow = true;
            this.UserGridView.OptionsView.ShowGroupPanel = false;
            // 
            // colLogin
            // 
            this.colLogin.Caption = "Логін";
            this.colLogin.FieldName = "Login";
            this.colLogin.Name = "colLogin";
            this.colLogin.Visible = true;
            this.colLogin.VisibleIndex = 1;
            this.colLogin.Width = 100;
            // 
            // colName
            // 
            this.colName.Caption = "ПІБ";
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 2;
            this.colName.Width = 150;
            // 
            // colPhone
            // 
            this.colPhone.Caption = "Телефон";
            this.colPhone.FieldName = "Phone";
            this.colPhone.Name = "colPhone";
            this.colPhone.Visible = true;
            this.colPhone.VisibleIndex = 3;
            this.colPhone.Width = 50;
            // 
            // colInfo
            // 
            this.colInfo.FieldName = "Info";
            this.colInfo.Name = "colInfo";
            // 
            // colBirthDay
            // 
            this.colBirthDay.Caption = "ДР";
            this.colBirthDay.FieldName = "BirthDay";
            this.colBirthDay.Name = "colBirthDay";
            this.colBirthDay.Visible = true;
            this.colBirthDay.VisibleIndex = 4;
            this.colBirthDay.Width = 50;
            // 
            // colOrganization
            // 
            this.colOrganization.FieldName = "Organization";
            this.colOrganization.Name = "colOrganization";
            // 
            // colId
            // 
            this.colId.Caption = "№";
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = true;
            this.colId.VisibleIndex = 0;
            this.colId.Width = 30;
            // 
            // colIsNewEntity
            // 
            this.colIsNewEntity.FieldName = "IsNewEntity";
            this.colIsNewEntity.Name = "colIsNewEntity";
            this.colIsNewEntity.OptionsColumn.ReadOnly = true;
            // 
            // UserListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UserListControl";
            this.Size = new System.Drawing.Size(543, 501);
            ((System.ComponentModel.ISupportInitialize)(this.NsiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NsiBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UserGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource NsiBindingSource;
        private DevExpress.XtraBars.BarManager NsiBarManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem AddButtonItem;
        private DevExpress.XtraBars.BarButtonItem RemoveButtonItem;
        private DevExpress.XtraBars.BarListItem FieldBarListItem;
        private DevExpress.XtraBars.BarEditItem FilterBarEditItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarStaticItem RecordCountStatusBar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton OkButton;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.SimpleButton CancelButton;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl UserGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView UserGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colLogin;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colPhone;
        private DevExpress.XtraGrid.Columns.GridColumn colInfo;
        private DevExpress.XtraGrid.Columns.GridColumn colBirthDay;
        private DevExpress.XtraGrid.Columns.GridColumn colOrganization;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colIsNewEntity;
    }
}
