using System;
using System.Collections.Generic;
using System.Text;
using Core.SDK.Composite.Common;
using System.Windows.Forms;

namespace Core.SDK.Common
{
    public delegate bool BooleanDelegate();
    public delegate void ExecuteDelegate();
    public delegate void ActivateDelegate(bool isActive);
    public delegate bool OnClosingDelegate(Nullable<bool> result);
    public delegate void IdentKeyDelegate(IdentKey key);
}
