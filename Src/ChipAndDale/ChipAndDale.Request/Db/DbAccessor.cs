using System;
using System.Data;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Request;
using Core.OracleModule;
using Core.SDK.Common;
using Core.SDK.Composite.Service;
using Core.SDK.Db;
using Core.SDK.Dom;
using Core.SDK.Log;
using ChipAndDale.SDK.Nsi;

namespace ChipAndDale.Request.Db
{
    internal class DbAccessor : IDbAccessor
    {
        internal DbAccessor()
        {
            _dbMgr = ServiceMgr.Current.GetInstance<IDbMgr>();
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();
            _commonDbService = ServiceMgr.Current.GetInstance<ICommonService>();
            _nsiService = ServiceMgr.Current.GetInstance<INsiService>();
            
            _logger = _logMgr.GetLogger("Requst.DbAccessor");
            _logger.Debug("Create.");
            _logger.Debug("Interfaces: IDBMgr = {0}; ICommonDbService = {1}; INsiService = {2}.", _dbMgr.ToStateString(), _commonDbService.ToStateString(), _nsiService.ToStateString());
        }

        public BindingCollection<RequestEntity> GetRequests(RequestListFilterEntity filter)
        {
            _logger.Debug("GetRequests.");
            if (filter == null) throw new ArgumentNullException("Filter param can not be null.");
            _logger.Debug("Params: filter = {0};", filter.ToInternalString());

            BindingCollection<RequestEntity> result = new BindingCollection<RequestEntity>();

            _commonDbService.SetParam("FROM", filter.StartDateTimeString);
            _commonDbService.SetParam("UNTIL", filter.StopDateTimeString);
            _commonDbService.SetParam("STATUS", filter.StatusIdList);
            _commonDbService.SetParam("ID_ORG", filter.OrganizationIdtString);
            _commonDbService.SetParam("ID_COMP", filter.ApplicationIdtString);
            _commonDbService.SetParam("ID_USER", filter.UserIdtString);
            _commonDbService.SetParam("ID_CREATOR", filter.CreatorIdtString);
            _commonDbService.SetParam("TAGS", filter.TagIdList);
            
            OraCommand command = new OraCommand("REQ_PKG.SEL_REQ");
            command.CommandType = CommandType.StoredProcedure;
            OraParamRefCursor refCur = new OraParamRefCursor("p_req_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);          
            Execute(command);

            if (refCur.ParamValue != null)
                foreach (DataRow reqRow in refCur.ParamValue.Rows)
                    result.Add(ToRequest(reqRow));
                
            return result;
        }

        public RequestEntity GetRequestById(string requestId)
        {
            _logger.Debug("GetRequestById.");
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId param can not be null.");
            _logger.Debug("Params: requestId = {0};", requestId.ToString());

            RequestEntity result = null;

            OraCommand command = new OraCommand("REQ_PKG.SEL_REQ_BYID");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_id", ParameterDirection.Input, Convert.ToInt32(requestId)));
            OraParamRefCursor refCur = new OraParamRefCursor("p_req_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);
            Execute(command);

            if (refCur.ParamValue != null && refCur.ParamValue.Rows.Count > 0)
            {
                result = ToRequest(refCur.ParamValue.Rows[0]);
                result.Tags.Fill(GetRequestTags(requestId));
                result.Attaches.Fill(GetRequestAttaches(requestId));
            }

            return result;
        }

        public BindingCollection<TagEntity> GetRequestTags(string requestId)
        {
            _logger.Debug("GetRequestTags.");
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId param can not be null.");
            _logger.Debug("Params: requestId = {0};", requestId.ToString());

            BindingCollection<TagEntity> result = new BindingCollection<TagEntity>();

            OraCommand command = new OraCommand("REQ_PKG.SEL_REQ_TAG");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, Convert.ToInt32(requestId)));
            OraParamRefCursor refCur = new OraParamRefCursor("p_reqTag_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);
            Execute(command);

            if (refCur.ParamValue != null)
                foreach (DataRow reqRow in refCur.ParamValue.Rows)
                    result.Add(ToTag(reqRow));           
            
            return result;
        }        

        public BindingCollection<AttachEntity> GetRequestAttaches(string requestId)
        {
            _logger.Debug("GetRequestAttaches.");
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId param can not be null.");
            _logger.Debug("Params: requestId = {0};", requestId.ToString());

            BindingCollection<AttachEntity> result = new BindingCollection<AttachEntity>();

            OraCommand command = new OraCommand("REQ_PKG.SEL_REQ_ATT");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, Convert.ToInt32(requestId)));
            OraParamRefCursor refCur = new OraParamRefCursor("p_reqAtt_cr", ParameterDirection.Output);
            command.AddDBParam(refCur);
            Execute(command);           

            if (refCur.ParamValue != null)
                foreach (DataRow reqRow in refCur.ParamValue.Rows)
                    result.Add(ToAttach(reqRow));

            return result;
        }

        public string InsRequest(RequestEntity request)
        {
            _logger.Debug("InsRequest.");
            if (request == null) throw new ArgumentNullException("Request param can not be null.");
            _logger.Debug("Params: request = {0};", request.ToInternalString());
            
            OraCommand command = new OraCommand("REQ_PKG.INS_REQ");
            command.CommandType = CommandType.StoredProcedure;
            OraParamInt32 returnParam = new OraParamInt32("return", ParameterDirection.ReturnValue, null);
            command.AddDBParam(returnParam);
            command.AddDBParam(new OraParamDateTime("p_req_date", ParameterDirection.Input, request.ReqDateTime));
            command.AddDBParam(new OraParamString("p_subject", ParameterDirection.Input, request.Subject));
            command.AddDBParam(new OraParamString("p_note", ParameterDirection.Input, request.Comments));
            command.AddDBParam(new OraParamInt32("p_orig_id", ParameterDirection.Input, request.Organization.NumId));
            command.AddDBParam(new OraParamString("p_contact", ParameterDirection.Input, request.Contact));
            command.AddDBParam(new OraParamInt32("p_req_type", ParameterDirection.Input, request.InfoSourceTypeId));
            command.AddDBParam(new OraParamInt32("p_comp_id", ParameterDirection.Input, request.Application.NumId));
            command.AddDBParam(new OraParamInt32("p_resp_id", ParameterDirection.Input, request.ResponseUser.NumId));
            command.AddDBParam(new OraParamInt32("p_req_state", ParameterDirection.Input, request.StateId));
            command.AddDBParam(new OraParamString("p_bug_num", ParameterDirection.Input, request.BugNumber));
            command.AddDBParam(new OraParamInt32("p_CM_num", ParameterDirection.Input, string.IsNullOrEmpty(request.CMVersion)? (int?)null : Convert.ToInt32(request.CMVersion)));
            command.AddDBParam(new OraParamString("p_ver_num", ParameterDirection.Input, request.ComponentVersion));
            command.AddDBParam(new OraParamString("p_important", ParameterDirection.Input, request.IsImportantString));
            OraParamDateTime createDateParam = new OraParamDateTime("p_create_date_out", ParameterDirection.Output, null);
            command.AddDBParam(createDateParam);
                        
            Execute(command);

            request.CreateDateTime = createDateParam.ParamValue.Value;
            request.Id = returnParam.ParamValue.Value.ToString();
            return request.Id;                                   
        }

        public void UpdRequest(RequestEntity request)
        {
            _logger.Debug("UpdRequest.");
            if (request == null) throw new ArgumentNullException("Request param can not be null.");
            _logger.Debug("Params: request= {0};", request.ToInternalString());

            OraCommand command = new OraCommand("REQ_PKG.UPD_REQ");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, request.NumId));
            command.AddDBParam(new OraParamDateTime("p_req_date", ParameterDirection.Input, request.ReqDateTime));
            command.AddDBParam(new OraParamString("p_subject", ParameterDirection.Input, request.Subject));
            command.AddDBParam(new OraParamString("p_note", ParameterDirection.Input, request.Comments));
            command.AddDBParam(new OraParamInt32("p_orig_id", ParameterDirection.Input, request.Organization.NumId));
            command.AddDBParam(new OraParamString("p_contact", ParameterDirection.Input, request.Contact));
            command.AddDBParam(new OraParamInt32("p_req_type", ParameterDirection.Input, request.InfoSourceTypeId));
            command.AddDBParam(new OraParamInt32("p_comp_id", ParameterDirection.Input, request.Application.NumId));
            command.AddDBParam(new OraParamInt32("p_resp_id", ParameterDirection.Input, request.ResponseUser.NumId));
            command.AddDBParam(new OraParamInt32("p_req_state", ParameterDirection.Input, request.StateId));
            command.AddDBParam(new OraParamString("p_bug_num", ParameterDirection.Input, request.BugNumber));
            command.AddDBParam(new OraParamInt32("p_CM_num", ParameterDirection.Input, string.IsNullOrEmpty(request.CMVersion) ? (int?)null : Convert.ToInt32(request.CMVersion)));
            command.AddDBParam(new OraParamString("p_ver_num", ParameterDirection.Input, request.ComponentVersion));
            command.AddDBParam(new OraParamString("p_important", ParameterDirection.Input, request.IsImportantString));                        

            Execute(command);            
        }

        public void DelRequest(string requestId)
        {
            _logger.Debug("DelRequest.");
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId param can not be null.");
            _logger.Debug("Params: requestId = {0};", requestId);

            OraCommand command = new OraCommand("REQ_PKG.DEL_REQ");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, Convert.ToInt32(requestId)));
            Execute(command);
        }

        public void InsReqTag(string requestId, TagEntity tag)
        {
            _logger.Debug("InsReqTag.");
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId param can not be null.");
            if (tag == null) throw new ArgumentNullException("Tag param can not be null.");
            _logger.Debug("Params: requestId = {0}; tag = {1};", requestId, tag.ToInternalString());            

            OraCommand command = new OraCommand("REQ_PKG.INS_REQ_TAG");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, Convert.ToInt32(requestId)));
            command.AddDBParam(new OraParamInt32("p_tag_id", ParameterDirection.Input, tag.NumId));
            Execute(command);
        }

        public void DelReqTag(string requestId, string tagId)
        {
            _logger.Debug("DelReqTag.");
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId param can not be null.");
            if (string.IsNullOrEmpty(tagId)) throw new ArgumentNullException("TagId param can not be null.");
            _logger.Debug("Params: requestId = {0};", requestId);

            OraCommand command = new OraCommand("REQ_PKG.DEL_REQ_TAG");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, Convert.ToInt32(requestId)));
            command.AddDBParam(new OraParamInt32("p_tag_id", ParameterDirection.Input, Convert.ToInt32(tagId)));
            Execute(command);           
        }

        public void InsReqAttach(string requestId, AttachEntity attach)
        {
            _logger.Debug("InsReqAttach.");
            if (string.IsNullOrEmpty(requestId)) throw new ArgumentNullException("RequestId param can not be null.");
            if (attach == null) throw new ArgumentNullException("Attach param can not be null.");
            _logger.Debug("Params: requestId = {0}; attach = {1};", requestId, attach.ToInternalString());            

            OraCommand command = new OraCommand("REQ_PKG.INS_REQ_ATT");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_req_id", ParameterDirection.Input, Convert.ToInt32(requestId)));
            command.AddDBParam(new OraParamString("p_att_name", ParameterDirection.Input, attach.Name));
            command.AddDBParam(new OraParamBLOB("p_att_data", ParameterDirection.Input, attach.Blob));
            Execute(command);           
        }

        public void DelReqAttach(string attachId)
        {
            _logger.Debug("DelRequest.");
            if (string.IsNullOrEmpty(attachId)) throw new ArgumentNullException("AttachId param can not be null.");
            _logger.Debug("Params: attachId = {0};", attachId);

            OraCommand command = new OraCommand("REQ_PKG.DEL_REQ_ATT");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_att_id", ParameterDirection.Input, Convert.ToInt32(attachId)));
            Execute(command);           
        }

        public void CopyReqAttach(string requestIdFrom, string requestIdTo)
        {
            _logger.Debug("CopyReqAttach.");
            if (string.IsNullOrEmpty(requestIdFrom)) throw new ArgumentNullException("RequestIdFrom param can not be null.");
            if (string.IsNullOrEmpty(requestIdTo)) throw new ArgumentNullException("RequestIdTo param can not be null.");
            _logger.Debug("Params: requestIdFrom = {0}; requestIdTo = {1};", requestIdFrom, requestIdTo);            

            OraCommand command = new OraCommand("REQ_PKG.COPY_REQ_ATT");
            command.CommandType = CommandType.StoredProcedure;
            command.AddDBParam(new OraParamInt32("p_reqFrom_id", ParameterDirection.Input, Convert.ToInt32(requestIdFrom)));
            command.AddDBParam(new OraParamInt32("p_reqTo_id", ParameterDirection.Input, Convert.ToInt32(requestIdTo)));
            Execute(command);              
        }

        public byte[] SelAttachBlob(string attachId)
        {
            _logger.Debug("SelAttachBlob.");
            if (string.IsNullOrEmpty(attachId)) throw new ArgumentNullException("AttachId param can not be null.");
            _logger.Debug("Params: attachId = {0};", attachId);
            
            OraCommand command = new OraCommand("REQ_PKG.SEL_REQ_ATT_BLOB");
            command.CommandType = CommandType.StoredProcedure;
            OraParamBLOB returnParam = new OraParamBLOB("return", ParameterDirection.ReturnValue, (byte[])null);
            command.AddDBParam(returnParam); 
            command.AddDBParam(new OraParamInt32("p_reqAtt_id", ParameterDirection.Input, Convert.ToInt32(attachId)));
            Execute(command);

            return returnParam.ParamValue;
        }

        public string GetRequestAuditReport(string requestId)
        {
            throw new NotImplementedException();
        }

        public string GetRequestForPeriodReport(DateTime from, DateTime until, long? id_org, long? id_comp, long? id_user, long? id_creator, string tags, string state)
        {
            throw new NotImplementedException();
        }


        #region private

        IDbMgr _dbMgr;
        ILogMgr _logMgr;
        ILogger _logger;
        ICommonService _commonDbService;
        INsiService _nsiService;

        void Execute(OraCommand command)
        {
            _dbMgr.Execute(command);
        }

        void ExecuteWithTransaction(OraCommand command)
        {
            using (DbTransaction transaction = new DbTransaction(_dbMgr))
            {
                _dbMgr.Execute(command);
                transaction.Success = true;
            }
        }

        private RequestEntity ToRequest(DataRow row)
        {
            RequestEntity request = new RequestEntity();
            request.IsPersisit = true;

            request.Id = row["ID"].ToString();
            request.ReqDateTime = Convert.ToDateTime(row["REQ_DATE"]);
            request.CreateDateTime = Convert.ToDateTime(row["CREATE_DATE"]);
            request.Subject = row["SUBJECT"].ToString();
            request.Comments = row["NOTE"].ToString();
            request.Contact = row["CONTACT"].ToString();

            AppEntity application = _nsiService.GetAppById(row["COMP_ID"].ToString());
            if (application != null) request.Application = application;            

            request.TagsString = row["TAGS"].ToString();

            OrgEntity organization = _nsiService.GetOrgById(row["ORG_ID"].ToString());
            if (organization != null) request.Organization = organization;

            UserEntity respUser = _nsiService.GetUserById(row["RESP_ID"].ToString());
            if (respUser != null) request.ResponseUser = respUser;

            UserEntity creator = _nsiService.GetUserById(row["CREATOR_ID"].ToString());
            if (creator != null) request.CreatorUser = creator;

            request.InfoSourceType = (InfoSourceType)Convert.ToInt32(row["REQ_TYPE"]);
            request.State = (RequestState)Convert.ToInt32(row["REQ_STATE"]);
            request.BugNumber = row["BUG_NUM"].ToString();
            request.CMVersion = row["CM_NUM"].ToString();            
            request.ComponentVersion = row["VER_NUM"].ToString();
            request.IsImportant = row["IMPORTANT"].ToString() == "1";

            return request;
        }

        private TagEntity ToTag(DataRow tagRow)
        {
            return new TagEntity(tagRow["ID"].ToString(), tagRow["TAG_NAME"].ToString());
        }

        private AttachEntity ToAttach(DataRow attachRow)
        {
           AttachEntity attach = new AttachEntity(attachRow["ID"].ToString(), attachRow["ATT_NAME"].ToString());
           attach.Blob = SelAttachBlob(attach.Id);
           return attach;
        }

        #endregion private
    }
}
