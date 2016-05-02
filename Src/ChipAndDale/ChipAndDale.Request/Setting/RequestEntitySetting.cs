using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Request;
using Core.SDK.Composite.Service;
using ChipAndDale.SDK.Nsi;

namespace ChipAndDale.Request.Setting
{
    internal class RequestEntitySetting
    {
        public RequestEntitySetting() { }
        
        #region Properties

        public string Id { get; set; }

        public RequestState State { get; set; }  

        public DateTime CreateDateTime { get; set; }

        public DateTime RequestDateTime { get; set; }

        public string OrganizationId { get; set; }

        public string ApplicationId { get; set; }

        public string ResponseId { get; set; }

        public string CreatorId { get; set; }

        public List<string> TagIdList { get; set; }              

        public string Subject { get; set; }

        public string Contact { get; set; }

        public string Comments { get; set; }

        public InfoSourceType InfoSourceType { get; set; }

        public string BugNumber { get; set; }

        public string CMVersion { get; set; }

        public string ComponentVersion { get; set; }

        public bool IsImportant { get; set; }

        // Temporary hack
        public byte[] AttachBytes1 { get; set; }
        public byte[] AttachBytes2 { get; set; }
        public byte[] AttachBytes3 { get; set; }

        public string AttachName1 { get; set; }
        public string AttachName2 { get; set; }
        public string AttachName3 { get; set; }

        #endregion Properties
    }
}
