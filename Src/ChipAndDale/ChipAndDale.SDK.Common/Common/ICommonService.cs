using ChipAndDale.SDK.Nsi;

namespace ChipAndDale.SDK.Common
{
    public interface ICommonService
    {
        void SetParam(string name, string value);

        void ClearAllParams(bool isCommit);

        void SendMessage(MessageEntity message, bool isCommit);

        bool IsConnect { get; }
        bool IsConnectWithMessage { get; }

        UserEntity User { get; }

        string Login { get; }
    }
}
