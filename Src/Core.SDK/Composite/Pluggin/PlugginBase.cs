using System;
using System.Collections.Generic;
using System.Text;
using Core.SDK.Composite.Common;
using Core.SDK.Log;
using Core.SDK.Composite.Service;

namespace Core.SDK.Composite.Pluggin
{
    public abstract class PlugginBase : IPluggin
    {
        public PlugginBase()
        {
            _isFail = false;
            _logMgr = ServiceMgr.Current.GetInstance<ILogMgr>();
            _logger = _logMgr.GetLogger(InternalName);
            _logger.Debug("Create.");
        }              

        public void Init()
        {
            try
            {
                _logger.Info("Init.");
                InitInternal();
            }
            catch (Exception ex)
            {
                IsFail = true;
                _logger.Fatal(ex);
                Dispose();
            }            
        }

        public void Preprocess()
        {
            if (IsFail) return;

            try
            {
                _logger.Info("Preprocess.");
                PreprocessInternal();
            }
            catch (Exception ex)
            {
                IsFail = true;
                _logger.Fatal(ex);
                Dispose();
            }               
        }

        public void Run()
        {
            if (IsFail) return;

            try
            {
                _logger.Info("Run.");
                RunInternal();
            }
            catch (Exception ex)
            {
                IsFail = true;
                _logger.Fatal(ex);
                Dispose();
            }
        }

        public string Name
        {
            get { return GetUIName(); }
        }

        public string InternalName
        {
            get { return GetInternalName(); }
        }

        public IdentKey Identidicator
        {
            get { return GetInternalIdent(); }
        }

        public bool IsFail
        {
            get { return _isFail; }
            protected set 
            { 
                _isFail = value;
                _logger.Debug("Set IsFail = {0};", value.ToString());
            }
        }

        public virtual void Dispose() { }

        protected abstract string GetUIName();
        protected abstract string GetInternalName();

        protected abstract IdentKey GetInternalIdent();

        protected abstract void InitInternal();

        protected abstract void PreprocessInternal();

        protected abstract void RunInternal();
        

        #region protected
        protected ILogMgr _logMgr;
        protected ILogger _logger;
        bool _isFail;
        #endregion protected
    }
}
