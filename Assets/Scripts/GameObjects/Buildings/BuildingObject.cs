using GameObjects.Buildings.Views;
using UnityEngine;

namespace GameObjects.Buildings
{
    public class BuildingObject : MonoBehaviour
    {
        [SerializeField] private BuildingView _buildingView;
        [SerializeField] private Transform _buildingVisualContainer;
        [SerializeField] private BuildingCollectTrigger _collectTrigger;
        
        private Building _building;

        public Transform BuildingVisualContainer => _buildingVisualContainer;
        public BuildingView BuildingView => _buildingView;
        public BuildingCollectTrigger CollectTrigger => _collectTrigger;

        public void SetModel(Building building)
        {
            _building = building;
            _collectTrigger.SetModel(building);
        }

        private void Start() =>
            _building.StartProduction();

        private void OnDestroy() =>
            _building.Dispose();
    }

}