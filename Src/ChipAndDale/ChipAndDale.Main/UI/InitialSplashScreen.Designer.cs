namespace ChipAndDale.Main.UI
{
    partial class InitialSplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialSplashScreen));
            this.SplashMarqueeProgressBarControl = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.SplashMarqueeProgressBarControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SplashMarqueeProgressBarControl
            // 
            this.SplashMarqueeProgressBarControl.EditValue = 0;
            this.SplashMarqueeProgressBarControl.Location = new System.Drawing.Point(11, 301);
            this.SplashMarqueeProgressBarControl.Name = "SplashMarqueeProgressBarControl";
            this.SplashMarqueeProgressBarControl.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.SplashMarqueeProgressBarControl.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.SplashMarqueeProgressBarControl.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.SplashMarqueeProgressBarControl.Properties.EndColor = System.Drawing.Color.Blue;
            this.SplashMarqueeProgressBarControl.Properties.LookAndFeel.SkinName = "High Contrast";
            this.SplashMarqueeProgressBarControl.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.SplashMarqueeProgressBarControl.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.SplashMarqueeProgressBarControl.Properties.MarqueeAnimationSpeed = 80;
            this.SplashMarqueeProgressBarControl.Properties.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.PingPong;
            this.SplashMarqueeProgressBarControl.Properties.StartColor = System.Drawing.Color.DeepSkyBlue;
            this.SplashMarqueeProgressBarControl.Size = new System.Drawing.Size(539, 20);
            this.SplashMarqueeProgressBarControl.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 275);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(50, 13);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Starting...";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl1.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.labelControl1.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.DodgerBlue;
            this.labelControl1.LineColor = System.Drawing.Color.Transparent;
            this.labelControl1.Location = new System.Drawing.Point(13, 270);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(135, 25);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "ChipAndDale ©";
            // 
            // InitialSplashScreen
            // 
            this.AllowControlsInImageMode = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 335);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.SplashMarqueeProgressBarControl);
            this.Controls.Add(this.labelControl2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(562, 335);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(562, 335);
            this.Name = "InitialSplashScreen";
            this.Opacity = 0.7D;
            this.ShowIcon = false;
            this.ShowMode = DevExpress.XtraSplashScreen.ShowMode.Image;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.SplashImage = ((System.Drawing.Image)(resources.GetObject("$this.SplashImage")));
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SplashMarqueeProgressBarControl.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl SplashMarqueeProgressBarControl;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
