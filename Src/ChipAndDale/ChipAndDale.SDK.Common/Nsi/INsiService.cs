using System;
using System.Collections.Generic;
using Core.SDK.Dom;

namespace ChipAndDale.SDK.Nsi
{
    public interface INsiService
    {
        BindingCollection<UserEntity> Users { get; }
        IEnumerable<UserEntity> GetUserByName(string name);
        UserEntity GetUserById(string id);
        UserEntity GetUserByLogin(string login);
        IEnumerable<UserEntity> OpenUsersDialog();

        BindingCollection<AppEntity> Applications { get; }
        IEnumerable<AppEntity> GetAppByName(string name);
        AppEntity GetAppById(string id);
        IEnumerable<AppEntity> OpenAppsDialog();

        BindingCollection<OrgEntity> Organizations { get; }
        IEnumerable<OrgEntity> GetOrgByName(string name);
        OrgEntity GetOrgById(string id);
        IEnumerable<OrgEntity> OpenOrgsDialog();

        BindingCollection<TagEntity> Tags { get; }
        IEnumerable<TagEntity> GetTagByName(string name);
        TagEntity GetTagById(string id);
        IEnumerable<TagEntity> OpenTagsDialog();

        IList<KeyValuePair<Enum, string>> InfoSourceTypes { get; }
    }
}
