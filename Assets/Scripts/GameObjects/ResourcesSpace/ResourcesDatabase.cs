using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameObjects.ResourcesSpace
{
    [CreateAssetMenu(menuName = "Game/Resources/Database", fileName = "ResourcesDatabase", order = 1)]
    public class ResourcesDatabase : ScriptableObject
    {
        [SerializeField] private ResourcesSettings[] _resources;

        public IReadOnlyCollection<ResourcesSettings> AllResources => _resources;

        public bool TryGetByType(GameResourcesType type, out ResourcesSettings settings)
        {
            settings = _resources.FirstOrDefault(resource => resource.Type == type);
            return settings != null;
        }
    }

}