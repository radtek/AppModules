using System;
using System.ComponentModel;
using System.Text;
using ChipAndDale.SDK.Common;
using ChipAndDale.SDK.Nsi;
using Core.SDK.Dom;
using Core.UtilsModule;
using System.Collections.Generic;

namespace ChipAndDale.SDK.Request
{    
    public class RequestEntity : EntityBase<RequestEntity>, IDataErrorInfo
    {
        public RequestEntity() : base("Request")
        {
            Tags = new EntityList<TagEntity>();
            Attaches = new EntityList<AttachEntity>();
            InfoSourceType = InfoSourceType.Call;
            State = RequestState.Open;
            Organization = OrgEntity.Create();
            Application = AppEntity.Create();
            ResponseUser = UserEntity.Create();
            CreatorUser = UserEntity.Create();
            CreateDateTime = DateHlp.GetDateTime_hhmmss();
            ReqDateTime = DateHlp.GetDateTime_hhmmss();

            PropertyChanged += new PropertyChangedEventHandler(RequestEntity_PropertyChanged);
        }

        void RequestEntity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ReqDateTime" || e.PropertyName == "Subject" || e.PropertyName == "Organization" || e.PropertyName == "Application")
                FirePropertyChanged("RequestBrief");
        }

        #region Property

        private DateTime _ReqDateTime;
        [DisplayName("Час звернення")]
        public DateTime ReqDateTime
        {
            get { return _ReqDateTime; }
            set { SetPropertyValue<DateTime>("ReqDateTime", ref _ReqDateTime, ref value); }
        }

        private DateTime _CreateDateTime;
        [DisplayName("Час вводу звернення")]
        public DateTime CreateDateTime
        {
            get { return _CreateDateTime; }
            set { _CreateDateTime = value; }
        }

        private string _Subject;
        [DisplayName("Зміст")] 
        public string Subject
        {
            get { return _Subject; }
            set { SetPropertyValue<string>("Subject", ref _Subject, ref value); }
        }

        private string _Comments;
        [DisplayName("Коментар (накопичувальний)")]  
        public string Comments
        {
            get { return _Comments; }
            set { SetPropertyValue<string>("Comments", ref _Comments, ref value); }
        }

        private string _Contact;
        [DisplayName("Контакти")]                
        public string Contact
        {
            get { return _Contact; }
            set { SetPropertyValue<string>("Contact", ref _Contact, ref value); }
        }

        private OrgEntity _OrgEntity;
        [DisplayName("Назва Організації")]
        public OrgEntity Organization
        {
            get { return _OrgEntity; }
            set { SetPropertyValue<OrgEntity>("Organization", ref _OrgEntity, ref value); }
        }

        private UserEntity _UserEntity;
        [DisplayName("Відповідальна особа")]
        public UserEntity ResponseUser
        {
            get { return _UserEntity; }
            set { SetPropertyValue<UserEntity>("ResponseUser", ref _UserEntity, ref value); }            
        }               

        private InfoSourceType _InfoSourceType;
        [DisplayName("Вид звернення")] 
        public InfoSourceType InfoSourceType
        {
            get { return _InfoSourceType; }
            set { SetPropertyValue<InfoSourceType>("InfoSourceType", ref _InfoSourceType, ref value); }            
        }        
        
        private AppEntity _AppEntity;
        [DisplayName("Назва компоненту")]
        public AppEntity Application
        {
            get { return _AppEntity; }
            set { SetPropertyValue<AppEntity>("Application", ref _AppEntity, ref value); }             
        }

        private string _TagsString;
        [DisplayName("Тегі")]    
        public string TagsString
        {
            get { return _TagsString; }
            set { _TagsString = value; }
        }

        EntityList<TagEntity> _Tags; 
        public EntityList<TagEntity> Tags
        {
            get { return _Tags; }
            set { _Tags = value; }
        }

        public List<string> TagIdList
        {
            get 
            {
                List<string> list = new List<string>();
                foreach (TagEntity tag in Tags.Entities)
                {
                    list.Add(tag.Id);
                }
                return list;
            }            
        }

        EntityList<AttachEntity> _Attaches;   
        public EntityList<AttachEntity> Attaches
        {
            get { return _Attaches; }            
            set { _Attaches = value; }
        }        

        private RequestState _ReqState;
        [DisplayName("Статус")]       
        public RequestState State
        {
            get { return _ReqState; }
            set { SetPropertyValue<RequestState>("State", ref _ReqState, ref value); }            
        }

        private string _BugNumber;
        [DisplayName("№ бага")]    
        public string BugNumber
        {
            get { return _BugNumber; }
            set { SetPropertyValue<string>("BugNumber", ref _BugNumber, ref value); }            
        }

        private string _CMVersion;
        [DisplayName("№ CМ")]
        public string CMVersion
        {
            get { return _CMVersion; }
            set { SetPropertyValue<string>("CMVersion", ref _CMVersion, ref value); }            
        }

        private string _ComponentVersion;
        [DisplayName("№ версии")]   
        public string ComponentVersion
        {
            get { return _ComponentVersion; }
            set { SetPropertyValue<string>("ComponentVersion", ref _ComponentVersion, ref value); }            
        }

        private UserEntity _CreatorUser;
        [DisplayName("Створив")]   
        public UserEntity CreatorUser
        {
            get { return _CreatorUser; }
            set { SetPropertyValue<UserEntity>("CreatorUser", ref _CreatorUser, ref value); }                        
        }

        private bool _IsImportant;
        [DisplayName("Важливе звернення")]  
        public bool IsImportant
        {
            get { return _IsImportant; }
            set { SetPropertyValue<bool>("IsImportant", ref _IsImportant, ref value); }                                    
        }

        public string RequestBrief
        {
            get 
            {
                StringBuilder text = new StringBuilder((IsNewEntity) ? "   Нове звернення" : "   Звернення №" + Id);
                if (!Organization.IsNewEntity) text.Append(" від організації \"" + Organization.Name + "\"");
                if (!Application.IsNewEntity) text.Append(" по компоненту \"" + Application.Name + "\"");
                text.AppendLine( " від " + ReqDateTime.ToString("dd.MM.yyyy"));
                if (!string.IsNullOrEmpty(Subject)) text.AppendLine(Subject);
                return text.ToString();
            }            
        }  

        #endregion Property


        #region Helper property

        public int StateId
        {
            get { return (int)State; }
        }

        public int InfoSourceTypeId
        {
            get { return (int)InfoSourceType; }
        }

        public string IsImportantString
        {
            get { return Hlp.BoolToNumsStr(IsImportant); }
        }

        #endregion helper property        


        #region IDeepClonable

        public override RequestEntity Clone()
        {
            RequestEntity result = new RequestEntity();
            result.CloneKey = CloneKey.Clone();
            result.Id = Id;
            result.ReqDateTime = ReqDateTime;
            result.CreateDateTime = CreateDateTime;
            result.Subject = Subject;
            result.Comments = Comments;
            result.Contact = Contact;
            result.Application = Application.Clone();
            result.TagsString = TagsString;
            result.Tags = Tags.Clone();
            result.Attaches = Attaches.Clone();
            result.Organization = Organization.Clone();
            result.ResponseUser = ResponseUser.Clone();
            result.InfoSourceType = InfoSourceType;
            result.State = State;
            result.BugNumber = BugNumber;
            result.CMVersion = CMVersion;
            result.ComponentVersion = ComponentVersion;
            result.CreatorUser = CreatorUser;
            result.IsImportant = IsImportant;
            return result;
        }

        #endregion IDeepClonable


        #region IFullEquatable

        public override bool FullEquals(RequestEntity other)
        {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return (string.Equals(Id, other.Id)
                    && ReqDateTime == other.ReqDateTime
                    && string.Equals(Subject, other.Subject)
                    && (Comments ?? "") == (other.Comments ?? "")
                    && string.Equals(Contact, other.Contact)
                    && Application.Equals(other.Application)
                    && Organization.Equals(other.Organization)
                    && ResponseUser.Equals(other.ResponseUser)
                    && CreatorUser.Equals(other.CreatorUser)
                    && InfoSourceType == other.InfoSourceType
                    && State == other.State
                    && (BugNumber ?? "") == (other.BugNumber ?? "")
                    && string.Equals(CMVersion, other.CMVersion)
                    && string.Equals(ComponentVersion, other.ComponentVersion)
                    && IsImportant == other.IsImportant);
        }
        #endregion IFullEquatable


        #region IDataErrorInfo Members
        string IDataErrorInfo.Error
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append((this as IDataErrorInfo)["Subject"]);
                /*result.Append((this as IDataErrorInfo)["Contact"]);
                result.Append((this as IDataErrorInfo)["Organization"]);
                result.Append((this as IDataErrorInfo)["Application"]);
                result.Append((this as IDataErrorInfo)["ResponseUser"]);*/
                return result.ToString();
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case "Subject":
                        {
                            if (string.IsNullOrEmpty(Subject))
                                result = "Необхідно ввести зміст звернення.";
                            break;
                        }
                    case "ResponseUser":
                        {
                            if (string.IsNullOrEmpty(ResponseUser.Name))
                                result = "Необхідно вибрати відповідальну особу.";
                            break;
                        }
                    case "Contact":
                        {
                            if (string.IsNullOrEmpty(Contact))
                                result = "Необхідно ввести контактні дані.";
                            break;
                        }
                    case "Organization":
                        {
                            if (string.IsNullOrEmpty(Organization.Name))
                                result = "Необхідно вибрати організацію.";
                            break;
                        }
                    case "Application":
                        {
                            if (string.IsNullOrEmpty(Application.Name))
                                result = "Необхідно вибрати компонент.";
                            break;
                        }                    
                    default:
                        break;
                }
                return result;
            }
        }
        #endregion IDataErrorInfo Members      
  
        
        #region Object override

        public override bool Equals(object obj)
        {
            RequestEntity request = obj as RequestEntity;
            if (request == null) return false;
            else return base.Equals(request);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (IsNewEntity) return "Створення нового звернення";
            else return "Детальна інформація по зверненню №" + Id.ToString();
        }

        #endregion Object override


        public bool IsValid
        {
            get { return string.IsNullOrEmpty((this as IDataErrorInfo).Error); }
        }

        public static RequestEntity Create()
        {
            return new RequestEntity();
        }
    }
}
