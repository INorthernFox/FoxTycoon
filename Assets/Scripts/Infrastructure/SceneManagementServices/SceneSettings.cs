using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.SceneManagementServices
{
    [CreateAssetMenu(fileName = "SceneSettings", menuName = "Game/Scenes/Settings", order = 0)]
    public class SceneSettings : ScriptableObject
    {
        [SerializeField] private List<SceneItem> _scenes = new();

        private Dictionary<SceneType, int> _idToBuild;

        public int BuildIndexOf(SceneType type)
        {
            _idToBuild ??= BuildMap();
            return _idToBuild.GetValueOrDefault(type, -1);
        }

        private Dictionary<SceneType, int> BuildMap()
        {
            var map = new Dictionary<SceneType, int>(_scenes.Count);
            foreach(var s in _scenes)
            {
                map[s.Type] = s.BuildIndex;
            }
            return map;
        }
    }

    [Serializable]
    public struct SceneItem
    {
        public string Name;
        public int BuildIndex;
        public SceneType Type;

#if UNITY_EDITOR
        public SceneAsset Screen;
#endif
    }

}