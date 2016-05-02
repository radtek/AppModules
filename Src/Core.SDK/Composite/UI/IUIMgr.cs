using System;
using System.Collections.Generic;
using System.Text;
using Core.SDK.Composite.Common;
using System.Drawing;
using Core.SDK.Log;

namespace Core.SDK.Composite.UI
{
    interface IUIMgr
    {        
        IdentKey AddMenuItem(string regionName, ICommand command);
        IdentKey AddMenuItem(string regionName, ICommand command, string parentItem);
        void RemoveMenuItem(IdentKey ident);
        void RefreshMenuItem(IdentKey ident);

        
        void SetStatusItemText(string regionName, string text);

        
        IdentKey AddView(string regionName, IView view, IdentKey existPanelKey);
        void ActivateView(IdentKey key);
        void CloseView(IdentKey ident);

        
        IdentKey CreateViewForm(IView view, DialogFormOption options);
        void RemoveViewForm(IdentKey key);
        void CloseViewForm(IdentKey key);
        void ChangeViewFormState(IdentKey key, bool isActivate);
        DialogSetting GetViewFormDialogSetting(IdentKey key);
        bool? IsViewFormActive(IdentKey key);
        bool? IsViewFormClose(IdentKey key);
        bool? IsViewFormModal(IdentKey key);
        bool? ShowViewFormModal(IdentKey key);
        bool? ShowViewFormModal(IdentKey key, DialogSetting settings);
        void ShowViewFormNoModal(IdentKey key);
        void ShowViewFormNoModal(IdentKey key, DialogSetting settings); 
    }            
}
