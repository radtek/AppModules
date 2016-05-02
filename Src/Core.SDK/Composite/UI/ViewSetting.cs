using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Core.SDK.Composite.UI
{
    public class DialogSetting
    {
        public DialogSetting()
            : this(new Point(100, 100), new Size(100, 100))
        { }

        public DialogSetting(Point position, Size size)
        {
            Position = position;
            Size = size;
            IsMinimize = false;
        }

        public Point Position { get; set; }
        public Size Size { get; set; }
        public bool IsMinimize { get; set; }

        public DialogSetting Clone()
        {
            DialogSetting setting = new DialogSetting();
            setting.Position = Position;
            setting.Size = Size;
            setting.IsMinimize = IsMinimize;
            return setting;
        }

        public override bool  Equals(object obj)
        {
            DialogSetting setting = obj as DialogSetting;
            if (setting == null) return false;

            return Point.Equals(Position, setting.Position) &&
                Size.Equals(Size, setting.Size) &&
                IsMinimize == setting.IsMinimize;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
