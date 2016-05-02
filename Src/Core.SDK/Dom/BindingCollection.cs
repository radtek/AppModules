using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Core.SDK.Dom
{
    [Serializable]
    public class BindingCollection<T> : BindingList<T>, IBindingListView
    {
        public BindingCollection() : base() { }

        public BindingCollection(IList<T> list)
            : base(list)
        {
            IBindingList asBindingList = list as IBindingList;
            if (asBindingList != null)
                asBindingList.ListChanged += SourceListChanged;
        }

        public BindingCollection(BindingCollection<T> list)
            : base(list)
        {
            IBindingList asBindingList = list as IBindingList;
            if (asBindingList != null)
                asBindingList.ListChanged += SourceListChanged;
        }

        private bool IsSorting = false;

        private bool _IsEnableSorting = true;

        public bool IsEnableSorting
        {
            get { return _IsEnableSorting; }
            set { _IsEnableSorting = value; }
        }

        void SourceListChanged(object sender, ListChangedEventArgs e)
        {
            if (!IsSorting)
                ClearSortProperties();
            OnListChanged(e);
        }

        [NonSerialized]
        private PropertyDescriptor _SortProperty;

        [NonSerialized]
        private ListSortDirection _SortDirection;

        [NonSerialized]
        private ListSortDescriptionCollection _SortDescriptions;

        public ListSortDescriptionCollection SortDescriptions
        {
            get { return _SortDescriptions; }
        }

        [NonSerialized]
        private bool _isSorted;

        private void ApplySort(System.Collections.Generic.IComparer<T> comparer)
        {
            T[] items = this.ToArray();
            Array.Sort(items, comparer);

            try
            {
                IsSorting = true;
                this.Clear();
                Array.ForEach(items, this.Add);
                
            }
            finally
            {
                IsSorting = false;
                _isSorted = true;
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            this.Items.CopyTo(array, 0);
            return array;
        }

        protected override void ApplySortCore(
            PropertyDescriptor property,
            ListSortDirection direction
            )
        {
            _SortProperty = property;
            _SortDirection = direction;

            _SortDescriptions = null;

            ApplySort(new PropertyComparer(property, direction));

            if (Count > 0)
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        public void ResortList()
        {
            if (_SortProperty != null)
                ApplySortCore(_SortProperty, _SortDirection);
        }

        protected override void RemoveSortCore()
        {
            ClearSortProperties();
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        private void ClearSortProperties()
        {
            _isSorted = false;
            _SortProperty = null;
            _SortDescriptions = null;
        }

        protected override bool SupportsSortingCore
        {
            get { return IsEnableSorting; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return _SortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _SortProperty; }
        }

        protected override bool IsSortedCore
        {
            get { return _isSorted; }
        }

        #region SortPropertyComparer

        public class PropertyComparer : System.Collections.Generic.IComparer<T>
        {
            private PropertyDescriptor _property;
            private ListSortDirection _direction;

            public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
            {
                _property = property;
                _direction = direction;
            }

            #region IComparer

            public int Compare(T xWord, T yWord)
            {
                object xValue = GetPropertyValue(xWord, _property.Name);
                object yValue = GetPropertyValue(yWord, _property.Name);

                if (_direction == ListSortDirection.Ascending)
                {
                    return CompareAscending(xValue, yValue);
                }
                else
                {
                    return CompareDescending(xValue, yValue);
                }
            }

            public bool Equals(T xWord, T yWord)
            {
                return xWord.Equals(yWord);
            }

            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }

            #endregion

            private int CompareAscending(object xValue, object yValue)
            {
                int result;

                if ((xValue == null) && (yValue != null)) result = -1;
                else if ((xValue != null) && (yValue == null)) result = 1;
                else if ((xValue == null) && (yValue == null)) result = 0;
                else
                    if (xValue is IComparable)
                    {
                        result = ((IComparable)xValue).CompareTo(yValue);
                    }
                    else if (xValue.Equals(yValue))
                    {
                        result = 0;
                    }
                    else result = xValue.ToString().CompareTo(yValue.ToString());

                return result;
            }

            private int CompareDescending(object xValue, object yValue)
            {
                return CompareAscending(xValue, yValue) * -1;
            }

            private object GetPropertyValue(T value, string property)
            {
                PropertyInfo propertyInfo = value.GetType().GetProperty(property);
                return propertyInfo.GetValue(value, null);
            }
        }

        /*class SortPropertyComparer : IComparer
        {
            readonly PropertyDescriptor _property;
            readonly ListSortDirection _direction;

            public SortPropertyComparer(PropertyDescriptor property, ListSortDirection direction)
            {
                _property = property;
                _direction = direction;
            }

            public int Compare(object x, object y)
            {
                object a = _property.GetValue(x);
                object b = _property.GetValue(y);
                int n;

                if ((a as IComparer) != null || (b as IComparer) != null)
                {
                    n = Comparer.Default.Compare(a, b);
                }
                else
                {
                    string aa = a == null ? "" : a.ToString();
                    string bb = b == null ? "" : b.ToString();
                    n = StringComparer.CurrentCulture.Compare(aa, bb);
                }

                return _direction == ListSortDirection.Ascending ? n : -n;
            }
        }*/
        /*==========================================*/
        #endregion


        #region IBindingListView Members

        public bool SupportsAdvancedSorting
        {
            get { return false; }
        }

        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private string _Filter;

        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        public void RemoveFilter()
        {
            _Filter = null;
        }

        private bool _IsEnableFiltering = true;

        public bool IsEnableFiltering
        {
            get { return _IsEnableFiltering; }
            set { _IsEnableFiltering = value; }
        }

        public bool SupportsFiltering
        {
            get { return IsEnableFiltering; }
        }

        #endregion
    }
}
