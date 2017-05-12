using System.Runtime.InteropServices;

namespace DFWin.Core.User32Extensions.Structs
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// ANIMATIONINFO specifies animation effects associated with user actions. 
    /// Used with SystemParametersInfo when SPI_GETANIMATION or SPI_SETANIMATION action is specified.
    /// </summary>
    /// <remark>
    /// The uiParam value must be set to (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO)) when using this structure.
    /// </remark>
    [StructLayout(LayoutKind.Sequential)]
    public struct ANIMATIONINFO
    {
        /// <summary>
        /// Creates an AMINMATIONINFO structure.
        /// </summary>
        /// <param name="iMinAnimate">If non-zero and SPI_SETANIMATION is specified, enables minimize/restore animation.</param>
        public ANIMATIONINFO(int iMinAnimate)
        {
            cbSize = (uint)Marshal.SizeOf(typeof(ANIMATIONINFO));
            this.iMinAnimate = iMinAnimate;
        }

        /// <summary>
        /// Creates an AMINMATIONINFO structure.
        /// </summary>
        /// <param name="minAnimate">If true and SPI_SETANIMATION is specified, enables minimize/restore animation.</param>
        public ANIMATIONINFO(bool minAnimate) : this(minAnimate ? 1 : 0) { }

        /// <summary>
        /// Always must be set to (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO)).
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// If non-zero, minimize/restore animation is enabled, otherwise disabled.
        /// </summary>
        public int iMinAnimate;

        public bool IsWindowRestorationAndMinimisationAnimated
        {
            get
            {
                return iMinAnimate != 0;
            }
            set
            {
                iMinAnimate = value ? 1 : 0;
            }
        }

        public static uint GetSize()
        {
            return (uint)Marshal.SizeOf(typeof(ANIMATIONINFO));
        }
    }
}
