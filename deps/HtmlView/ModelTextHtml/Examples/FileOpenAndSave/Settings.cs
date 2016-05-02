using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;

namespace FileOpenAndSave
{
    /// <summary>
    /// This class wraps the program's properties or settings which
    /// persist from one instance of the program to the next.
    /// </summary>
    public class Settings : ApplicationSettingsBase
    {
        internal Settings()
        {
            base.Reload();
        }

        [UserScopedSetting()]
        [DefaultSettingValue(null)]
        public string recentFilename
        {
            get { return (string)this["recentFilename"]; }
            set { this["recentFilename"] = value; base.Save(); }
        }
    }
}
