using GameObjects.Buildings.Views;
using Infrastructure.Loggers;
using UnityEngine;

namespace GameObjects.Buildings
{
    public class BuildingObject : MonoBehaviour
    {
        [SerializeField] private BuildingView _buildingView;
        [SerializeField] private Transform _buildingVisualContainer;
        [SerializeField] private BuildingCollectTrigger _collectTrigger;

        private Building _building;
        private IGameLogger _logger;

        public Transform BuildingVisualContainer => _buildingVisualContainer;
        public BuildingView BuildingView => _buildingView;
        public BuildingCollectTrigger CollectTrigger => _collectTrigger;

        public void SetLogger(IGameLogger logger) =>
            _logger = logger;

        public void ValidateObject()
        {
            if(_buildingView == null)
                _logger.Log($"[BuildingObject] BuildingView is not assigned on {gameObject.name}", LogLevel.Error, LogSystemType.Core);
            if(_buildingVisualContainer == null)
                _logger.Log($"[BuildingObject] BuildingVisualContainer is not assigned on {gameObject.name}", LogLevel.Error, LogSystemType.Core);
            if(_collectTrigger == null)
                _logger.Log($"[BuildingObject] BuildingCollectTrigger is not assigned on {gameObject.name}", LogLevel.Error, LogSystemType.Core);
        }

        public void SetModel(Building building)
        {
            _building = building;

            if(_collectTrigger != null)
                _collectTrigger.SetModel(building);
        }

        private void Start()
        {
            if(_building != null)
                _building.StartProduction();
        }

        private void OnDestroy()
        {
            if(_building != null)
                _building.Dispose();
        }
    }

}