﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using DFWin.User32Extensions.Enumerations;
using DFWin.User32Extensions.Exceptions;
using PInvoke;

namespace DFWin.User32Extensions.Models
{
    /// <summary>
    /// An abstract representation of a window. Use this to more conveniently call down to User32 methods.
    /// </summary>
    public class Window : IDisposable
    {
        public Window(IntPtr windowPointer)
        {
            WindowPointer = windowPointer;
            Details = new WindowDetails(this);
        }

        private User32.SafeDCHandle deviceContext;

        private User32.SafeDCHandle GetDeviceContext()
        {
            if (deviceContext == null || deviceContext.IsClosed || deviceContext.IsInvalid)
            {
                deviceContext = User32.GetDC(WindowPointer);
            }
            return deviceContext;
        }

        /// <summary>
        /// The raw pointer to the window.
        /// </summary>
        public IntPtr WindowPointer { get; }

        /// <summary>
        /// The rectangle of the window, including its title bars and scroll bars.
        /// </summary>
        public Rectangle WindowRectangle => User32Extensions.GetWindowRectangle(WindowPointer);

        /// <summary>
        /// The rectangle of the client window, which is the area inside any immediate scroll / title bars.
        /// This is the area Dwarf Fortress is actually running in.
        /// </summary>
        public Rectangle ClientRectangle => User32Extensions.GetClientRectangle(WindowPointer);

        public WindowDetails Details { get; }

        public bool IsMinimised => User32.IsIconic(WindowPointer);

        /// <summary>
        /// Forces the window to redraw, which can be useful if the window was not previously visible and hasn't received focus.
        /// </summary>
        public void Redraw()
        {
            User32.SendMessage(WindowPointer, User32.WindowMessage.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the state of the window, avoiding automatically activating (focussing) the window if possible.
        /// </summary>
        public void SetState(WindowState windowState)
        {
            User32.ShowWindow(WindowPointer, windowState.ToShowStyle());
        }

        /// <summary>
        /// Modifies the window to ensure it is layered, and so supports settings such as transparency.
        /// </summary>
        public void EnsureLayered()
        {
            Details.EnsureLayered();
        }

        /// <summary>
        /// Sets if the window should be transparent (invisible) or not. This will throw if the window is not layered.
        /// </summary>
        public void SetTransparency(bool isTransparent)
        {
            if (!Details.IsLayered) throw new InvalidOperationException("Cannot set the transparency of a window that is not layered.");

            const int layeredWindowAlphaFlag = 0x2;

            var result = DllImports.SetLayeredWindowAttributes(WindowPointer, 0, isTransparent ? (byte)1 : (byte)0, layeredWindowAlphaFlag);
            if (result == 0) throw new User32Exception("Unable to set window transparency.", Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Resizes the client rectangle, which is the window without the title or scroll bars. This will not move the window from its top left position.
        /// 
        /// This does nothing if the window is already the given size (it won't even redraw).
        /// 
        /// This will steal the window focus, so you should return focus to your process if necessary. TODO: see if true without redrawing
        /// This will throw if the window is minimised, as it cannot be resized in that state.
        /// 
        /// Returns true if the window was resized.
        /// </summary>
        public bool ResizeClientRectangle(int width, int height, bool redrawIfResized = true)
        {
            var clientRectangle = ClientRectangle;
            var windowRectangle = WindowRectangle;

            if (clientRectangle.Width == width && clientRectangle.Height == height) return false;

            if (IsMinimised) throw new InvalidOperationException("Cannot resize the client window while the window is minimised.");

            var succeeded = User32.MoveWindow(WindowPointer, 
                windowRectangle.X, 
                windowRectangle.Y, 
                (windowRectangle.Width - clientRectangle.Width) + width, 
                (windowRectangle.Height - clientRectangle.Height) + height, redrawIfResized);
            if (!succeeded) throw new User32Exception("Unable to resize client rectangle.", Marshal.GetLastWin32Error());

            return true;
        }


        /// <summary>
        /// Takes a picture of the client area in the window. You should ensure the window is not minimised for this to return an image.
        /// </summary>
        public Bitmap TakeScreenshotOfClient()
        {
            var clientRectangle = ClientRectangle;

            var bitmap = new Bitmap(clientRectangle.Width, clientRectangle.Height);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var succeeded = User32.PrintWindow(WindowPointer, GetDeviceContext().HWnd, User32.PrintWindowFlags.PW_CLIENTONLY);
            if (!succeeded) throw new User32Exception("Unable to take a screenshot of the client area of the window.", Marshal.GetLastWin32Error());
            stopwatch.Stop();
            Console.WriteLine("Screenshot took: " + stopwatch.Elapsed.TotalMilliseconds);
            return bitmap;
        }

        /// <summary>Deletes the specified device context (DC).</summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <returns><para>If the function succeeds, the return value is nonzero.</para><para>If the function fails, the return value is zero.</para></returns>
        /// <remarks>An application must not delete a DC whose handle was obtained by calling the <c>GetDC</c> function. Instead, it must call the <c>ReleaseDC</c> function to free the DC.</remarks>
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);


        /// <summary>
        /// Send the key/s to the window. This is really a key down message followed immediately by a key up message.
        /// </summary>
        public void SendKeys(params User32.VirtualKey[] keys)
        {
            foreach (var key in keys)
            {
                User32.SendMessage(WindowPointer, User32.WindowMessage.WM_KEYDOWN, (IntPtr)key, IntPtr.Zero);
                User32.SendMessage(WindowPointer, User32.WindowMessage.WM_KEYUP, (IntPtr)key, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Gives the window focus, moving it to the foreground and preparing it to receive user input.
        /// </summary>
        public void GiveFocus()
        {
            var succeeded = User32.SetForegroundWindow(WindowPointer);
            if (!succeeded) throw new User32Exception("Unable to bring the window to the foreground", Marshal.GetLastWin32Error());
        }

        public void Dispose()
        {
            deviceContext?.Dispose();
        }
    }
}
