using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.SDK.Composite.UI;
using Core.SDK.Log;
using Core.XtraCompositeModule;
using DevExpress.XtraBars;
using Core.SDK.Composite.Common;
using Core.XtraCompositeModule.DialogMgr;
using DevExpress.XtraEditors;

namespace XtraCompositeTest
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();

            _LogMgr = new Core.LogModule.LogMgr(@"J:\Other_project\MyUtils\Deps\NLog\NLog.config");
            _Logger = _LogMgr.GetLogger("XtraCompositeTest");
            _Logger.Info("Start XtraCompositeTest.");

            _regionMgr = new RegionMgr(_LogMgr);
            ICommandRegion MainMenuRegion = new BarCommandRegion(Core.SDK.Composite.UI.CommonRegionName.MainMenu, barManager1.MainMenu, 0);
            _regionMgr.AddCommandRegion(MainMenuRegion.Name, MainMenuRegion);           
        }

        IRegionMgr _regionMgr;
        ILogMgr _LogMgr;
        ILogger _Logger;
        IdentKey _key;
        IdentKey _key2, _key3;
        ICommand _command;        
        bool _isEnable = true;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _command = new Command("testcommand", () => { MessageBox.Show("test1"); }, IsEnable);
            _key = _regionMgr.GetCommandRegion(Core.SDK.Composite.UI.CommonRegionName.MainMenu).AddCommand(_command, "MainGroupItem");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            _isEnable = false;
            _regionMgr.GetCommandRegion(Core.SDK.Composite.UI.CommonRegionName.MainMenu).RefreshCommand(_key);
        }

        private bool IsEnable()
        {
            return _isEnable;
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            ICommand command = new Command("testcommand2", () => { }, IsEnable, "NewMenu", null, null, "1", null, Core.SDK.Composite.UI.CommandType.Group);
            _key2 = _regionMgr.GetCommandRegion(Core.SDK.Composite.UI.CommonRegionName.MainMenu).AddCommand(command);
            command = new Command("testcommand2_1", () => { MessageBox.Show("test2_1"); }, IsEnable, "NewMenuItem", null, null, "1_1", null, Core.SDK.Composite.UI.CommandType.CheckButton);
            _key3 = _regionMgr.GetCommandRegion(Core.SDK.Composite.UI.CommonRegionName.MainMenu).AddCommand(command, "NewMenu");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            _regionMgr.GetCommandRegion(Core.SDK.Composite.UI.CommonRegionName.MainMenu).RemoveCommand(_key2);
        }        
    }
}
