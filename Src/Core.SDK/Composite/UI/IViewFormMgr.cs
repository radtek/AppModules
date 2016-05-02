using System;
using Core.SDK.Composite.Common;
using System.Windows.Forms;
namespace Core.SDK.Composite.UI
{
    public interface IViewFormMgr
    {                                
        DialogResult ShowModal(IView view);
        DialogResult ShowModal(IView view, DialogSetting settings);
        DialogResult ShowModal(IView view, DialogFormOption options);
        DialogResult ShowModal(IView view, DialogFormOption options, DialogSetting settings);        
        
        IdentKey ShowNoModal(IView view);
        IdentKey ShowNoModal(IView view, DialogSetting settings);
        IdentKey ShowNoModal(IView view, DialogFormOption options);
        IdentKey ShowNoModal(IView view, DialogFormOption options, DialogSetting settings);
        void CloseNoModal(IdentKey key);        
        void ChangeStateNoModal(IdentKey key, bool isActivate);
        bool IsActiveNoModal(IdentKey key);        
        
        DialogSetting GetDialogSetting(IdentKey key);
    }
}
