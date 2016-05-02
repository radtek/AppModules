using System;
using DevExpress.XtraWaitForm;
using System.Drawing;

namespace ChipAndDale.Main.UI
{
    public partial class MainWaitForm : WaitForm
    {
        public MainWaitForm()
        {
            InitializeComponent();
            this.WaitProgressPanel.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.WaitProgressPanel.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.WaitProgressPanel.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }

        private void MainWaitForm_Load(object sender, EventArgs e)
        {
            Location = new Point(Location.X, Location.Y + 200);
        }
    }
}