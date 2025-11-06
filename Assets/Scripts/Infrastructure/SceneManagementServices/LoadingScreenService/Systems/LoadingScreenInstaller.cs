using Zenject;

namespace Infrastructure.SceneManagementServices.LoadingScreenService.Systems
{
    public class LoadingScreenInstaller : MonoInstaller
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        public LoadingScreenVisual PrefabPC;
#endif
#if UNITY_EDITOR || UNITY_ANDROID
        public LoadingScreenVisual PrefabAndroid;
#endif

        public LoadingScreenVisualSettings LoadingVisualSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LoadingScreenManager>().AsSingle();
            Container.Bind<LoadingScreenFactory>().AsSingle();
            Container.Bind<LoadingScreenVisualSettings>().FromInstance(LoadingVisualSettings).AsSingle();

#if UNITY_STANDALONE
            Container.Bind<LoadingScreenVisual>().FromInstance(PrefabPC).AsSingle();
#elif UNITY_ANDROID
            Container.Bind<LoadingScreenVisual>().FromInstance(PrefabAndroid).AsSingle();
#endif
        }
    }
}