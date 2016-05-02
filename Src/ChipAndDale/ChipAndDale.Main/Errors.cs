using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.SDK.Common;

namespace ChipAndDale.Main
{
    internal class NsiLoadError : UIMessage
    {
        internal NsiLoadError(string addInfo, Exception ex)
            : base("100", Properties.Resources.NsiLoadErrorMessage, addInfo, ex)
        { }
    }

    internal class NsiEditFormError : UIMessage
    {
        internal NsiEditFormError(string addInfo, Exception ex)
            : base("101", Properties.Resources.NsiEditFormErrorMessage, addInfo, ex)
        { }
    }

    internal class NsiViewFormError : UIMessage
    {
        internal NsiViewFormError(string addInfo, Exception ex)
            : base("102", Properties.Resources.NsiViewFormErrorMessage, addInfo, ex)
        { }
    }
}
