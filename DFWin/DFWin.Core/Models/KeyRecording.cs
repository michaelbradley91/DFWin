using System;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Models
{
    public class KeyRecording
    {
        public Keys Key { get; }
        public bool WasDown { get; }
        public DateTimeOffset Time { get; }

        public KeyRecording(Keys key, bool wasDown)
        {
            Key = key;
            WasDown = wasDown;
            Time = DateTimeOffset.UtcNow;
        }
    }
}
