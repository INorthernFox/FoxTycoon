using System;
using System.Collections.Generic;
using GameObjects.ResourcesSpace;
using UniRx;
using UnityEngine;

namespace GameObjects.Players
{
    public class PlayerResourcesView : MonoBehaviour
    {
        [SerializeField] private RectTransform _root;
        [SerializeField] private PlayerResourcesItemView _prefab;

        private readonly CompositeDisposable _disposables = new();
        private ResourcesDatabase _resourcesDatabase;

        private readonly Dictionary<GameResourcesType, PlayerResourcesItemView> _resources = new();

        public void SetResourcesDatabase(ResourcesDatabase database) =>
            _resourcesDatabase = database;

        public void AddStream(IDisposable disposable) =>
            _disposables.Add(disposable);

        public void Add(IResourcesData info)
        {
            if(_resources.ContainsKey(info.Type) || !_resourcesDatabase.TryGetByType(info.Type, out ResourcesSettings settings))
                return;

            PlayerResourcesItemView item = Instantiate(_prefab, _root);
            item.SetIcon(settings.Icon);
            info.Count
                .Subscribe(x => item.SetCount(x))
                .AddTo(_disposables);

            _resources.Add(info.Type, item);
        }
    }
}