using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars;
using Core.SDK.Composite.UI;

namespace Core.XtraCompositeModule
{
    public static class BarItemFactory
    {
        public static BarItem CreateItem(CommandType itemType)
        {
            BarItem result = null;
            switch (itemType)
            {
                case CommandType.Button:
                    result = new BarButtonItem();
                    break;
                case CommandType.CheckButton:
                    result = new BarCheckItem();
                    break;
                case CommandType.Group:
                    result = new BarSubItem();
                    break;
                default:
                    throw new NotSupportedException(string.Format("BarItemFactory. Command type = {0} not supported.", itemType.ToString()));
            }
            return result;
        }
    }

    public static class BarItemInvokerFactory
    {
        public static  BarItemInvoker CreateItemInvoker(BarItemLink link, ICommand command)
        {
            BarItemInvoker result = null;
            switch (command.CommandType)
            {
                case CommandType.Button:
                    result = new BarItemInvoker(link, command);
                    break;
                case CommandType.CheckButton:
                    result = new CheckedBarItemInvoker((BarCheckItemLink)link, command);
                    break;
                case CommandType.Group:
                    result = new BarItemInvoker(link, command);
                    break;
                default:
                    throw new NotSupportedException(string.Format("BarItemInvokerFactory. Command type = {0} not supported.", command.CommandType.ToString()));
            }
            return result;
        }
    }
}
