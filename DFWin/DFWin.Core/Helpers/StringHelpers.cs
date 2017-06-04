using System;

namespace DFWin.Core.Helpers
{
    public static class StringHelpers
    {
        public static string[] SplitByString(this string str, string delimiter)
        {
            return str.Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
