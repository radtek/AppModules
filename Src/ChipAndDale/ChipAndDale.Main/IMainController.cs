using ChipAndDale.SDK.Common;

namespace ChipAndDale.Main
{
    public interface IMainController: ICommonService
    {
        void Init();

        void OnConnect();
        void OnReconnect();
        void OnDisconnect();
        
        void OnLoadSetting();
        void OnSaveSetting();
    }
}
