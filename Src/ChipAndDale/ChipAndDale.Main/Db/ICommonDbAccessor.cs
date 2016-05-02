using ChipAndDale.SDK.Common;

namespace ChipAndDale.Main.Db
{
    internal interface ICommonDbAccessor
    {
        void AuthorizateUser(string name);
        void AuthorizateUser(string name, string password);

        void SetParam(string name, string value);

        void SendMessage(MessageEntity message);      
    }    
}
