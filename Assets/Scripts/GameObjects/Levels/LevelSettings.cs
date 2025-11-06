using System.Collections.Generic;
using GameObjects.Buildings;
using GameObjects.ResourcesSpace;
using UnityEngine;

namespace GameObjects.Levels
{
    [CreateAssetMenu(menuName = "Game/Levels/Settings", fileName = "LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private ResourcesDatabase _resourcesDatabase;
        [SerializeField] private BuildingSettings[] _settings;
        [SerializeField] private LevelObject _levelPrefab;
        [SerializeField] private Vector3 _playerPosition;
        [SerializeField] private int _id;

        public int Id => _id;
        public LevelObject LevelPrefab => _levelPrefab;
        public ResourcesDatabase ResourcesDatabase => _resourcesDatabase;
        public IReadOnlyList<BuildingSettings> Buildings => _settings;
        public Vector3 PlayerPosition => _playerPosition;
    }

}