using Zenject;

namespace Infrastructure.GameSystemsInitializers
{
    public class SystemsInitializerInstaller : Installer<SystemsInitializerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameSystemsInitializer >().AsSingle(); 
        }
    }
}