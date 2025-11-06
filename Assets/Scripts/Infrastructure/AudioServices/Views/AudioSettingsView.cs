using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.AudioServices.Views
{
    public class AudioSettingsView : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Slider _volumeSlider;        
        [SerializeField] private GameObject _volumeWindow;
        [SerializeField] private TMP_Text _volumeText;

        private IAudioManager _manager;
        private readonly CompositeDisposable _disposables= new();
        private bool _suppressSlider; 

        public void Initialization(IAudioManager manager)
        {
            _manager = manager;
            Initialization();
        }

        private void Initialization()
        {
            _settingsButton.onClick
                .AsObservable()
                .Subscribe(_ => _volumeWindow.SetActive(!_volumeWindow.activeSelf))
                .AddTo(_disposables);

            _manager.Data.Volume
                .Subscribe(percent  =>
                {
                    percent = Mathf.Clamp(percent, 0f, 1f);

                    _volumeSlider.value = percent;
                    _suppressSlider = false;

                    UpdateText(percent);
                })
                .AddTo(_disposables);

            _volumeSlider
                .OnValueChangedAsObservable()
                .Where(_ => !_suppressSlider)
                .DistinctUntilChanged()
                .Subscribe(percent =>
                {
                    _manager.Data.ChangeVolume(percent);
                    UpdateText(percent);
                })
                .AddTo(_disposables);
        }
        
        private void UpdateText(float percent) =>
            _volumeText.text = ((int)(percent*100)).ToString();

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}