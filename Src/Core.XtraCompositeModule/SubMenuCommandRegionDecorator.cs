using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.UI;
using DevExpress.XtraBars;
using Core.SDK.Composite.Common;

namespace Core.XtraCompositeModule
{
    public class SubMenuCommandRegionDecorator : ICommandRegion
    {
        public SubMenuCommandRegionDecorator(string regionName, BarCommandRegion barCommandRegion, BarSubItem subMenu)
        {
            if (string.IsNullOrEmpty(regionName)) throw new ArgumentNullException("RegionName can not be null or empty.");
            if (barCommandRegion == null) throw new ArgumentNullException("BarCommandRegion can not be null.");
            if (subMenu == null) throw new ArgumentNullException("CommandRegion can not be null.");

            _regionName = regionName;
            _barCommandRegion = barCommandRegion;
            _subMenu = subMenu;
            _identKeyCash = new List<IdentKey>();
        }

        public IdentKey AddCommand(ICommand view)
        {
            IdentKey ident = _barCommandRegion.AddCommand(view, _subMenu.Name);
            _identKeyCash.Add(ident);
            return ident;
        }

        public IdentKey AddCommand(ICommand view, string parentItem)
        {
            IsBelongToSubItem(parentItem);
            IdentKey ident = _barCommandRegion.AddCommand(view, parentItem);
            _identKeyCash.Add(ident);
            return ident;            
        }        

        public void RemoveCommand(IdentKey key)
        {
            if (_identKeyCash.IndexOf(key) != -1)
            {
                _barCommandRegion.RemoveCommand(key);
                _identKeyCash.Remove(key);
            }
            else throw new InvalidOperationException("Command with such key is not found.");
        }

        public void RefreshCommand(IdentKey key)
        {
            if (_identKeyCash.IndexOf(key) != -1) _barCommandRegion.RefreshCommand(key);
            else throw new InvalidOperationException("Command with such key is not found.");
        }

        public string Name
        {
            get { return _regionName; }
        }

        #region private

        string _regionName;
        BarCommandRegion _barCommandRegion;
        BarSubItem _subMenu;
        List<IdentKey> _identKeyCash;

        private void IsBelongToSubItem(string parentItem)
        {
            /*BarSubItem item = _barCommandRegion.FindItemByName(parentItem);

            if (_subMenu.ContainsItem
            while (item != null)
            { 
                if (string.Equals(item.Name, _subMenu.Name)) return;
                item = _barCommandRegion.FindItemByName(item.Name);
            }
            throw new InvalidOperationException("Command with such key is not found or not belong to this region.");*/ 
        }

        /*iprivate bool IsChaild(BarSubItem parentItem, BarSubItem childItem)
        {
            f (parentItem.ContainsItem(childItem) return true;
            else
            {
                foreach (BarSubItem item in parentItem.item)
            }
        }*/

        #endregion private
    }
}
