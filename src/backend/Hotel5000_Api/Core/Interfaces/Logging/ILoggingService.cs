using Core.Entities.Logging;
using Core.Enums.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Logging
{
    public interface ILoggingService
    {
        void Log(LogEntity log);
        void Log(string message, LogLevel type);
    }
}
