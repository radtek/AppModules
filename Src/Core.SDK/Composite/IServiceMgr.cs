using System;
using System.Collections.Generic;
using System.Text;

namespace SDK.Composite
{
    public interface IServiceMgr
    {
        T GetService<T>();
    }
}
