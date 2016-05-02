
namespace Core.SDK.Setting
{
    public interface ISettingMgr
    {
        ISettingReadWriter ReadWriteProvider { get; set; }

        void LoadSetting(string userName, string sectionName, string subSectionName, object setting);
        void SaveSettings(string userName, string sectionName, string subSectionName, object setting);
        void SaveSetting(string userName, string sectionName, string subSectionName, object setting, bool isCommit);

        void ClearSettings(string userName, string sectionName,  bool isCommit);

        void LoadSettingProperty(string userName, string sectionName, string subSectionName, object setting, string propertyName);
        void SaveSettingProperty(string userName, string sectionName, string subSectionName, object setting, string propertyName);
    }
}
