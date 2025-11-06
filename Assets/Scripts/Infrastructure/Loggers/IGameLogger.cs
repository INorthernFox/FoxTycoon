namespace Infrastructure.Loggers
{
    public interface IGameLogger
    {
        public void Log(string message, LogLevel level, LogSystemType systemType, string id = null);
    }

}