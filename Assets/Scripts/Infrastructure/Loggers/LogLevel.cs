using System;

namespace Infrastructure.Loggers
{
    [Flags] 
    public enum LogLevel
    {
        None    = 0,
        Error   = 1 << 0,
        Warning = 1 << 1,
        Info    = 1 << 2,
        All     = ~0    
    }
}