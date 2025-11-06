using Infrastructure.Loggers.Devs;
using Zenject;

namespace Infrastructure.Loggers
{
    public class LogInstaller : MonoInstaller
    {
        public DevGameLoggerSettings _devGameLoggerSettings;

        public override void InstallBindings()
        {
            Container.Bind<DevGameLoggerSettings>().FromInstance(_devGameLoggerSettings).AsSingle();
            Container.BindInterfacesTo<DevGameLogger>().AsSingle();
        }
    }
}