using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects.Buildings.Views
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _resourceName;
        [SerializeField] private TMP_Text _resourceCount;

        private readonly CompositeDisposable _disposables = new();
        
        public void SetInfo(Sprite icon, string resourceName )
        {
            _icon.sprite = icon;
            _resourceName.text = resourceName;
        }

        public void SubscribeToCount( IReadOnlyReactiveProperty<int> count )
        {
            count
                .Subscribe(c => _resourceCount.text = c.ToString())
                .AddTo(_disposables);
        }

        private void OnDestroy() =>
            _disposables.Dispose();
    }

}