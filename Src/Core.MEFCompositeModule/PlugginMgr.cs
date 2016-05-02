using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Core.SDK.Log;
using Core.SDK.Composite.Pluggin;

namespace Core.CompositeModule
{
    public class PlugginMgr : IPlugginMgr
    {
        public PlugginMgr(ILogMgr logMgr)
        {
            _logMgr = logMgr;
            _logger = _logMgr.GetLogger("PlugginMgr");
            _logger.Info("Create.");            
        }

        [ImportMany(typeof(IPluggin))]        
        public IEnumerable<IPluggin> Pluggins { get; set; }        

        #region private
        ILogMgr _logMgr;
        ILogger _logger;
        #endregion private



        public void Init()
        {
            _logger.Debug("Init.");
            foreach (IPluggin pluggin in Pluggins)
            {
                try
                {
                    pluggin.Init();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Pluggin's name = " + pluggin.InternalName);
                }
            }            
        }

        public void Preprocess()
        {
            _logger.Debug("Preprocess.");
            foreach (IPluggin pluggin in Pluggins)
            {
                try
                {
                    pluggin.Preprocess();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Pluggin's name = " + pluggin.InternalName);
                }
            }  
        }

        public void Run()
        {
            _logger.Debug("Run.");
            foreach (IPluggin pluggin in Pluggins)
            {
                try
                {
                    pluggin.Run();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Pluggin's name = " + pluggin.InternalName);
                }
            }  
        }

        public void Dispose()
        {
            _logger.Debug("Dispose.");
            foreach (IPluggin pluggin in Pluggins)
            {
                try
                {
                    pluggin.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Pluggin's name = " + pluggin.InternalName);
                }
            }  
        }
    }
}
