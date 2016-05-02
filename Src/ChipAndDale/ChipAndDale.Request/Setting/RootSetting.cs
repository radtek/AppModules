using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChipAndDale.Request.Setting
{
    internal class RootSetting
    {
        public RootSetting() 
        {
            ListViewModelList = new List<string>();
            EditViewModelList = new List<string>();
            FilterList = new List<string>();
        }

        #region Properties

        public List<string> ListViewModelList { get; set; }

        public List<string> EditViewModelList { get; set; }

        public List<string> FilterList { get; set; }

        #endregion Properties
    }
}
