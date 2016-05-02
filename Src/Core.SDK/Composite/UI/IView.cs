using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Core.SDK.Composite.Common;

namespace Core.SDK.Composite.UI
{    
    public interface IView
    {                
        object UserControl { get; }
        string Name { get; set; }
        string Caption { get; set; }
        string Hint { get; set; }
        IdentKey Ident { get; set; }
        Image Image { get; set; }
        DialogSetting DialogSetting { get; set; } 

        /// <summary>
        /// Определяет, можно ли закрывать окно - в данном методе может проводиться валидация введенных данных
        /// </summary>
        bool CanClose { get; }

        /// <summary>
        /// Вызывается перед закрытием окна
        /// </summary>
        /// <param name="result">Если представление помещается в диалоговое окно, то
        /// в данном параметре передается результат (аналог DialogResult). Если
        /// в регионе главной формы - null
        /// Возвращаемый результат определяет можно ли закрывать окно:
        /// в случае возникновения исключения, его необходимо перехватить и вернуть false - для
        /// исключения потери введенных данных</param>
        bool OnClosing(Nullable<bool> result);

        /// <summary>
        /// Вызывается после закрытия формы
        /// </summary>
        void OnAfterClose();

        /// <summary>
        /// Если представление добавляется в TabControl, то при смене активной закладки для всех неактивных
        /// будет вызван OnActivate(false), а для активной - OnActivate(true). Если представление добвляется в
        /// панель, то OnActivate(true) будет вызвано только при получении фокуса и только для данного представления.
        /// </summary>
        void OnActivate(bool isActive);
               

        /// <summary>
        ///  Вызывается при изменении состояния плавающей панели (не используется для диалоговых окон)
        /// </summary>
        /// <param name="dockOption"></param>
        void ChangeDockingState(ViewDockingState dockOption);                
    }  
}
