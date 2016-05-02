using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SDK.Composite.UI
{
    [Flags]
    public enum DialogFormOption
    {
        ShowCloseButton = 1 << 0,
        ShowMinimizeButton = 1 << 1,
        ShowMaximizeButton = 1 << 2,
        ShowIcon = 1 << 3,
        ShowInOSTaskBar = 1 << 4,        
        FixedSize = 1 << 5,
        Locked = 1 << 6,
        FullScreen = 1 << 7,
        Minimize = 1 << 8,
        ShowBorder = 1 << 9,        
    }

    public static class DialogFormOptionsHelper
    {
        public static DialogFormOption Default = DialogFormOption.ShowIcon | DialogFormOption.ShowMinimizeButton | DialogFormOption.ShowMaximizeButton |
            DialogFormOption.ShowCloseButton | DialogFormOption.ShowBorder;

        public static DialogFormOption Message = DialogFormOption.ShowIcon | DialogFormOption.ShowCloseButton | DialogFormOption.ShowBorder |
            DialogFormOption.FixedSize;

        public static DialogFormOption Dialog = DialogFormOption.ShowIcon | DialogFormOption.ShowCloseButton | DialogFormOption.ShowBorder |
            DialogFormOption.FixedSize;

        public static DialogFormOption SplashScreen = DialogFormOption.FixedSize | DialogFormOption.Locked;
    }    
}
