using System.Collections.Generic;
using UnityEngine;

namespace GameObjects.Levels
{
    [CreateAssetMenu(menuName = "Game/Levels/Database ", fileName = "LevelDatabase ", order = 0)]
    public class LevelsDatabase : ScriptableObject
    {
        [SerializeField] private LevelSettings[] _settings;

        public IReadOnlyList<LevelSettings> Settings => _settings;
    }
}