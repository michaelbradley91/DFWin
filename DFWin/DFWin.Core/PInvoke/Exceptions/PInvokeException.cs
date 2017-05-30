using System;

namespace DFWin.Core.PInvoke.Exceptions
{
    public class PInvokeException : Exception
    {
        public PInvokeException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; }
    }
}
