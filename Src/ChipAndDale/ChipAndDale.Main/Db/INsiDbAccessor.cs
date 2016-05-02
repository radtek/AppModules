using System.Collections.Generic;
using ChipAndDale.SDK.Nsi;
using Core.SDK.Dom;

namespace ChipAndDale.Main.Db
{
    interface INsiDbAccessor
    {        
        BindingCollection<AppEntity> GetApps();
        void AddApp(AppEntity app);
        void UpdateApp(AppEntity app);
        void RemoveApp(AppEntity app);
        void ProccessAppChanges(IEnumerable<AppEntity> add, IEnumerable<AppEntity> upd, IEnumerable<AppEntity> del);


        BindingCollection<OrgEntity> GetOrgs();
        void AddOrg(OrgEntity org);
        void UpdateOrg(OrgEntity org);
        void RemoveOrg(OrgEntity org);
        void ProccessOrgChanges(IEnumerable<OrgEntity> add, IEnumerable<OrgEntity> upd, IEnumerable<OrgEntity> del);


        BindingCollection<TagEntity> GetTags();
        void AddTag(TagEntity tag);
        void UpdateTag(TagEntity tag);
        void RemoveTag(TagEntity tag);
        void ProccessTagChanges(IEnumerable<TagEntity> add, IEnumerable<TagEntity> upd, IEnumerable<TagEntity> del);


        BindingCollection<UserEntity> GetUsers();
        void AddUser(UserEntity user);
        void UpdateUser(UserEntity user);
        void RemoveUser(UserEntity user);
        void ProccessUserChanges(IEnumerable<UserEntity> add, IEnumerable<UserEntity> upd, IEnumerable<UserEntity> del);
    }
}
