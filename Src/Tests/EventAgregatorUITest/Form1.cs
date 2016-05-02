using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.SDK.Composite.Event;

namespace EventAgregatorUITest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _LogMgr = new Core.LogModule.LogMgr(@"J:\Other_project\MyUtils\Deps\NLog\NLog.config");
            _Logger = _LogMgr.GetLogger("EventAgregatorUITest");
            _Logger.Info("Start EventAgregatorUITest.");
            _eventMgr = new EventMgr(_LogMgr);
            _eventMgr.GetEvent<StringEvent>().Subscribe(UpdateLabel, ThreadOption.UIThread);
        }

        IEventMgr _eventMgr;
        Core.SDK.Log.ILogMgr _LogMgr;
        Core.SDK.Log.ILogger _Logger;

        private void button1_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender1, arg) => { _eventMgr.GetEvent<StringEvent>().Publish("NewText!!!"); };
            worker.RunWorkerAsync(null);
        }

        void UpdateLabel(string text)
        {
            label1.Text = text;
        }

        public class StringEvent : CompositeEvent<string>
        { }
    }
}
