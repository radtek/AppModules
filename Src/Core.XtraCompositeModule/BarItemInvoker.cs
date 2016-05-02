using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.UI;
using DevExpress.XtraBars;

namespace Core.XtraCompositeModule
{
    public class BarItemInvoker : CommandInvokerBase
    {
        public BarItemInvoker(BarItemLink itemLink, ICommand command): base(command)
        {
            _barItemLink = itemLink;
            if (!string.IsNullOrEmpty(command.Name)) _barItemLink.Item.Name = command.Name;
            _barItemLink.Item.ItemPress += new ItemClickEventHandler(DoExecute);
            UpdateUI();
        }

        public override void UpdateUI()
        {
            BarItem item = _barItemLink.Item;
            item.Caption = Command.Caption;
            item.Hint = Command.Hint;
            item.Enabled = Command.IsEnabled;
            _barItemLink.Visible = Command.IsVisible;
        }

        public BarItemLink BarItemLink { get { return _barItemLink; } }
        

        public override void Dispose()
        {
            _barItemLink.Item.ItemClick -= new ItemClickEventHandler(DoExecute);
        }

        #region private
        BarItemLink _barItemLink;
        #endregion        
    }
}
