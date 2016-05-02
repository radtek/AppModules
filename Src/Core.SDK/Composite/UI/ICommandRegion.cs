using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.UI
{
    public interface ICommandRegion : IRegion
    {
        IdentKey AddCommand(ICommand view);
        IdentKey AddCommand(ICommand view, string parentItem);
        void RemoveCommand(IdentKey key);
        void RefreshCommand(IdentKey key);
    }
}
