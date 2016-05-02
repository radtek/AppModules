using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.Common
{
     public class IdentKey : IEquatable<IdentKey>
    {
        private readonly Guid _id;
       
        public IdentKey()
        {
            _id = Guid.NewGuid();
        }

        public IdentKey(Guid guid)
        {
            _id = guid;
        }
      
        public bool Equals(IdentKey other)
        {
            if (other == null) return false;
            return Equals(_id, other._id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as IdentKey);
        }
       
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override string ToString()
        {
            return _id.ToString();
        }

        public IdentKey Clone()
        {
            return new IdentKey(_id);
        }

         public Guid ToGuid()
         {
             return _id;
         }
    }
}
