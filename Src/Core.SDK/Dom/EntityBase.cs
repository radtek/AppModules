using System;
using System.ComponentModel;
using Core.SDK.Composite.Common;
using Core.SDK.Interface;

namespace Core.SDK.Dom
{
    public abstract class EntityBase<T> : 
        IEntity<T>, 
        IDeepClonable<T>, 
        IEquatable<T>,
        IFullEquatable<T>,
        INotifyPropertyChanged  where T : EntityBase<T>
    {
        public EntityBase() : this("Entity")
        {}

        public EntityBase(string name)
        {
            _key = new IdentKey();
            _cloneKey = _key;
            _id = _defaultId;
            _iPersisit = false;
            _internalName = string.Format("{0}_{1}", name, _key.ToString());
        }  
                
        #region property

        IdentKey _key;
        [Browsable(false)]
        public IdentKey IdentKey
        {
            get { return _key; }
        }

        readonly string _defaultId = "#(1]";
        string _id;        
        public string Id
        {
            get { return IsNewEntity? string.Empty : _id; }
            set 
            {
                if (string.IsNullOrEmpty(value)) _id = _defaultId;
                else _id = value; 
            }
        }

        bool _iPersisit; // true - saved in store, got and not edited entity
        public bool IsPersisit
        {
            get { return _iPersisit; }
            set { _iPersisit = value; }
        }

        public bool IsNewEntity
        {
            get { return string.Equals(_id, _defaultId); }
        }

        public int? NumId
        {
            get { return IsNewEntity ? null : new Nullable<int>(Convert.ToInt32(Id)); }
        }

        IdentKey _cloneKey; // идентификатор , неизменяемый при клонировании
        public IdentKey CloneKey
        {
            get { return _cloneKey; }
            set { _cloneKey = value; }
        }

        string _internalName;
        public string InternalName
        {
            get { return _internalName; }
        }       

        #endregion property


        #region IDeepClonable

        public abstract T Clone();

        #endregion IDeepClonable


        #region IEquatable

        public virtual bool Equals(T other)
        {
            return EqualsById(other);
        }

        #endregion IEquatable        


        #region IFullEquatable

        public virtual bool FullEquals(T other)
        {
            return EqualsById(other);
        }

        #endregion IFullEquatable        


        #region IEntity

        public virtual string ToInternalString()
        {
            return string.Format("Entity {0}. Id = {1}", GetType().Name, Id);
        }

        #endregion IEntity        


        #region INotifyPropertyChanged

        event PropertyChangedEventHandler _propertyChangedEvent;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChangedEvent += value;
            }
            remove
            {
                _propertyChangedEvent -= value;
            }            
        }

        protected bool SetPropertyValue<TProperty>(string propertyName, ref TProperty oldValue, ref TProperty newValue)
        {
            return SetPropertyValue<TProperty>(propertyName, ref oldValue, ref newValue, true);
        }

        protected bool SetPropertyValue<TProperty>(string propertyName, ref TProperty oldValue, ref TProperty newValue, bool checkEquals)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if ((oldValue == null && newValue != null) || !oldValue.Equals((TProperty)newValue) || !checkEquals)
            {
                oldValue = newValue;
                FirePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void FirePropertyChanged(string propertyName)
        {
            if (this._propertyChangedEvent != null)
            {
                this._propertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged


        #region others func

        public bool EqualsById(T entity)
        {
            if (entity == null) return false;
            else if (IsNewEntity && entity.IsNewEntity && IdentKey == entity.IdentKey) return true;
            else return string.Equals(Id, entity.Id);
        }

        public void MarkAsNew()
        {
            _id = _defaultId;
        }

        public bool IsMyClone(T entity)
        {
            if (entity == null) return false;
            else return (CloneKey.Equals(entity.CloneKey));
        }

        public void ResetCloneKey()
        {
            _cloneKey = new IdentKey();
        }

        #endregion others func
    }
}
