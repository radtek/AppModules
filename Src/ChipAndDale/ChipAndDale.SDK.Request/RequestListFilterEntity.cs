using System;
using ChipAndDale.SDK.Nsi;
using Core.SDK.Dom;


namespace ChipAndDale.SDK.Request
{
    public class RequestListFilterEntity : EntityBase<RequestListFilterEntity>
    {
        public RequestListFilterEntity() : base("ReqFilter")
        {
            FilterName = "Новий фільтр";
            StartDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-3);
            StopDateTime = null;
            ResponseUser = UserEntity.Create();
            CreatorUser = UserEntity.Create();
            Application = AppEntity.Create();
            Organization = OrgEntity.Create();            
        }

        #region Property

        private string _filterName;        
        public string FilterName
        {
            get { return _filterName; }
            set { _filterName = value; }
        }
        
        private DateTime _startDateTime;        
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }


        private DateTime? _stopDateTime;        
        public DateTime? StopDateTime
        {
            get { return _stopDateTime; }
            set { _stopDateTime = value; }
        }


        private UserEntity _user;        
        public UserEntity ResponseUser
        {
            get { return _user; }
            set { _user = value; }
        }

        private UserEntity _creator;        
        public UserEntity CreatorUser
        {
            get { return _creator; }
            set { _creator = value; }
        }


        private AppEntity _appEntity;        
        public AppEntity Application
        {
            get { return _appEntity; }
            set { _appEntity = value; }
        }


        private OrgEntity _orgEntity;        
        public OrgEntity Organization
        {
            get { return _orgEntity; }
            set { _orgEntity = value; }
        }

        private string _subject;        
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        private string _comments;        
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
                
        private string _statusList;
        public string StatusIdList
        {
            get { return _statusList; }
            set { _statusList = value; }
        }

        private string _tagList;
        public string TagIdList
        {
            get { return _tagList; }
            set { _tagList = value; }
        }

        private string _requestId;
        public string RequestId
        {
            get { return _requestId; }
            set { _requestId = value; }
        }

        #endregion Property


        #region Helper property

        public string StartDateTimeString
        {
            get { return StartDateTime.ToString("dd.MM.yyyy HH:mm"); }            
        }

        public string StopDateTimeString
        {
            get { return StopDateTime.HasValue? StopDateTime.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty; }
        }        

        public string OrganizationIdtString
        {
            get { return Organization.IsNewEntity ? string.Empty : Organization.Id; }
        }

        public string ApplicationIdtString
        {
            get { return Application.IsNewEntity ? string.Empty : Application.Id; }
        }

        public string UserIdtString
        {
            get { return ResponseUser.IsNewEntity ? string.Empty : ResponseUser.Id; }
        }

        public string CreatorIdtString
        {
            get { return CreatorUser.IsNewEntity ? string.Empty : CreatorUser.Id; }
        }        

        #endregion Helper property


        #region Object override

        public override bool Equals(object obj)
        {
            RequestListFilterEntity requestFilter = obj as RequestListFilterEntity;
            return Equals(requestFilter);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return FilterName;
        }

        #endregion Object override


        #region IDeepClonable

        public override RequestListFilterEntity Clone()
        {
            RequestListFilterEntity result = new RequestListFilterEntity();
            result.Id = Id;
            result.CloneKey = CloneKey.Clone();
            result.FilterName = FilterName;
            result.StartDateTime = StartDateTime;
            result.StopDateTime = StopDateTime;
            result.Organization = Organization.Clone();
            result.ResponseUser = ResponseUser.Clone();
            result.CreatorUser = CreatorUser.Clone();
            result.Application = Application.Clone();
            result.StatusIdList = StatusIdList;
            result.TagIdList = TagIdList;
            result.Subject = Subject;
            result.Contact = Contact;
            result.Comments = Comments;
            result.RequestId = RequestId;
            return result;
        }

        #endregion IDeepClonable


        #region IFullEquatable

        public override bool FullEquals(RequestListFilterEntity other)
        {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return other != null
                && StartDateTime == other.StartDateTime
                && Nullable.Equals(StopDateTime, other.StopDateTime)
                && UserEntity.Equals(ResponseUser, other.ResponseUser)
                && UserEntity.Equals(CreatorUser, other.CreatorUser)
                && AppEntity.Equals(Application, other.Application)
                && OrgEntity.Equals(Organization, other.Organization)
                && string.Equals(StatusIdList, other.StatusIdList)
                && string.Equals(TagIdList, other.TagIdList)
                && string.Equals(Subject, other.Subject)
                && string.Equals(Comments, other.Comments)
                && string.Equals(Contact, other.Contact)
                && string.Equals(RequestId, other.RequestId);
        }

        #endregion IFullEquatable


        #region IEquatable

        public override bool Equals(RequestListFilterEntity other)
        {
            if (other == null) return false;
            else return string.Equals(CloneKey, other.CloneKey);
        }

        #endregion IEquatable


        public static RequestListFilterEntity Create()
        {
            return new RequestListFilterEntity();
        }       
    }
}
