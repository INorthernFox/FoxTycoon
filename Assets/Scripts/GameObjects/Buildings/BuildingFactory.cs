using GameObjects.ResourcesSpace;
using Infrastructure.SaveServices.Interfaces;
using UnityEngine;
using Zenject;

namespace GameObjects.Buildings
{
    public class BuildingFactory : IFactory<BuildingSettings, Transform, int, ResourcesSettings, BuildingObject>
    {
        private readonly BuildingObject _prefab;
        private readonly ISaveManager _saveManagers;

        public BuildingFactory(BuildingObject prefab, ISaveManager saveManagers)
        {
            _prefab = prefab;
            _saveManagers = saveManagers;
        }

        public BuildingObject Create(BuildingSettings settings, Transform root, int levelId, ResourcesSettings resourcesSettings)
        {
            BuildingData data = new(levelId, settings.Id, settings.ResourcesType);
            _saveManagers.RegisterSaveable(data);
            Building building = new(data, resourcesSettings);

            BuildingObject buildingObject = Object.Instantiate(_prefab, root);
            buildingObject.transform.position = settings.Position;

            buildingObject.BuildingView.SetInfo(resourcesSettings.Icon, resourcesSettings.Name);
            buildingObject.BuildingView.SubscribeToCount(data.Count);
            Object.Instantiate(settings.BuildingPrefab, buildingObject.BuildingVisualContainer);

            buildingObject.SetModel(building);

            return buildingObject;
        }
    }
}