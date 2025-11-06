using Zenject;

namespace Infrastructure.SceneManagementServices
{
    public class SceneManagementInstaller : MonoInstaller
    {
        public SceneSettings SceneSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameSceneManager>().AsSingle();
            Container.Bind<SceneSettings>().FromInstance(SceneSettings).AsSingle();
        }
    }
}