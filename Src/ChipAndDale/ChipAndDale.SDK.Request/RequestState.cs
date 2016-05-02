using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.UtilsModule;
using System.ComponentModel;

namespace ChipAndDale.SDK.Request
{
    [TypeConverter(typeof(EnumDescriptionConverter<RequestState>))]
    public enum RequestState
    {
        [Description("Відкрите")]
        Open = 0,
        [Description("Аналізується")]
        Analyze = 1,
        [Description("Виконується")]
        InWork = 2,
        [Description("Виконано")]
        Done = 3,
        [Description("Закрите")]
        Close = 4,    
    }
}
