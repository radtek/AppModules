using System;
using ChipAndDale.SDK.Nsi;
using ChipAndDale.SDK.Request;
using Core.SDK.Dom;
using ChipAndDale.SDK.Common;

namespace ChipAndDale.Request.Db
{
    internal interface IDbAccessor
    {
        BindingCollection<RequestEntity> GetRequests(RequestListFilterEntity filter);
        RequestEntity GetRequestById(string requestId);
        BindingCollection<TagEntity> GetRequestTags(string requestId);
        BindingCollection<AttachEntity> GetRequestAttaches(string requestId);

        string InsRequest(RequestEntity request);
        void UpdRequest(RequestEntity request);
        void DelRequest(string requestId);

        void InsReqTag(string requestId, TagEntity tag);
        void DelReqTag(string requestId, string tagId);

        void InsReqAttach(string requestId, AttachEntity attach);
        void DelReqAttach(string attachId);
        void CopyReqAttach(string requestIdFrom, string requestIdTo);
        byte[] SelAttachBlob(string attachId);

        string GetRequestAuditReport(string requestId);
        string GetRequestForPeriodReport(DateTime from, DateTime until, long? id_org, long? id_comp, long? id_user, long? id_creator, string tags, string state);
    }
}
