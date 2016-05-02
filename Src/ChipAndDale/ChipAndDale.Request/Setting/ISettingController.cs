using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChipAndDale.SDK.Request;
using ChipAndDale.Request.ViewModel;

namespace ChipAndDale.Request.Setting
{
    internal interface ISettingController
    {
        void Load();
        
        void Save();

        void SetToDefault();

        List<RequestListFilterEntity> Filters { get; set; }

        List<RequestListViewModel> ListViewModels { get; set; }

        List<RequestEditViewModel> EditViewModels { get; set; }

        string SaveRequestToSetting(RequestEntity request);

        RequestEntity LoadRequestFromSetting(string settingName);
    }
}
