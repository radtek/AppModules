using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChipAndDale.Request.ViewModel;
using ChipAndDale.SDK.Request;

namespace ChipAndDale.Request.Setting
{
    internal class ListViewModelSetting
    {
        public ListViewModelSetting() { }

        #region Properties

        public string CurrentFilterName { get; set; }

        public byte[] GridData { get; set; }

        public string CurrentRequestId { get; set; }        

        public string EditViewModelList { get; set; }

        public bool ReadOnly { get; set; }

        #endregion Properties
        
        
        internal void InitFrom(RequestListViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("ViewModel param can not be null.");

            CurrentFilterName = viewModel.Filter.FilterName;
            GridData = viewModel.GridData;
            if (viewModel.SelectRequestList != null)
                foreach (RequestEntity request in viewModel.SelectRequestList)
                {
                    CurrentRequestId = request.Id;
                }
        }

        internal RequestListViewModel ConvertTo()
        {
            return null;
        }
    }
}
