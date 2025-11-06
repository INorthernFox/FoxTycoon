using UnityEngine;
using Zenject;

namespace Infrastructure.SceneManagementServices.LoadingScreenService.Systems
{
    public class LoadingScreenFactory : IFactory<LoadingScreenRoot,LoadingScreenVisual>
    {
        private readonly ILoadingScreenManager _manager;
        private readonly LoadingScreenVisual _prefab;
        private readonly LoadingScreenVisualSettings _settings;
        
        public LoadingScreenFactory(
            ILoadingScreenManager manager,
            LoadingScreenVisual prefab, 
            LoadingScreenVisualSettings settings)
        {
            _manager = manager;
            _prefab = prefab;
            _settings = settings;
        }

        public LoadingScreenVisual Create(LoadingScreenRoot root) 
        {
            LoadingScreenVisual visual= Object.Instantiate(_prefab, root.transform);
            visual.SetSettings(_settings);
            _manager.SetVisual(visual);
            return visual;
        }
    }
}