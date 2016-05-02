using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.UtilsModule;
using System.ComponentModel;

namespace ChipAndDale.SDK.Common
{
    public enum InfoSourceType
    {
        [Description("Телефон")]
        Call = 1,
        [Description("Ел.пошта")]
        Email = 2,
        [Description("Лист")]
        Letter = 3,
        [Description("Форум")]
        Forum = 4,
        [Description("Джабер")]
        Jabber = 5
    }
}
