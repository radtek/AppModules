using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.UI;
using Core.SDK.Composite.Common;
using DevExpress.XtraBars;

namespace Core.XtraCompositeModule
{
    public class MainMenuCommandRegionDecorator : ICommandRegion
    {
        public MainMenuCommandRegionDecorator(string regionName, BarCommandRegion barCommandRegion)
        {
            if (string.IsNullOrEmpty(regionName)) throw new ArgumentNullException("RegionName can not be null or empty.");
            if (barCommandRegion == null) throw new ArgumentNullException("BarCommandRegion can not be null.");            

            _regionName = regionName;
            _barCommandRegion = barCommandRegion;            
            _identKeyCash = new List<IdentKey>();
            _parentItemNameCash = new List<string>();
        }

        public IdentKey AddCommand(ICommand view)
        {
            IdentKey ident = _barCommandRegion.AddCommand(view);
            AddToCash(view, ident);
            return ident;
        }        

        public IdentKey AddCommand(ICommand view, string parentItem)
        {
            IsBelongToMainMenu(parentItem);
            IdentKey ident = _barCommandRegion.AddCommand(view, parentItem);
            AddToCash(view, ident);
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
        List<IdentKey> _identKeyCash;
        List<string> _parentItemNameCash;

        private void IsBelongToMainMenu(string parentItem)
        {
            if (_parentItemNameCash.IndexOf(parentItem) == -1)
                throw new InvalidOperationException("Command with such key is not found or not belong to this region.");
        }

        private void AddToCash(ICommand view, IdentKey key)
        {
            if (view.CommandType == CommandType.Group) _parentItemNameCash.Add(view.Name);
            _identKeyCash.Add(key);
        }

        #endregion private
    }
}
