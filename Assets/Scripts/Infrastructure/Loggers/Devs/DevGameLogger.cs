using System.Text;
using UnityEngine;
using Zenject;

namespace Infrastructure.Loggers.Devs
{
    public class DevGameLogger : IGameLogger
    {
        private readonly DevGameLoggerSettings _settings;

        [Inject]
        public DevGameLogger(DevGameLoggerSettings devGameLoggerSettings)
        {
            _settings = devGameLoggerSettings;
        }

        public void Log(string message, LogLevel level, LogSystemType systemType,  string id = null)
        {
            if(!_settings.IsEnabledLevel(level) || !_settings.IsEnabledSystem(systemType, level))
                return;

            StringBuilder result = new("[");
            result.Append(level.ToString().ToUpper());
            result.Append("] ");
            result.Append(message);
            result.Append(" ");
            result.Append(id);

            if(level == LogLevel.Error)
                Debug.LogError(result);
            else
                Debug.Log(result);
        }
    }

}