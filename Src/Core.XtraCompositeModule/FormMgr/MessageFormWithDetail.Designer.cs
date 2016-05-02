namespace Core.XtraCompositeModule.DialogMgr
{
    partial class MessageFormWithDetail
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.DetailButton = new DevExpress.XtraEditors.SimpleButton();
            this.OkButton = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.MessageLabel = new DevExpress.XtraEditors.LabelControl();
            this.ImageEdit = new DevExpress.XtraEditors.PictureEdit();
            this.DetailPanelControl = new DevExpress.XtraEditors.PanelControl();
            this.DetailMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailPanelControl)).BeginInit();
            this.DetailPanelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailMemoEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.DetailButton);
            this.panelControl1.Controls.Add(this.OkButton);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 267);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(424, 28);
            this.panelControl1.TabIndex = 4;
            // 
            // DetailButton
            // 
            this.DetailButton.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DetailButton.Appearance.Options.UseFont = true;
            this.DetailButton.Location = new System.Drawing.Point(228, 3);
            this.DetailButton.Name = "DetailButton";
            this.DetailButton.Size = new System.Drawing.Size(100, 23);
            this.DetailButton.TabIndex = 1;
            this.DetailButton.Text = "Показати";
            this.DetailButton.Click += new System.EventHandler(this.DetailButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OkButton.Appearance.Options.UseFont = true;
            this.OkButton.Location = new System.Drawing.Point(109, 3);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(100, 23);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // panelControl4
            // 
            this.panelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl4.Controls.Add(this.MessageLabel);
            this.panelControl4.Controls.Add(this.ImageEdit);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(424, 83);
            this.panelControl4.TabIndex = 8;
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoEllipsis = true;
            this.MessageLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.MessageLabel.Location = new System.Drawing.Point(65, 11);
            this.MessageLabel.MaximumSize = new System.Drawing.Size(295, 100);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(295, 13);
            this.MessageLabel.TabIndex = 9;
            this.MessageLabel.Text = "labelControl1";
            // 
            // ImageEdit
            // 
            this.ImageEdit.Location = new System.Drawing.Point(12, 29);
            this.ImageEdit.Name = "ImageEdit";
            this.ImageEdit.Properties.AllowFocused = false;
            this.ImageEdit.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ImageEdit.Properties.Appearance.Options.UseBackColor = true;
            this.ImageEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ImageEdit.Properties.ShowMenu = false;
            this.ImageEdit.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.False;
            this.ImageEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.ImageEdit.Size = new System.Drawing.Size(38, 38);
            this.ImageEdit.TabIndex = 8;
            // 
            // DetailPanelControl
            // 
            this.DetailPanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.DetailPanelControl.Controls.Add(this.DetailMemoEdit);
            this.DetailPanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailPanelControl.Location = new System.Drawing.Point(0, 83);
            this.DetailPanelControl.Name = "DetailPanelControl";
            this.DetailPanelControl.Size = new System.Drawing.Size(424, 184);
            this.DetailPanelControl.TabIndex = 9;
            // 
            // DetailMemoEdit
            // 
            this.DetailMemoEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailMemoEdit.Location = new System.Drawing.Point(0, 0);
            this.DetailMemoEdit.Name = "DetailMemoEdit";
            this.DetailMemoEdit.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.DetailMemoEdit.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DetailMemoEdit.Properties.Appearance.Options.UseBackColor = true;
            this.DetailMemoEdit.Properties.Appearance.Options.UseFont = true;
            this.DetailMemoEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.DetailMemoEdit.Properties.ReadOnly = true;
            this.DetailMemoEdit.Size = new System.Drawing.Size(424, 184);
            this.DetailMemoEdit.TabIndex = 0;
            // 
            // MessageFormWithDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 295);
            this.Controls.Add(this.DetailPanelControl);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageFormWithDetail";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailPanelControl)).EndInit();
            this.DetailPanelControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetailMemoEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton OkButton;
        private DevExpress.XtraEditors.SimpleButton DetailButton;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.LabelControl MessageLabel;
        private DevExpress.XtraEditors.PictureEdit ImageEdit;
        private DevExpress.XtraEditors.PanelControl DetailPanelControl;
        private DevExpress.XtraEditors.MemoEdit DetailMemoEdit;




    }
}