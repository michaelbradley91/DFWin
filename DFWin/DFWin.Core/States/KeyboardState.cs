using System.Collections.Immutable;
using DFWin.Core.Models;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.States
{
    public class KeyboardState
    {
        public ImmutableDictionary<Keys, KeyRecording> KeyRecordings { get; }
        public ImmutableHashSet<Keys> PressedKeys { get; }

        public KeyboardState(ImmutableDictionary<Keys, KeyRecording> keyRecordings, ImmutableHashSet<Keys> pressedKeys)
        {
            KeyRecordings = keyRecordings;
            PressedKeys = pressedKeys;
        }

        public static readonly KeyboardState InitialState = new KeyboardState(ImmutableDictionary<Keys, KeyRecording>.Empty, ImmutableHashSet<Keys>.Empty);
    }
}
