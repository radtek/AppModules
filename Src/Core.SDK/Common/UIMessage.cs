using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.SDK.Common
{
    public abstract class UIMessage
    {
        protected UIMessage(string code, string text, string addInfo, Exception ex)
        {
            Code = code;
            Text = text;
            AddInfo = addInfo;
            Exception = ex;
        }

        public string Code { get; protected set; }
        public string Text { get; protected set; }
        public string AddInfo { get; protected set; }
        public Exception Exception { get; protected set; }

        public override string ToString()
        {
            return string.Format("Code-{0}: {1}. /n{2}", Code, Text, AddInfo);
        }

        public virtual string FullText()
        {
            if (Exception == null) return ToString();
            else return string.Format("{0}/n{1}/nStack:/n", ToString(), Exception.Message, Exception.StackTrace);
        }
    }
}
