using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChipAndDale.Request.ViewModel;
using System.Drawing;

namespace ChipAndDale.Request.Setting
{    
    internal class EditViewModelSetting
    {
        public EditViewModelSetting() { }

        #region Properties        

        public string RequestName { get; set; }

        public Point Position { get; set; }

        public Size Size { get; set; }

        public bool IsCommit { get; set; }

        public bool IsProcess { get; set; }

        public bool ReadOnly { get; set; }

        #endregion Properties                        
    }
}
