using DFWin.Core.Interfaces;
using DFWin.Core.Models;

namespace DFWin.Core.Services
{
    public interface IWarmUpService
    {
        WarmUpTask BeginWarmUp();
    }

    public class WarmUpService : IWarmUpService
    {
        private readonly IWarmUpConfiguration configuration;

        public WarmUpService(IWarmUpConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public WarmUpTask BeginWarmUp()
        {
            var task = new WarmUpTask(configuration);
            task.Start();
            return task;
        }
    }
}
