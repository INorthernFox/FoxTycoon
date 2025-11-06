using System;
using GameObjects.ResourcesSpace;
using Infrastructure.SaveServices;
using UniRx;

namespace GameObjects.Buildings
{
    public class BuildingData : BaseSaveable<BuildingData.Data>
    {
        private readonly ResourcesData _resourcesData;

        public IReadOnlyReactiveProperty<int> Count => _resourcesData.Count;
        public GameResourcesType ResourcesType => _resourcesData.Type;

        public BuildingData(int levelId, string buildingId, GameResourcesType resourcesType)
        {
            _resourcesData = new ResourcesData(resourcesType, 0);
            Key = $"b_{levelId}_{buildingId}_{resourcesType}";
        }

        public void AddCount(int count)
        {
            SetChanges();
            _resourcesData.AddCount(count);
        }

        public int RemoveCount(int count)
        {
            SetChanges();
            return _resourcesData.RemoveCount(count);
        }

        public override string Key { get; }

        protected override Data GetData() =>
            new(_resourcesData.Count.Value);

        protected override void OnLoad(Data data) =>
            _resourcesData.Initialize(data.Count);

        [Serializable]
        public struct Data
        {
            public readonly int Count;

            public Data(int count)
            {
                Count = count;
            }
        }
    }
}