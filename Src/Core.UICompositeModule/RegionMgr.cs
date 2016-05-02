using System;
using System.Collections.Generic;
using Core.SDK.Composite.UI;
using Core.SDK.Log;

namespace Core.UICompositeModule
{
    public class RegionMgr : IRegionMgr
    {
        public RegionMgr(ILogMgr logMgr)
        {
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("RegionMgr");            
            _logger.Info("Create.");
            _ViewRegions = new Dictionary<string, IViewRegion>();
            _CommandRegions = new Dictionary<string, ICommandRegion>();
            _RegionNames = new List<string>();           
        }

        
        public IViewRegion GetViewRegion(string name)
        {
            IViewRegion region = null;
            if (_ViewRegions.TryGetValue(name, out region)) return region;
            else return null;
        }

        public void AddViewRegion(string name, IViewRegion region)
        {
            if (region == null) throw new ArgumentException("Region can not be null.");

            IViewRegion r = null;
            if (_ViewRegions.TryGetValue(name, out r)) throw new ArgumentException("Region with this name already exists.");
            
            _ViewRegions.Add(name, region);
            _RegionNames.Add(name);
            _logger.Debug("Added view region with name " + name);
        }

        public IEnumerable<IViewRegion> ViewRegions 
        {
            get
            { 
                foreach(IViewRegion region in _ViewRegions.Values)
                    yield return region;
            }
        }

        public ICommandRegion GetCommandRegion(string name)
        {
            ICommandRegion region = null;
            if (_CommandRegions.TryGetValue(name, out region)) return region;
            else return null;
        }

        public void AddCommandRegion(string name, ICommandRegion region)
        {
            if (region == null) throw new ArgumentException("Region can not be null.");

            ICommandRegion existRegion = null;
            if (_CommandRegions.TryGetValue(name, out existRegion)) throw new ArgumentException("Region with this name already exists.");

            _CommandRegions.Add(name, region);
            _RegionNames.Add(name);
            _logger.Debug("Added command region with name " + name);
        }

        public IEnumerable<ICommandRegion> CommandRegions
        {
            get
            {
                foreach (ICommandRegion region in _CommandRegions.Values)
                    yield return region;
            }
        }

        public IEnumerable<string> RegionNames 
        { 
            get
            {
                return _RegionNames;
            }
        }

        public void RestoreLayout(List<KeyValuePair<string, byte[]>> layoutList)
        {
            _logger.Debug("RestoreLayout");
            if (layoutList == null) return;

            foreach (KeyValuePair<string, byte[]> pair in layoutList)
            {                
                IViewRegion region = GetViewRegion(pair.Key);
                if (region != null) region.RestoreLayout(pair.Value);               
            }
        }

        public List<KeyValuePair<string, byte[]>> SaveLayout()
        {
            List<KeyValuePair<string, byte[]>> layoutList = new List<KeyValuePair<string, byte[]>>();
            foreach (IViewRegion region in _ViewRegions.Values)
            {
                KeyValuePair<string, byte[]> pair = new KeyValuePair<string, byte[]>(region.Name, region.SaveLayout());               
                layoutList.Add(pair);
            }
            return layoutList;
        }

        #region private
        Dictionary<string, IViewRegion> _ViewRegions;
        Dictionary<string, ICommandRegion> _CommandRegions;
        List<string> _RegionNames;
        ILogMgr _logMgr;
        ILogger _logger;        
        #endregion private
    }
}
