using GameObjects.ResourcesSpace;
using Infrastructure.Loggers;
using Infrastructure.SaveServices.Interfaces;
using UnityEngine;
using Zenject;

namespace GameObjects.Buildings
{
    public class BuildingFactory : IFactory<BuildingSettings, Transform, int, ResourcesSettings, BuildingObject>
    {
        private readonly BuildingObject _prefab;
        private readonly ISaveManager _saveManagers;
        private readonly IGameLogger _logger;

        public BuildingFactory(BuildingObject prefab
            , ISaveManager saveManagers
            , IGameLogger logger)
        {
            _prefab = prefab;
            _saveManagers = saveManagers;
            _logger = logger;
        }

        public BuildingObject Create(BuildingSettings settings, Transform root, int levelId, ResourcesSettings resourcesSettings)
        {
            BuildingData data = new(levelId, settings.Id, settings.ResourcesType);
            _saveManagers.RegisterSaveable(data);
            Building building = new(data, resourcesSettings);

            BuildingObject buildingObject = Object.Instantiate(_prefab, root);
            buildingObject.transform.position = settings.Position;
            
            buildingObject.SetLogger(_logger);
            buildingObject.ValidateObject();

            buildingObject.BuildingView.SetInfo(resourcesSettings.Icon, resourcesSettings.Name);
            buildingObject.BuildingView.SubscribeToCount(data.Count);
            Object.Instantiate(settings.BuildingPrefab, buildingObject.BuildingVisualContainer);

            buildingObject.SetModel(building);

            return buildingObject;
        }
    }
}