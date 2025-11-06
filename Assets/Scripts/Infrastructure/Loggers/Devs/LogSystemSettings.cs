using System;
using UnityEngine;

namespace Infrastructure.Loggers.Devs
{
    [Serializable]
    public struct LogSystemSettings
    {
        [HideInInspector] public string Name;
        public LogLevel Level;
        public LogSystemType Type;

        public LogSystemSettings(string name, LogLevel level, LogSystemType type)
        {
            Name = name;
            Level = level;
            Type = type;
        }
    }
}