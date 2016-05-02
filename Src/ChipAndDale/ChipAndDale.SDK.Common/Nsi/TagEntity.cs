using System.ComponentModel;
using System.Text;
using Core.SDK.Dom;

namespace ChipAndDale.SDK.Nsi
{    
    public class TagEntity : EntityBase<TagEntity>, IDataErrorInfo
    {
        public TagEntity(): base()
        { }

        public TagEntity(string id, string name)
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
        
        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }

        #endregion Property


        #region IDeepClonable

        public override TagEntity Clone()
        {
            TagEntity tag = new TagEntity(Id, Name);                        
            tag.Info = Info;
            return tag;
        }

        #endregion IDeepClonable


        #region IFullEquatable

        public override bool FullEquals(TagEntity other)
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
            TagEntity tag = obj as TagEntity;
            if (tag == null) return false;
            else return base.Equals(tag);
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
                                result = "Поле 'Назва додатку' повинно бути заповнено.";
                            else if (Name.Length > 50)
                                result = "Поле 'Назва додатку' не може бути більше 50 символів.";
                            break;
                        }
                    default:
                        break;
                }
                return result;
            }
        }
        #endregion IDataErrorInfo Members


        public static TagEntity Create()
        {
            return new TagEntity();
        }
    }
}
