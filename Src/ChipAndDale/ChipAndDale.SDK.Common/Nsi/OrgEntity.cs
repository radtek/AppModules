using System.ComponentModel;
using System.Text;
using Core.SDK.Dom;

namespace ChipAndDale.SDK.Nsi
{
    public class OrgEntity : EntityBase<OrgEntity>, IDataErrorInfo
    {
        public OrgEntity(): base()
        { }

        public OrgEntity(string id, string name)
            : this()
        {
            Id = id;           
            Name = name;
        }

        #region Property

        private string _name;        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }

        #endregion Property


        #region IDeepClonable

        public override OrgEntity Clone()
        {
            OrgEntity org = new OrgEntity(Id, Name);            
            org.Phone = Phone;
            org.Info = Info;
            return org;
        }

        #endregion IDeepClonable


        #region IFullEquatable

        public override bool FullEquals(OrgEntity other)
        {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return (string.Equals(Id, other.Id) &&                     
                     string.Equals(Name, other.Name) &&
                     string.Equals(Phone, other.Phone) &&
                     string.Equals(Info, other.Info));
        }

        #endregion IFullEquatable


        #region Object override

        public override bool Equals(object obj)
        {
            OrgEntity org = obj as OrgEntity;
            if (org == null) return false;
            else return base.Equals(org);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion


        #region IDataErrorInfo Members
        string IDataErrorInfo.Error
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.Append((this as IDataErrorInfo)["Name"]);                
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
                    case "Name":
                        {
                            if (string.IsNullOrEmpty(Name))
                                result = "Поле 'Назва організації' повинно бути заповнено.";
                            else if (Name.Length > 50)
                                result = "Поле 'Назва організації' не може бути більше 50 символів.";
                            break;
                        }
                    default:
                        break;
                }
                return result;
            }
        }
        #endregion IDataErrorInfo Members


        public static OrgEntity Create()
        {
            return new OrgEntity();
        }
    }
}
