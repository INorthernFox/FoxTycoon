using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.SceneManagementServices.LoadingScreenService
{
    public class LoadingScreenVisual : MonoBehaviour
    {
        [SerializeField] private Image _art;
        private LoadingScreenVisualSettings _settings;

        public bool IsOn { get; private set; }

        private Tween _tween;

        public void SetSettings(LoadingScreenVisualSettings settings) =>
            _settings = settings;

        public void PlayShowAnimation(bool fast)
        {
            _tween?.Kill();
            IsOn = true;

            gameObject.SetActive(true);

            if(fast)
            {
                RemoveFade();
                return;
            }

            SetFullFade();

            _tween = _art.DOFade(1f, _settings.FadeDuration)
                .SetEase(Ease.OutQuad).OnKill(RemoveFade);
        }

        public void PlayHideAnimation()
        {
            _tween?.Kill();
            IsOn = false;

            _tween = DOVirtual.DelayedCall(_settings.HideDelay, () =>
            {
                _art.DOFade(0f, _settings.FadeDuration)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    })
                    .OnKill(SetFullFade);
            });
        }

        private void SetFullFade()
        {
            Color color = _art.color;
            color.a = 0f;
            _art.color = color;
        }

        private void RemoveFade()
        {
            Color color = _art.color;
            color.a = 1f;
            _art.color = color;
        }
    }
}