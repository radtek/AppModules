using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars;
using Core.SDK.Composite.UI;

namespace Core.XtraCompositeModule
{
    class CheckedBarItemInvoker : BarItemInvoker
    {
        public CheckedBarItemInvoker(BarCheckItemLink itemLink, ICommand command)
            : base(itemLink, command)
        { }

        public override void UpdateUI()
        {
            BarCheckItem checkItem = BarItemLink.Item as BarCheckItem;
            if (checkItem != null) checkItem.Checked = Command.IsChecked;
            base.UpdateUI();
        }
    }
}
