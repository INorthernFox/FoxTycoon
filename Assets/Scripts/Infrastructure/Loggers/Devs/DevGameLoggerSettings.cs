using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Loggers.Devs
{
    [CreateAssetMenu(fileName = "DevGameLoggerSettings", menuName = "Game/Dev/Logger/Settings", order = 0)]
    public class DevGameLoggerSettings : ScriptableObject
    {
        [SerializeField]
        private LogLevel _globalEnabledLevels = LogLevel.Error | LogLevel.Warning;
        [SerializeField]
        private LogSystemSettings[] _enabledSystemsType;
        private Dictionary<LogSystemType, LogLevel> _systems;

        public bool IsEnabledLevel(LogLevel level) => (_globalEnabledLevels & level) != 0;
        public bool IsEnabledSystem(LogSystemType systemType, LogLevel level)
        {
            _systems ??= BuildSystemsDictionary();
            return _systems.TryGetValue(systemType, out LogLevel levels) && (levels & level) != 0;
        }

        private Dictionary<LogSystemType, LogLevel> BuildSystemsDictionary() =>
            _enabledSystemsType.ToDictionary(entry => entry.Type, entry => entry.Level);

        private void OnValidate()
        {
            if(Application.isPlaying)
                return;

            for( int i = 0; i < _enabledSystemsType.Length; i++ )
            {
                LogSystemSettings settings = _enabledSystemsType[i];
                settings.Name = settings.Type.ToString();
                _enabledSystemsType[i] = settings;
            }
        }
    }

}