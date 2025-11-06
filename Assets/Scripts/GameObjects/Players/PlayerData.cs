using System;
using System.Collections.Generic;
using System.Linq;
using GameObjects.ResourcesSpace;
using GameObjects.ResourcesSpace.Extensions;
using Infrastructure.SaveServices;
using UniRx;

namespace GameObjects.Players
{
    public class PlayerData : BaseSaveable<PlayerData.Data>
    {
        public IReadOnlyCollection<IResourcesData> Resources => _resources.Values;
        private Dictionary<GameResourcesType, ResourcesData> _resources = new();

        public IObservable<ResourceAddedInfo> ResourceAdded => _resourceAdded.AsObservable();
        private readonly Subject<ResourceAddedInfo> _resourceAdded = new();

        public IObservable<IResourcesData> NewResourceAdded => _newResourceAdded.AsObservable();
        private readonly Subject<IResourcesData> _newResourceAdded = new();
        
        public PlayerData(int levelId) =>
            Key = $"Player_{levelId}";

        public override string Key { get; }

        public void AddResource(GameResourcesType type, int added)
        {
            if(added > 0)
                SetChanges();

            if(_resources.TryGetValue(type, out ResourcesData data))
            {
                added = data.AddCount(added);
            }
            else
            {
                _resources[type] = new ResourcesData(type, added);
                _newResourceAdded.OnNext(_resources[type]);
            }

            _resourceAdded.OnNext(new ResourceAddedInfo(type, added));
        }

        protected override Data GetData() =>
            new(_resources.Values.Select(x => x.ToSave()).ToList());

        protected override void OnLoad(Data data) =>
            _resources = data.Resources.ToDictionary(x => x.Type, x => x.WithSave());

        [Serializable]
        public class Data
        {
            public readonly List<ResourcesData.Save> Resources;

            public Data(List<ResourcesData.Save> resources)
            {
                Resources = resources;
            }
        }
    }
}