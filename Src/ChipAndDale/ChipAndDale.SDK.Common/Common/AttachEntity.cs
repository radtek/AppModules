using System;
using System.ComponentModel;
using System.Text;
using Core.SDK.Dom;

namespace ChipAndDale.SDK.Common
{    
    public class AttachEntity : EntityBase<AttachEntity>, IDataErrorInfo
    {
        public AttachEntity(): base()
        { }

        public AttachEntity(string id, string name)
            : this()
        {
            Id = id;           
            Name = name;
        }                                    

        private string _name;        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DateTime _dateCreate;
        public DateTime DateCreate
        {
            get { return _dateCreate; }
            set { _dateCreate = value; }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        private byte[] _blob;
        public byte[] Blob
        {
            get { return _blob; }
            set { _blob = value; }
        }

        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }

        public override AttachEntity Clone()
        {
            AttachEntity attach = new AttachEntity(Id, Name);            
            attach.Info = Info;
            attach.Note = Note;
            attach.DateCreate = DateCreate;
            attach.Blob = Core.UtilsModule.Hlp.CopyByteAray(Blob);
            return attach;
        }

        public override bool Equals(AttachEntity other)
        {
            if (other == null) return false;

            if (object.ReferenceEquals(this, other)) return true;

            return (string.Equals(Id, other.Id) &&                     
                     string.Equals(Name, other.Name) &&
                     string.Equals(Note, other.Note) &&
                     string.Equals(Info, other.Info)) &&
                     DateCreate == other.DateCreate &&
                     Core.UtilsModule.Hlp.EqualsByteAray(Blob, other.Blob);
        }

        public static AttachEntity Create()
        {
            return new AttachEntity();
        }

        #region Object override

        public override bool Equals(object obj)
        {
            AttachEntity attach = obj as AttachEntity;
            if (attach == null) return false;
            else return Equals(attach);
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
    }
}
