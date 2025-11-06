using Constants;
using GameObjects.Buildings;
using GameObjects.Players;
using GameObjects.ResourcesSpace;
using Infrastructure.InputSpace;
using Infrastructure.InputSpace.Camera;
using Infrastructure.InputSpace.Raycasters;
using Infrastructure.Loggers;
using UnityEngine;
using Zenject;

namespace GameObjects.Levels
{
    public class LevelsFactory : IFactory<GameSceneRoot, LevelSettings, LevelObject>
    {
        private readonly BuildingFactory _buildingFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly MainCamera _mainCamera;
        private readonly IInputManager _inputManager;
        private readonly IGameLogger _logger;

        public LevelsFactory(
            BuildingFactory buildingFactory,
            PlayerFactory playerFactory,
            MainCamera mainCamera,
            IInputManager inputManager,
            IGameLogger logger)
        {
            _buildingFactory = buildingFactory;
            _playerFactory = playerFactory;
            _mainCamera = mainCamera;
            _inputManager = inputManager;
            _logger = logger;
        }

        public LevelObject Create(GameSceneRoot root, LevelSettings settings)
        {
            LevelObject levelObject = Object.Instantiate(settings.LevelPrefab, root.transform);
            MainCamera camera = CreateCamera(levelObject);
            RayHitHandler handler = CreateRayHitHandler(camera);

            root.SetLevelObject(levelObject);
            CreateBuildings(settings, levelObject);
            CreatePlayer(settings, levelObject, handler);
            levelObject.UpdateNavMesh();
            return levelObject;
        }

        private MainCamera CreateCamera(LevelObject levelObject)
        {
            MainCamera camera = Object.Instantiate(_mainCamera, levelObject.transform);
            camera.SetInputManager(_inputManager);
            levelObject.AddCamera(camera);

            CreateRayHitHandler(camera);

            return camera;
        }

        private static RayHitHandler CreateRayHitHandler(MainCamera camera)
        {
            RayHitHandler handler = new RayHitHandler();
            handler.SubscribeToRayHits(camera.GroundRaycaster.RayHits);
            return handler;
        }

        private void CreatePlayer(LevelSettings settings,
            LevelObject levelObject, RayHitHandler handler)
        {
            PlayerObject playerObject = _playerFactory.Create(levelObject.transform,
                settings.Id, settings.PlayerPosition, settings.ResourcesDatabase);

            playerObject.Mover.SubscribeToMoveRequest(handler.MoveRequest);
            levelObject.AddPlayer(playerObject);
        }

        private void CreateBuildings(LevelSettings settings, LevelObject levelObject)
        {
            foreach(BuildingSettings buildingSettings in settings.Buildings)
            {
                if(!settings.ResourcesDatabase.TryGetByType(buildingSettings.ResourcesType, out ResourcesSettings resourcesSettings))
                {
                    _logger.Log($"No settings for resource {buildingSettings.ResourcesType} "
                                + $"when building house {buildingSettings.Id}",
                        LogLevel.Error,
                        LogSystemType.Scene,
                        LogIds.LevelsFactory.CreateBuildings);

                    continue;
                }

                BuildingObject building = _buildingFactory.Create(buildingSettings, levelObject.transform, settings.Id, resourcesSettings);
                levelObject.AddBuilding(building);
            }
        }
    }
}