using System;
using System.Collections.Generic;

namespace DFWin.Helpers
{
    public static class ExceptionHelpers
    {
        /// <summary>
        /// Runs all of the actions in order, only throwing exceptions once all have run.
        /// If multiple actions throw exceptions, this will throw an aggregate exception containing them all.
        /// If only one action throws an exception, just that exception is thrown.
        /// </summary>
        /// <param name="actions"></param>
        public static void TryAll(params Action[] actions)
        {
            var exceptions = new List<Exception>();
            foreach (var action in actions)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (exceptions.Count == 0) return;
            if (exceptions.Count == 1) throw exceptions[0];
            throw new AggregateException(exceptions);
        }
    }
}
