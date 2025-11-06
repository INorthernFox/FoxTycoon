using Infrastructure.Systems;
using Zenject;

namespace Infrastructure.SceneManagementServices.LoadingScreenService.Systems
{
    public static class LoadingScreenInitializer
    {
        public static SystemSceneRoots AddLoadingScreen(this SystemSceneRoots systemSceneRoot, DiContainer container)
        {
            LoadingScreenFactory factory = container.Resolve<LoadingScreenFactory>();
            LoadingScreenRoot root = Extensions.Create<LoadingScreenRoot>(systemSceneRoot.transform);
            LoadingScreenVisual visual = factory.Create(root);
            return systemSceneRoot;
        }
    }
}