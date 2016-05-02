using System;
using DevExpress.XtraSplashScreen;

namespace ChipAndDale.Main.UI
{
    public partial class InitialSplashScreen : SplashScreen
    {
        public InitialSplashScreen()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }
    }
}