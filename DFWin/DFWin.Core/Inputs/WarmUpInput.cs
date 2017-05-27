using System;
using DFWin.Core.Resources.Models;

namespace DFWin.Core.Inputs
{
    public class WarmUpInput
    {
        public bool IsRunningTask => warmUpProgress != null;
        
        private readonly IWarmUpProgress warmUpProgress;

        private WarmUpInput() { }

        public WarmUpInput(IWarmUpProgress warmUpProgress)
        {
            this.warmUpProgress = warmUpProgress;
        }

        public static readonly WarmUpInput None = new WarmUpInput();

        public IWarmUpProgress GetProgress()
        {
            if (!IsRunningTask) throw new InvalidOperationException("Cannot get the progress of a warm up task when no task has been run.");

            return warmUpProgress;
        }
    }
}
