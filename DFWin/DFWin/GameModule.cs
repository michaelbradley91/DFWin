using Autofac;
using DFWin.Models;

namespace DFWin
{
    public class GameModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DwarfFortress>().AsSelf().SingleInstance();
            builder.RegisterType<WarmUpConfiguration>().AsSelf().SingleInstance();
        }
    }
}
