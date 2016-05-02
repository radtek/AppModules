using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Core.SDK.Composite.UI
{
    public enum CommandType
    {
        Button,        
        CheckButton,
        Group
    }

    public interface ICommand
    {
        void Execute();

        string Name { get; }    // For internal using
        string Caption { get; }
        string Hint { get; }
        Image Image { get; }
        bool IsVisible { get; }
        bool IsEnabled { get; }
        bool IsChecked { get; }
        bool HasState { get; }

        CommandType CommandType { get; }
    }
}
