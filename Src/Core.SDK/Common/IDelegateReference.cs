using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.Common
{
    public interface IDelegateReference
    {       
        Delegate Target { get; }
    }
}
