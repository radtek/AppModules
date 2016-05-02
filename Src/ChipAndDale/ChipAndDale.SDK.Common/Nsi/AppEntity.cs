using System.ComponentModel;
using System.Text;
using Core.SDK.Dom;

namespace ChipAndDale.SDK.Nsi
{
    public class AppEntity : EntityBase<AppEntity>, IDataErrorInfo
    {
        public AppEntity(): base()
        { }

        public AppEntity(string id, string name)
            : this()
        {
            Id = id;           
            Name = name;
        }                                    

        #region Property

        private string _name;
        [DisplayName("Назва")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }        

        private string _info;
        [DisplayName("")]
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }

        #endregion Property


        #region IDeepClonable

        public override AppEntity Clone()
        {
            AppEntity app = new AppEntity(Id, Name);           
            app.Info = Info;
            return app;
        }

        #endregion IDeepClonable


        #region IFullEquatable

        public override bool FullEquals(AppEntity other)
        {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return (string.Equals(Id, other.Id) &&                     
                     string.Equals(Name, other.Name) &&                     
                     string.Equals(Info, other.Info));
        }

        #endregion IFullEquatable


        #region Object override

        public override bool Equals(object obj)
        {
            AppEntity app = obj as AppEntity;
            if (app == null) return false;
            else return base.Equals(app);
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


        public static AppEntity Create()
        {
            return new AppEntity();
        }
    }
}
