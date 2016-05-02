using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChipAndDale.SDK.Request;

namespace ChipAndDale.Request.Setting
{
    internal class FilterEntitySetting
    {
        public FilterEntitySetting()
        { }


        #region Properties

        public string FilterName { get; set; }

        public Guid CloneKey { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? StopDateTime { get; set; }
        
        public string OrganizationId { get; set; }

        public string ApplicationId { get; set; }

        public string ResponseId { get; set; }

        public string CreatorId { get; set; }

        public string TagIdList { get; set; }

        public string StatusIdList { get; set; }

        public string Id { get; set; }

        public string Subject { get; set; }

        public string Contact { get; set; }

        public string Comments { get; set; }

        #endregion Properties

        
        
        internal void InitFrom(RequestListFilterEntity filter)
        { }

        internal RequestListFilterEntity ConvertTo()
        {
            return null;
        }
    }
}
