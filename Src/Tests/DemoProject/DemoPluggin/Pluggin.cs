using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Composite.Pluggin;
using Core.SDK.Composite.Common;
using System.ComponentModel.Composition;
using Core.SDK.Composite.Service;
using Core.SDK.Composite.Event;

namespace DemoPluggin
{
    [Export(typeof(IPluggin))]
    public class Pluggin : PlugginBase
    {
        [ImportingConstructor]
        public Pluggin(IServiceMgr serv, IEventMgr ev): base()
        { 
        
        }
        
        protected override string GetUIName()
        {
            return "Pluggin name";
        }

        protected override IdentKey GetInternalIdent()
        {
            return new IdentKey();
        }

        [Export(typeof(string))]
        public string TestStr { get { return "test!!!"; } }        
    }
}
