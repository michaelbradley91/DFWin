using System;

namespace DFWin.Core.User32Extensions.Exceptions
{
    public class User32Exception : Exception
    {
        public User32Exception(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; }
    }
}
