using ChipAndDale.SDK.Nsi;

namespace ChipAndDale.Main.Nsi
{
    internal interface INsiController : INsiService
    {
        void OpenUsersEditDialog();
        void OpenAppsEditDialog();
        void OpenOrgsEditDialog();
        void OpenTagsEditDialog();

        void Refresh();
    }
}
