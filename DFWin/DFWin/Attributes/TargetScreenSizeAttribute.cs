using System;

namespace DFWin.Attributes
{
    public class TargetScreenSizeAttribute : Attribute
    {
        public int Width { get; }
        public int Height { get; }

        public TargetScreenSizeAttribute(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
