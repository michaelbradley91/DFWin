using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DFWin.Core.Constants;
using DFWin.Core.PInvoke;
using Microsoft.Xna.Framework.Input;
using PInvoke;

namespace DFWin.Core.Helpers
{
    public static class MouseButtonHelpers
    {
        public static ButtonState GetButtonState(this MouseState mouseState, MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return mouseState.LeftButton;
                case MouseButtons.Right:
                    return mouseState.RightButton;
                case MouseButtons.Middle:
                    return mouseState.MiddleButton;
                case MouseButtons.Forward:
                    return mouseState.XButton1;
                case MouseButtons.Backward:
                    return mouseState.XButton2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        /// <summary>
        /// Due to issues with certain mice / Monogame, the returned state may not exactly match the Monogame state.
        /// </summary>
        public static IReadOnlyDictionary<MouseButtons, ButtonState> GetButtonState(this MouseState mouseState)
        {
            // Due to an issue with some mice / Monogame, we check the XButtons with PInvoke as well.
            var isFowardPressed = mouseState.XButton2 == ButtonState.Pressed || User32.VirtualKey.VK_XBUTTON2.IsPressed();
            var isBackwardPressed = mouseState.XButton1 == ButtonState.Pressed || User32.VirtualKey.VK_XBUTTON1.IsPressed();

            return new Dictionary<MouseButtons, ButtonState>
            {
                {MouseButtons.Left, mouseState.LeftButton},
                {MouseButtons.Right, mouseState.RightButton},
                {MouseButtons.Middle, mouseState.MiddleButton},
                {MouseButtons.Forward, isFowardPressed ? ButtonState.Pressed : ButtonState.Released},
                {MouseButtons.Backward, isBackwardPressed ? ButtonState.Pressed : ButtonState.Released}
            };
        }

        /// <summary>
        /// Due to issues with certain mice / Monogame, the returned buttons may not exactly match the Monogame state.
        /// </summary>
        public static ImmutableHashSet<MouseButtons> GetPressedButtons(this MouseState mouseState)
        {
            return mouseState.GetButtonState().Where(kvp => kvp.Value == ButtonState.Pressed).Select(kvp => kvp.Key).ToImmutableHashSet();
        }
    }
}