using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Dom;

namespace ChipAndDale.SDK.Request
{
    public interface IRequestService
    {        
        RequestEntity OpenRequestEditDialog(RequestEntity request, bool readOnly, bool isProcess, bool isCommit);
        IEnumerable<RequestEntity> OpenRequstListDialog(RequestListFilterEntity filter);

        RequestListFilterEntity OpenRequestListFilterEditDialog(RequestListFilterEntity filter);

        BindingCollection<RequestEntity> GetRequstList(RequestListFilterEntity filter);
        RequestEntity GetRequestById(string requestId);
        void AddRequest(RequestEntity request, bool isCommit);
        void RemoveRequest(string requestId, bool isCommit);
        void UpdateRequest(RequestEntity requestBefore, RequestEntity requestAfter, bool isCommit);

        IList<KeyValuePair<Enum, string>> RequestStates { get; }

        bool IsRequestFiltered(RequestEntity request, RequestListFilterEntity filter);

        string SaveRequestToSetting(RequestEntity request);
        RequestEntity LoadRequestFromSetting(string settingName);
    }
}
