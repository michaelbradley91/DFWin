using DFWin.User32Extensions.Structs;

namespace DFWin.User32Extensions.Models
{
    public class SystemInformation
    {
        public static readonly SystemInformation Current = new SystemInformation();

        private SystemInformation() { }

        /// <summary>
        /// Returns true if minimising, restoring or maximising windows is animated. Returns false otherwise.
        /// </summary>
        public bool AreWindowStateChangesAnimated()
        {
            var animationInfo = User32Extensions.GetSystemAnimationInfo();
            return animationInfo.IsWindowRestorationAndMinimisationAnimated;
        }

        /// <summary>
        /// Enables or disables animations shown when windows are minimised, restored or maximised.
        /// </summary>
        public void SetWindowStateChangeAnimation(bool enabled)
        {
            User32Extensions.SetSystemAnimationInfo(new ANIMATIONINFO(enabled));
        }
    }
}
