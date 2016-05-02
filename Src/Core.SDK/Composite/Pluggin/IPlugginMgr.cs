using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.Pluggin
{
    public interface IPlugginMgr
    {
        IEnumerable<IPluggin> Pluggins { get; }

        void Init();
        void Preprocess();
        void Run();
        void Dispose();
    }
}
