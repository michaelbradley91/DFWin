using DFWin.Core.Interfaces;
using DFWin.Core.Resources.Models;

namespace DFWin.Core.Services
{
    public interface IWarmUpService
    {
        WarmUpTask BeginWarmUp();
    }

    public class WarmUpService : IWarmUpService
    {
        private readonly IInputService inputService;
        private readonly IWarmUpConfiguration configuration;

        public WarmUpService(IInputService inputService, IWarmUpConfiguration configuration)
        {
            this.inputService = inputService;
            this.configuration = configuration;
        }

        public WarmUpTask BeginWarmUp()
        {
            var task = new WarmUpTask(inputService, configuration);
            task.StartAndInitialiseWarmUpInput();
            return task;
        }
    }
}
