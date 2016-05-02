using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.SDK.Interface;

namespace Core.SDK.Dom
{
    public class EntityList<T> : IEntityList<T>, IDeepClonable<EntityList<T>> where T : EntityBase<T>
    {
        public EntityList()
        {
            _Entitys = new BindingCollection<T>();            
        }

        public EntityList(BindingCollection<T> list)
        {
            if (list == null) _Entitys = new BindingCollection<T>();
            else _Entitys = list;
            if (_Entitys.Count > 0) Current = _Entitys[0];
        }

        public EntityList(List<T> list)
        {
            if (list == null) _Entitys = new BindingCollection<T>();
            else _Entitys = new BindingCollection<T>(list);
            if (_Entitys.Count > 0) Current = _Entitys[0];
        }

        BindingSource _BindingSource;
        public void SetBindingSource(BindingSource binding)
        {
            Unsubscribe();
            if (binding != null) Subscribe(binding);
        }

        BindingCollection<T> _Entitys;
        public BindingCollection<T> Entities
        {
            get { return _Entitys; }
        }

        public EntityList<T> Clone()
        {
            BindingCollection<T> bindingListView = new BindingCollection<T>();
            if (_Entitys != null)
            {
                foreach (T entity in _Entitys)
                    bindingListView.Add(entity.Clone());
            }
            EntityList<T> entityList = new EntityList<T>(bindingListView);
            if (Current != null) entityList.Current = Current.Clone();
            entityList.Caption = Caption;                     
            return entityList;
        }

        public void Fill(BindingCollection<T> newList)
        {
            if (newList == null) _Entitys = new BindingCollection<T>();
            else _Entitys = newList;
            if (_Entitys.Count == 0 || _Entitys.IndexOf(Current) == -1) Current = null;
            else Current = Current;

            if (_BindingSource != null)
            {
                _BindingSource.DataSource = Entities;
                int pos = _Entitys.IndexOf(Current);
                _BindingSource.Position = pos;
            }
        }

        public void MoveBindingSourceTo(EntityList<T> entity)
        {
            if (entity == null) return;

            int pos = _Entitys.IndexOf(Current);
            BindingSource source = _BindingSource;
            SetBindingSource(null);
            entity.SetBindingSource(source);
            if (pos != -1) source.Position = pos;
        }

        T _Current;
        public T Current
        {
            get
            {
                return _Current;
            }
            set
            {
                int pos = -1;
                if (value != null)
                {
                    pos = _Entitys.IndexOf(value);
                    if (pos == -1) return;
                    _Entitys[pos] = value;
                }                
                _Current = value;                
                if (_BindingSource != null) _BindingSource.Position = pos;
            }
        }        

        string _caption;
        public string Caption 
        {
            get { return _caption; } 
            set { _caption = value; } 
        }


        #region static memebers

        public static bool DiffTwoList<TEntity>(EntityList<TEntity> before, EntityList<TEntity> after, out BindingCollection<TEntity> add, out BindingCollection<TEntity> upd, out BindingCollection<TEntity> del) where TEntity : EntityBase<TEntity>
        {
            add = new BindingCollection<TEntity>();
            upd = new BindingCollection<TEntity>();
            del = new BindingCollection<TEntity>();

            if ((before == null) && (after != null))
            {
                add = after.Entities;                
            }
            else if ((before != null) && (after == null))
            {
                del = before.Entities;
            }
            else if ((before != null) && (after != null))
            {
                del = new BindingCollection<TEntity>(before.Entities.Except(after.Entities, new EntityIdComparer<TEntity>()).ToList<TEntity>());

                foreach (TEntity entity in after.Entities.Except(before.Entities, new EntityFullComparer<TEntity>()))
                {
                    if (entity.IsNewEntity) add.Add(entity);
                    else upd.Add(entity);
                }                
            }
            return (add.Count == 0 && upd.Count == 0 && del.Count == 0);            
        }

        public static BindingCollection<TEntity> DiffTwoListAndGetNew<TEntity>(EntityList<TEntity> before, EntityList<TEntity> after) where TEntity : EntityBase<TEntity>
        {
            BindingCollection<TEntity> result = new BindingCollection<TEntity>();

            if ((before == null) && (after != null))
                result = after.Entities;                        
            else if ((before != null) && (after != null))
                foreach (TEntity entity in after.Entities.Except(before.Entities, new EntityFullComparer<TEntity>()))
                    if (entity.IsNewEntity) result.Add(entity);                                     
            
            return result;
        }

        public static BindingCollection<TEntity> DiffTwoListAndGetUpd<TEntity>(EntityList<TEntity> before, EntityList<TEntity> after) where TEntity : EntityBase<TEntity>
        {
            BindingCollection<TEntity> result = new BindingCollection<TEntity>();

            if ((before != null) && (after != null))
                foreach (TEntity entity in after.Entities.Except(before.Entities, new EntityFullComparer<TEntity>()))
                    if (!entity.IsNewEntity) result.Add(entity);
            return result;
        }

        public static BindingCollection<TEntity> DiffTwoListAndGetDel<TEntity>(EntityList<TEntity> before, EntityList<TEntity> after) where TEntity : EntityBase<TEntity>
        {
            BindingCollection<TEntity> result = new BindingCollection<TEntity>();

            if ((before != null) && (after == null))
                result = before.Entities;            
            else if ((before != null) && (after != null))
                result = new BindingCollection<TEntity>(before.Entities.Except(after.Entities, new EntityIdComparer<TEntity>()).ToList<TEntity>());

            return result;
        }

        public static bool Equal<TEntity>(EntityList<TEntity> list1, EntityList<TEntity> list2) where TEntity : EntityBase<TEntity>
        {
            if (object.Equals(list1, list2)) return true;

            if (list1 != null && 
                list2 != null && 
                list1.Entities.Except(list2.Entities).Count() == 0 &&
                list2.Entities.Except(list1.Entities).Count() == 0) return true;
            
            return false;
        }

        #endregion static memebers


        #region private
        private void Subscribe(BindingSource binding)
        {
            _BindingSource = binding;
            _BindingSource.DataSource = Entities;
            int pos = _Entitys.IndexOf(Current);
            _BindingSource.Position = pos;
            _BindingSource.PositionChanged += new EventHandler(_BindingSource_PositionChanged);
            _BindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(_BindingSource_ListChanged);
        }

        void _BindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            _Current = (T)_BindingSource.Current;
        }

        void _BindingSource_PositionChanged(object sender, EventArgs e)
        {
            _Current = (T)_BindingSource.Current;
        }

        private void Unsubscribe()
        {
            if (_BindingSource == null) return;

            _BindingSource.PositionChanged -= new EventHandler(_BindingSource_PositionChanged);
            _BindingSource.ListChanged -= new System.ComponentModel.ListChangedEventHandler(_BindingSource_ListChanged);
            _BindingSource.DataSource = null;
            _BindingSource = null;
        }
        #endregion private        
    }
}
