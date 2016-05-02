using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.UI;
using DevExpress.XtraBars;
using Core.SDK.Composite.Common;
using System.Drawing;

namespace Core.XtraCompositeModule
{
    public class BarCommandRegion: ICommandRegion
    {
        public BarCommandRegion(string regionName, Bar rootBar, int lastIndex)
        {
            if (string.IsNullOrEmpty(regionName)) throw new ArgumentNullException("RegionName can not be null or empty.");
            if (rootBar.Manager == null) throw new ArgumentNullException("RootBar must be added to BarManager.");
            if (rootBar == null) throw new ArgumentNullException("RootBar can not be null.");

            _regionName = regionName;
            _barManager = rootBar.Manager;
            _rootBar = rootBar;
            _Commands = new Dictionary<IdentKey, CommandData>();
            LastIndex = lastIndex;
        }               

        public IdentKey AddCommand(ICommand command)
        {
            return CreateMenuAndSubscribe(command, _rootBar.BarName);
        }

        public IdentKey AddCommand(ICommand command, string parentItem)
        {
            if (string.IsNullOrEmpty(parentItem))
                throw new ArgumentException("ParentItem can not be null.");
            return CreateMenuAndSubscribe(command, parentItem);
        }
        
        public void RemoveCommand(IdentKey key)
        {            
            UnSubscribeAndRemoveMenu(key);
        }

        public void RefreshCommand(IdentKey key)
        { 
            CommandData data = null;
            if (_Commands.TryGetValue(key, out data))
            {
                data.Invoker.UpdateUI();
            }
            else throw new InvalidOperationException("Command with such key not found.");
        }

        public string Name
        {
            get { return _regionName; }
        }

        
        #region private

        string _regionName;
        Bar _rootBar;
        BarManager _barManager;
        Dictionary<IdentKey, CommandData> _Commands;
        int _lastIndex;

        protected int LastIndex
        {
            get { return _lastIndex; }
            set { _lastIndex = value; }
        }

        class CommandData
        {
            public CommandData(BarItemInvoker invoker, BarSubItem parent)
            {
                Invoker = invoker;
                ParentItem = parent;
            }

            public BarItemInvoker Invoker { get; set; }
            public BarSubItem ParentItem { get; set; }
            
        }

        private IdentKey CreateMenuAndSubscribe(ICommand command, string parent)
        {
            BarSubItem parentItem = null;
            if (!string.Equals(parent, _rootBar.BarName))
            {
                parentItem = FindItemByName(parent);
                if (parentItem == null)
                    throw new InvalidOperationException("Parent item not find.");
            }

            BarItem item = null;
            BarItemLink link = null;
            BarItemInvoker invoker = null;
            IdentKey key = new IdentKey();
            int lastValidIndex = LastIndex;
            try
            {
                item = BarItemFactory.CreateItem(command.CommandType);
                item.Tag = key;
                _barManager.Items.Add(item);
                item.Glyph = command.Image;
                item.Appearance.BackColor = Color.Transparent;
                
                if (parentItem == null)
                {
                    link = _rootBar.ItemLinks.Insert(LastIndex, item);                    
                    LastIndex++;
                }
                else
                    link = parentItem.ItemLinks.Insert(parentItem.ItemLinks.Count, item);

                invoker = BarItemInvokerFactory.CreateItemInvoker(link, command);
                _Commands.Add(key, new CommandData(invoker, parentItem));
                 
                return key;
            }
            catch (Exception ex)
            {
                ClearResource(parentItem, item, link, invoker, key, lastValidIndex);
                throw ex;
            }
        }

        private void ClearResource(BarSubItem parentItem, BarItem item, BarItemLink link, BarItemInvoker invoker, IdentKey key, int lastValidIndex)
        {
            LastIndex = lastValidIndex;
            try { _Commands.Remove(key); }
            catch { }
            try { if (invoker != null) { invoker.Dispose(); } }
            catch { }
            try
            {
                if (link != null)
                {
                    if (parentItem == null) _rootBar.ItemLinks.Remove(link);
                    else parentItem.ItemLinks.Remove(link);
                }
            }
            catch { }
            try { if (item != null) { _barManager.Items.Remove(item); } }
            catch { }
        }

        public BarSubItem FindItemByName(string parent)
        {
            foreach (BarItem item in _barManager.Items)
            {
                if (string.Equals(item.Name, parent))
                {
                    BarSubItem subMenu = item as BarSubItem;
                    if (subMenu != null) return subMenu;
                }
            }
            return null;
        }

        private void UnSubscribeAndRemoveMenu(IdentKey key)
        {
            if (key == null) throw new ArgumentNullException("Key can not be null.");

            CommandData data = null;
            if (!_Commands.TryGetValue(key, out data)) throw new InvalidOperationException("Command with such key is not found.");
            
            ICommand command = data.Invoker.Command;            
            BarItemInvoker invoker = data.Invoker;
            BarSubItem parentItem = data.ParentItem;
            BarItemLink link = invoker.BarItemLink;
            BarItem item = link.Item;
            if (command.CommandType == CommandType.Group)
            {
                BarSubItem groupItem = item as BarSubItem;
                List<BarItemLink> itemLinks = new List<BarItemLink>();
                foreach (BarItemLink itemLink in groupItem.ItemLinks)
                {
                    itemLinks.Add(itemLink);
                }
                foreach (BarItemLink itemLink in itemLinks)
                {
                    BarItem i = itemLink.Item;
                    IdentKey itemKey = i.Tag as IdentKey;
                    if (itemKey != null) UnSubscribeAndRemoveMenu(itemKey);
                }
            }
            ClearResource(parentItem, item, link, invoker, key, parentItem == null ? LastIndex-- : LastIndex);
        }
        #endregion private 
    }
}
