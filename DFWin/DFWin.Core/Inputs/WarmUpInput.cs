using System;
using DFWin.Core.Resources.Models;

namespace DFWin.Core.Inputs
{
    public class WarmUpInput
    {
        public bool IsRunningTask => warmUpTask != null;

        private readonly WarmUpTask warmUpTask;

        private WarmUpInput() { }

        public WarmUpInput(WarmUpTask warmUpTask)
        {
            this.warmUpTask = warmUpTask;
        }

        public static WarmUpInput None => new WarmUpInput();

        public WarmUpProgress GetProgress()
        {
            if (!IsRunningTask) throw new InvalidOperationException("Cannot get the progress of a warm up task when no task has been run.");

            return warmUpTask.Progress;
        }
    }
}
