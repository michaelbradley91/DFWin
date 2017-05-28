using System.Collections.Immutable;
using DFWin.Core.Models;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Inputs
{
    public class KeyboardInput
    {
        public KeyboardState KeyboardState { get; }
        public ImmutableDictionary<Keys, KeyRecording> KeyRecordings { get; }
        public ImmutableHashSet<Keys> PressedKeys { get; }

        public KeyboardInput(KeyboardState keyboardState, ImmutableDictionary<Keys, KeyRecording> keyRecordings, ImmutableHashSet<Keys> pressedKeys)
        {
            KeyboardState = keyboardState;
            KeyRecordings = keyRecordings;
            PressedKeys = pressedKeys;
        }

        public static KeyboardInput InitialInput => new KeyboardInput(Keyboard.GetState(), ImmutableDictionary<Keys, KeyRecording>.Empty, ImmutableHashSet<Keys>.Empty);
    }
}
