using DFWin.Core.PInvoke.Structs;

namespace DFWin.Core.PInvoke.Models
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
            var animationInfo = PInvokeExtensions.GetSystemAnimationInfo();
            return animationInfo.IsWindowRestorationAndMinimisationAnimated;
        }

        /// <summary>
        /// Enables or disables animations shown when windows are minimised, restored or maximised.
        /// </summary>
        public void SetWindowStateChangeAnimation(bool enabled)
        {
            PInvokeExtensions.SetSystemAnimationInfo(new ANIMATIONINFO(enabled));
        }
    }
}
