using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChipAndDale.SDK.Request;
using ChipAndDale.Request.ViewModel;

namespace ChipAndDale.Request
{
    internal interface IMainController : IRequestService
    {
        void Prepocess();

        void OpenRequestListPage(RequestListFilterEntity filter);
        void OpenRequestEditForm(RequestEntity request, bool readOnly);

        bool ProcessRequestViewModel(RequestEditViewModel viewModel);
        void OnAfterCloseRequestViewModel(RequestEditViewModel viewModel);
        
        void OnAfterCloseRequesListViewModel(RequestListViewModel viewModel);

        RequestEditViewModel CreateRequestEditViewModel(RequestEntity request, bool readOnly, bool isProcess, bool isCommit);
        RequestListViewModel CreateRequestListViewModel(RequestListFilterEntity filter, bool readOnly);
        RequestFilterViewModel CreateRequestFilterViewModel(RequestListFilterEntity filter);

        void LoadSetting();
        void SaveSetting();

        List<RequestListFilterEntity> Filters { get; }
        RequestListFilterEntity OnSaveFilter(RequestListFilterEntity filter);
        void OnDeleteFilter(RequestListFilterEntity filter);
    }
}
