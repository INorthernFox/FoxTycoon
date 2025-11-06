using Zenject;

namespace Infrastructure.SaveServices
{
    public class SaveInstaller : Installer<SaveInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SaveManager>().AsSingle();
            Container.BindInterfacesTo<DevSaveStorage>().AsSingle();
            Container.Bind<AutoSaver>().AsSingle();
        }
    }
}