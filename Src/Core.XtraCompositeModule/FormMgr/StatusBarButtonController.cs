using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars;
using System.Drawing;
using Core.SDK.Composite.Common;
using Core.SDK.Common;
using System.Windows.Forms;

namespace Core.XtraCompositeModule
{
    public delegate void ButtonClickDelegate(IdentKey key, bool isActivate);

    public class StatusBarButtonController
    {
        public StatusBarButtonController(Bar statusBar)
        {
            if (statusBar == null) throw new ArgumentNullException("StatusBar can not be null.");
            if (statusBar.Manager == null) throw new ArgumentNullException("StatusBar must be added to BarManager.");
            _statusBar = statusBar;
            _barManager = _statusBar.Manager;
            _ButtonItems = new Dictionary<IdentKey, BarButtonItem>();
            _isInverse = false;
            _timer = new Timer();
            _timer.Interval = 300;
            _timer.Tick += new EventHandler(OnTimer_Tick);
        }       

        public void AddButton(IdentKey key, string caption, string hint, Image image)
        {
            if (key == null) throw new ArgumentNullException("Key can not be null.");
            if (caption == null) throw new ArgumentNullException("Caption can not be null.");

            BarButtonItem button = new BarButtonItem();
            button.Tag = key;
            button.Caption = caption;
            button.Hint = hint;
            button.Border = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            button.ButtonStyle = BarButtonStyle.Check;
            _barManager.Items.Add(button);
            BarItemLink link = _statusBar.ItemLinks.Insert(_statusBar.ItemLinks.Count, button);
            button.ItemClick += new ItemClickEventHandler(button_ItemClick);            
            _ButtonItems.Add(key, button);
        }             

        public void RemoveButton(IdentKey key)
        {
            BarButtonItem button = GetButtonByKey(key);

            button.ItemClick -= new ItemClickEventHandler(button_ItemClick);
            _barManager.Items.Remove(button);
            _ButtonItems.Remove(key);
        }        

        public void ChangeButtonState(IdentKey key, bool isActive)
        {
            BarButtonItem button = GetButtonByKey(key);
            //bool isActiveExt = _isInverse ? !isActive : isActive;
            if (!isActive) 
                button.Down = false;
            else 
                foreach (KeyValuePair<IdentKey, BarButtonItem> pair in _ButtonItems)
                {
                    pair.Value.Down = (button == pair.Value);
                }
            _isInverse = true;
            _timer.Enabled = true;
            _inverseButtonKey = key;
        }

        public void ClickOnButton(IdentKey key)
        {
            BarButtonItem button = GetButtonByKey(key);
            button.PerformClick();
        }

        public event ButtonClickDelegate ButtonClickEvent;

        
        #region private

        Bar _statusBar;
        BarManager _barManager;
        Dictionary<IdentKey, BarButtonItem> _ButtonItems;
        bool _isInverse;
        IdentKey _inverseButtonKey;
        Timer _timer;

        private BarButtonItem GetButtonByKey(IdentKey key)
        {
            if (key == null) throw new ArgumentNullException("Key can not be null.");

            BarButtonItem button = null;
            if (!_ButtonItems.TryGetValue(key, out button)) throw new InvalidOperationException("StatusBar button with such key not found.");
            return button;
        }

        void button_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarButtonItem button = e.Item as BarButtonItem;
            if (button == null) return;
            OnButtinClick(button);
        }   

        private void OnButtinClick(BarButtonItem button)
        {
            IdentKey key = button.Tag as IdentKey;
            if (key == null) return;
            if (ButtonClickEvent != null)
            {
                if (_isInverse && _inverseButtonKey == key)
                    button.Down = !button.Down;
                ButtonClickEvent(key, button.Down);
            }
        }

        void OnTimer_Tick(object sender, EventArgs e)
        {
            _isInverse = false;
            _timer.Enabled = false;
            _inverseButtonKey = null;
        }

        #endregion private
    }
}
