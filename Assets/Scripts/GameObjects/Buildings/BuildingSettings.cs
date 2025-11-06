using System;
using GameObjects.ResourcesSpace;
using UnityEngine;

namespace GameObjects.Buildings
{
    [CreateAssetMenu(menuName = "Game/Building/Settings", fileName = "BuildingSettings", order = 0)]
    public class BuildingSettings : ScriptableObject
    {
        [SerializeField] private GameObject _buildingPrefab;
        [SerializeField] private GameResourcesType _resourcesType;
        [SerializeField] private Vector3 _position;
        [SerializeField] private string _id;
        
        public GameObject BuildingPrefab => _buildingPrefab;
        public GameResourcesType ResourcesType => _resourcesType;
        public Vector3 Position => _position;
        public  string Id => _id;

        private void OnValidate()
        {
            if(string.IsNullOrEmpty(_id))
                _id = Guid.NewGuid().ToString();
        }
    }
}