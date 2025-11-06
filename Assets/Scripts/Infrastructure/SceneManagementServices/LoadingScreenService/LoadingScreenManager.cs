using Infrastructure.Loggers;
using UniRx;

namespace Infrastructure.SceneManagementServices.LoadingScreenService
{
    public  class LoadingScreenManager : ILoadingScreenManager
    {
        private readonly IGameLogger _logger;
        private LoadingScreenVisual _visual;

        private readonly ReactiveProperty<bool> _isAvailable = new(false);
        public IReadOnlyReactiveProperty<bool> IsAvailable => _isAvailable;

        private int _currentRequestCount;

        public LoadingScreenManager(IGameLogger logger)
        {
            _logger = logger;
        }
        
        public void SetVisual(LoadingScreenVisual visual)
        {
            if(visual == null)
                return;

            _visual = visual;
            _isAvailable.Value = true;

            if(_currentRequestCount > 0 && !_visual.IsOn)
                _visual.PlayShowAnimation( true);
        }

        public void TryShow(bool fast = false)
        {
            if(!_isAvailable.Value)
                return;

            _logger.Log($"Start Show LoadingScreen count={_currentRequestCount}", LogLevel.Info, LogSystemType.Scene);
            _currentRequestCount++;

            if(!_visual.IsOn)
                _visual.PlayShowAnimation(fast);
        }

        public void TryHide()
        {
            if(!_isAvailable.Value || _currentRequestCount <= 0)
                return;
            
            _currentRequestCount--;
            _logger.Log($"Start Hide LoadingScreen count={_currentRequestCount}", LogLevel.Info, LogSystemType.Scene);

            if(_visual.IsOn)
                _visual.PlayHideAnimation();
        }
    }
}