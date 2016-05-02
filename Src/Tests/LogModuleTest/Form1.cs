using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogModuleTest
{
    public partial class Form1 : Form
    {
        public Form1(Core.SDK.Log.ILogMgr logMgr)
        {
            InitializeComponent();           
            _logger = logMgr.GetLogger("MainForm");

            _logger.Debug("FirstDebug");
            _logger.Info("FirstInfo");
            _logger.Warn("FirstWarn");
            _logger.Error("FirstError");
            _logger.Fatal("FirstFatal");
        }
        Core.SDK.Log.ILogger _logger;

        private void button1_Click(object sender, EventArgs e)
        {
            _logger.Debug(new EvaluateException("Test ex1"), "Debug ex");
            _logger.Info(new EvaluateException("Test ex1"), "Info");
            _logger.Warn(new EvaluateException("Test ex1"), "Warn");
            _logger.Error(new EvaluateException("Test ex1"), "Error");
            _logger.Fatal(new EvaluateException("Test ex1"), "Fatal");
        }
    }
}
