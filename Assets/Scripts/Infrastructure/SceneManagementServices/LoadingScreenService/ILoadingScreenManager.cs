using UniRx;

namespace Infrastructure.SceneManagementServices.LoadingScreenService
{
    public interface ILoadingScreenManager
    {
        IReadOnlyReactiveProperty<bool> IsAvailable { get; }
        void TryShow(bool fast = false);
        void TryHide();
        void SetVisual(LoadingScreenVisual visual);
    }
}