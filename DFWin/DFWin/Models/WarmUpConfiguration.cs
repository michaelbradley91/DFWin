using System;
using System.Reflection;
using DFWin.Core.Interfaces;

namespace DFWin.Models
{
    public class WarmUpConfiguration : IWarmUpConfiguration
    {
        public int TimeToWaitPerProcessForGoodPerformanceInMilliseconds => 500;
        public int NumberOfWarmUpProcessesToSpawn => 20;

        private readonly Lazy<string> executablePath = new Lazy<string>(() => new Uri(Assembly.GetAssembly(typeof(WarmUp.Program)).CodeBase).LocalPath);
        public string ExecutablePath => executablePath.Value;
    }
}
