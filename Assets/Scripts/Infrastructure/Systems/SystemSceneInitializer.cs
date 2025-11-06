using Infrastructure.AudioServices.Systems;
using Infrastructure.SceneManagementServices.LoadingScreenService.Systems;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Systems
{
    public class SystemSceneInitializer
    {
        private readonly DiContainer _container;

        public SystemSceneInitializer(DiContainer container)
        {
            _container = container;
        }

        public Scene AddSystemComponents(Scene scene)
        {
            scene.CreateRoot<SystemSceneRoots>()
                .AddLoadingScreen(_container)
                .AddAudio(_container);

            return scene;
        }
    }

}