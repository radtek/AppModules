
namespace Core.SDK.Setting
{
    public interface ISettingReadWriter
    {
        void Load(string username, string sectionName, string subSectionName);
        void Save(string username, string sectionName, string subSectionName, bool isCommit);
        void Clear(string username, string sectionName, bool isCommit);
        
        // true - есть запись в БД, false - нет записи в БД
        bool ReadValue(string settingName, out string settingValue);
        bool ReadValue(string settingName, out byte[] settingValue);
        void WriteValue(string settingName, string settingValue);
        void WriteValue(string settingName, byte[] settingValue);
    }
}
