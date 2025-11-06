using System.Collections.Generic;
using GameObjects.Buildings;
using GameObjects.Players;
using Infrastructure.InputSpace.Camera;
using Unity.AI.Navigation;
using UnityEngine;

namespace GameObjects.Levels
{
    public class LevelObject : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _surface;

        private readonly List<BuildingObject> _buildings = new();
        private PlayerObject _playerObject;
        private MainCamera _mainCamera;

        public void UpdateNavMesh() =>
            _surface.BuildNavMesh();

        public void AddBuilding(BuildingObject building) =>
            _buildings .Add(building);
        
        public void AddPlayer(PlayerObject playerObject) =>
            _playerObject = playerObject;
        
        public void AddCamera(MainCamera mainCamera) =>
            _mainCamera = mainCamera;
    }
}