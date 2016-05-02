using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.UI
{
    public interface IRegionMgr
    {
        IViewRegion GetViewRegion(string name);
        void AddViewRegion(string name, IViewRegion region);
        IEnumerable<IViewRegion> ViewRegions { get; }
        
        ICommandRegion GetCommandRegion(string name);
        void AddCommandRegion(string name, ICommandRegion region);
        IEnumerable<ICommandRegion> CommandRegions { get; }

        IEnumerable<string> RegionNames { get; }

        void RestoreLayout(List<KeyValuePair<string, byte[]>> layoutArray);
        List<KeyValuePair<string, byte[]>> SaveLayout();
    }
}
