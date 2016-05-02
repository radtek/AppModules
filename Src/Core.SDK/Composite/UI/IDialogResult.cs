using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Composite.UI
{
    public interface IDialogResult
    {
        object OkButton { get; }
        object CancelButton { get; }
    }
}
