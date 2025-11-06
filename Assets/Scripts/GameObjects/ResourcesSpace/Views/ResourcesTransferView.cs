using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects.ResourcesSpace.Views
{

    public class ResourcesTransferView : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _resourceCount;

        [Header("Timing")]
        [SerializeField] private float _visibleDelay = 1.5f;

        [Header("Visuals")]
        [SerializeField] private string _prefix = "+";
        private CompositeDisposable _disposables = new();

        private int _sum = 0;
        private GameResourcesType _lastResourceType = GameResourcesType.None;

        private ResourcesDatabase _resourcesDatabase;
        private IDisposable _timerStream;

        private void LateUpdate() =>
            transform.rotation = Quaternion.Euler(30, 0, 0);

        public void SetResourcesDatabase(ResourcesDatabase database) =>
            _resourcesDatabase = database;
        private void OnDestroy()
        {
            _disposables?.Dispose();
            _disposables = null;
        }

        public void Add(ResourceAddedInfo info)
        {
            if(info.Count == 0)
                return;

            if(_lastResourceType != info.Type)
            {
                _sum = 0;
                _lastResourceType = info.Type;
            }

            SetIcon(info.Type);

            if(!gameObject.activeSelf)
                gameObject.SetActive(true);

            _sum += info.Count;

            StartTimer();

            _resourceCount.text = $"{_prefix}{_sum}";
        }

        private void StartTimer()
        {
            _timerStream?.Dispose();
            _timerStream = Observable
                .Timer(TimeSpan.FromSeconds(_visibleDelay))
                .Subscribe(_ =>
                {
                    _sum = 0;
                    gameObject.SetActive(false);
                });
        }

        private void SetIcon(GameResourcesType resourcesType)
        {
            if(!_resourcesDatabase.TryGetByType(resourcesType, out ResourcesSettings resources))
            {
                _icon.sprite = null;
                return;
            }
            _icon.sprite = resources.Icon;
        }

        public void AddStream(IDisposable resourcesTransferStream) =>
            _disposables.Add(resourcesTransferStream);
    }
}