using System;
using System.ComponentModel;
using System.Text;
using Core.SDK.Dom;

namespace ChipAndDale.SDK.Nsi
{
    public class UserEntity : EntityBase<UserEntity>, IDataErrorInfo
    {
        public UserEntity(): base()
        {
            Organization = OrgEntity.Create();
        }              

        public UserEntity(string id, string login, string name) : this()
        {
            Id = id;
            Login = login;
            Name = name;            
        }

        #region Property

        private string _login;                
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

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

        private DateTime _birthDay;        
        public DateTime BirthDay
        {
            get { return _birthDay; }
            set { _birthDay = value; }
        }

        OrgEntity _org;
        public OrgEntity Organization
        {
            get { return _org; }
            set { _org = value; }
        }

        #endregion Property


        #region IDeepClonable

        public override UserEntity Clone()
        {
            UserEntity user = new UserEntity(Id, Login, Name);
            user.BirthDay = BirthDay;
            user.Phone = Phone;
            user.Info = Info;
            return user;
        }

        #endregion IDeepClonable


        #region IFullEquatable

        public override bool FullEquals(UserEntity other)
        {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return (string.Equals(Id, other.Id) &&
                     string.Equals(Login, other.Login) &&
                     string.Equals(Name, other.Name) &&
                     string.Equals(Phone, other.Phone) &&
                     string.Equals(Info, other.Info) &&
                     BirthDay == other.BirthDay);
        }

        #endregion IFullEquatable
       

        #region Object override

        public override bool Equals(object obj)
        {
            UserEntity user = obj as UserEntity;
            if (user == null) return false;
            else return base.Equals(user);
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
                result.Append((this as IDataErrorInfo)["Login"]);
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
                    case "Login":
                        {
                            if (string.IsNullOrEmpty(Login))
                                result = "Поле 'Логін користувача' повинно бути заповнено.";
                            else if (Login.Length > 20)
                                result = "Поле 'Логін користувача' не може бути більше 20 символів.";
                            break;
                        }
                    case "Name":
                        {
                            if (string.IsNullOrEmpty(Name))
                                result = "Пол 'Повне ім'я користувача' повинно бути заповнено.";
                            else if (Name.Length > 50)
                                result = "Поле 'Повне ім'я користувача' не може бути більше 50 символів.";
                            break;
                        }
                    default:
                        break;
                }
                return result;
            }
        }
        #endregion IDataErrorInfo Members


        public static UserEntity Create()
        {
            return new UserEntity();
        }
    }
}
