using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.SDK.Dom
{
    public interface IEntityList<T> where T : EntityBase<T>
    {
        BindingCollection<T> Entities { get; }

        T Current { get; set; }
        
        void SetBindingSource(BindingSource binding);

        string Caption { get; }
    }
}
