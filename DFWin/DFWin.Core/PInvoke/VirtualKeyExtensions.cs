using System;
using Microsoft.Xna.Framework.Input;
using PInvoke;

namespace DFWin.Core.PInvoke
{
    public static class VirtualKeyExtensions
    {
        /// <summary>
        /// Converts a MonoGame key into a User32 key. This returns VK_NO_KEY (0) if the key cannot be translated.
        /// </summary>
        public static User32.VirtualKey ToVirtualKey(this Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    return User32.VirtualKey.VK_BACK;
                case Keys.Tab:
                    return User32.VirtualKey.VK_TAB;
                case Keys.Enter:
                    return User32.VirtualKey.VK_RETURN;
                case Keys.CapsLock:
                    return User32.VirtualKey.VK_CAPITAL;
                case Keys.Escape:
                    return User32.VirtualKey.VK_ESCAPE;
                case Keys.Space:
                    return User32.VirtualKey.VK_SPACE;
                case Keys.End:
                    return User32.VirtualKey.VK_END;
                case Keys.Home:
                    return User32.VirtualKey.VK_HOME;
                case Keys.Left:
                    return User32.VirtualKey.VK_LEFT;
                case Keys.Up:
                    return User32.VirtualKey.VK_UP;
                case Keys.Right:
                    return User32.VirtualKey.VK_RIGHT;
                case Keys.Down:
                    return User32.VirtualKey.VK_DOWN;
                case Keys.Select:
                    return User32.VirtualKey.VK_SELECT;
                case Keys.Print:
                    return User32.VirtualKey.VK_PRINT;
                case Keys.Execute:
                    return User32.VirtualKey.VK_EXECUTE;
                case Keys.Insert:
                    return User32.VirtualKey.VK_INSERT;
                case Keys.Delete:
                    return User32.VirtualKey.VK_DELETE;
                case Keys.Help:
                    return User32.VirtualKey.VK_HELP;
                case Keys.D0:
                    return User32.VirtualKey.VK_END;
                case Keys.A:
                    return User32.VirtualKey.VK_A;
                case Keys.B:
                    return User32.VirtualKey.VK_B;
                case Keys.C:
                    return User32.VirtualKey.VK_C;
                case Keys.D:
                    return User32.VirtualKey.VK_D;
                case Keys.E:
                    return User32.VirtualKey.VK_E;
                case Keys.F:
                    return User32.VirtualKey.VK_F;
                case Keys.G:
                    return User32.VirtualKey.VK_G;
                case Keys.H:
                    return User32.VirtualKey.VK_H;
                case Keys.I:
                    return User32.VirtualKey.VK_I;
                case Keys.J:
                    return User32.VirtualKey.VK_J;
                case Keys.K:
                    return User32.VirtualKey.VK_K;
                case Keys.L:
                    return User32.VirtualKey.VK_L;
                case Keys.M:
                    return User32.VirtualKey.VK_M;
                case Keys.N:
                    return User32.VirtualKey.VK_N;
                case Keys.O:
                    return User32.VirtualKey.VK_O;
                case Keys.P:
                    return User32.VirtualKey.VK_P;
                case Keys.Q:
                    return User32.VirtualKey.VK_Q;
                case Keys.R:
                    return User32.VirtualKey.VK_R;
                case Keys.S:
                    return User32.VirtualKey.VK_S;
                case Keys.T:
                    return User32.VirtualKey.VK_T;
                case Keys.U:
                    return User32.VirtualKey.VK_U;
                case Keys.V:
                    return User32.VirtualKey.VK_V;
                case Keys.W:
                    return User32.VirtualKey.VK_W;
                case Keys.X:
                    return User32.VirtualKey.VK_X;
                case Keys.Y:
                    return User32.VirtualKey.VK_Y;
                case Keys.Z:
                    return User32.VirtualKey.VK_Z;
                case Keys.Apps:
                    return User32.VirtualKey.VK_APPS;
                case Keys.Sleep:
                    return User32.VirtualKey.VK_SLEEP;
                case Keys.NumPad0:
                    return User32.VirtualKey.VK_NUMPAD0;
                case Keys.NumPad1:
                    return User32.VirtualKey.VK_NUMPAD1;
                case Keys.NumPad2:
                    return User32.VirtualKey.VK_NUMPAD2;
                case Keys.NumPad3:
                    return User32.VirtualKey.VK_NUMPAD3;
                case Keys.NumPad4:
                    return User32.VirtualKey.VK_NUMPAD4;
                case Keys.NumPad5:
                    return User32.VirtualKey.VK_NUMPAD5;
                case Keys.NumPad6:
                    return User32.VirtualKey.VK_NUMPAD6;
                case Keys.NumPad7:
                    return User32.VirtualKey.VK_NUMPAD7;
                case Keys.NumPad8:
                    return User32.VirtualKey.VK_NUMPAD8;
                case Keys.NumPad9:
                    return User32.VirtualKey.VK_NUMPAD9;
                case Keys.Multiply:
                    return User32.VirtualKey.VK_MULTIPLY;
                case Keys.Add:
                    return User32.VirtualKey.VK_ADD;
                case Keys.Separator:
                    return User32.VirtualKey.VK_SEPARATOR;
                case Keys.Subtract:
                    return User32.VirtualKey.VK_SUBTRACT;
                case Keys.Decimal:
                    return User32.VirtualKey.VK_DECIMAL;
                case Keys.Divide:
                    return User32.VirtualKey.VK_DIVIDE;
                case Keys.F1:
                    return User32.VirtualKey.VK_F1;
                case Keys.F2:
                    return User32.VirtualKey.VK_F2;
                case Keys.F3:
                    return User32.VirtualKey.VK_F3;
                case Keys.F4:
                    return User32.VirtualKey.VK_F4;
                case Keys.F5:
                    return User32.VirtualKey.VK_F5;
                case Keys.F6:
                    return User32.VirtualKey.VK_F6;
                case Keys.F7:
                    return User32.VirtualKey.VK_F7;
                case Keys.F8:
                    return User32.VirtualKey.VK_F8;
                case Keys.F9:
                    return User32.VirtualKey.VK_F9;
                case Keys.F10:
                    return User32.VirtualKey.VK_F10;
                case Keys.F11:
                    return User32.VirtualKey.VK_F11;
                case Keys.F12:
                    return User32.VirtualKey.VK_F12;
                case Keys.F13:
                    return User32.VirtualKey.VK_F13;
                case Keys.F14:
                    return User32.VirtualKey.VK_F14;
                case Keys.F15:
                    return User32.VirtualKey.VK_F15;
                case Keys.F16:
                    return User32.VirtualKey.VK_F16;
                case Keys.F17:
                    return User32.VirtualKey.VK_F17;
                case Keys.F18:
                    return User32.VirtualKey.VK_F18;
                case Keys.F19:
                    return User32.VirtualKey.VK_F19;
                case Keys.F20:
                    return User32.VirtualKey.VK_F20;
                case Keys.F21:
                    return User32.VirtualKey.VK_F21;
                case Keys.F22:
                    return User32.VirtualKey.VK_F22;
                case Keys.F23:
                    return User32.VirtualKey.VK_F23;
                case Keys.F24:
                    return User32.VirtualKey.VK_F24;
                case Keys.NumLock:
                    return User32.VirtualKey.VK_NUMLOCK;
                case Keys.Scroll:
                    return User32.VirtualKey.VK_SCROLL;
                case Keys.LeftShift:
                    return User32.VirtualKey.VK_SHIFT;
                case Keys.RightShift:
                    return User32.VirtualKey.VK_SHIFT;
                case Keys.LeftControl:
                    return User32.VirtualKey.VK_CONTROL;
                case Keys.RightControl:
                    return User32.VirtualKey.VK_CONTROL;
                case Keys.Pause:
                    return User32.VirtualKey.VK_PAUSE;
                default:
                    DfWin.Warn("Could not translate key: " + key);
                    return User32.VirtualKey.VK_NO_KEY;
            }
        }

        private const int KeyPressed = 0x8000;

        /// <summary>
        /// Returns false if PInvoke cannot determine if the key is pressed.
        /// </summary>
        public static bool IsPressed(this User32.VirtualKey virtualKey)
        {
            try
            {
                return Convert.ToBoolean(User32.GetKeyState((int) virtualKey) & KeyPressed);
            }
            catch (Exception e)
            {
                DfWin.Error("Unable to retrieve key state with PInvoke due to exception: " + e);
                return false;
            }
        }
    }
}
