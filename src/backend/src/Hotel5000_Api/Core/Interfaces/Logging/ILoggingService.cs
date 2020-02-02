using Core.Entities.LoggingEntities;
using Core.Enums.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Logging
{
    public interface ILoggingService
    {
        Task Log(Log log);
        Task Log(string message, LogLevel type);
        Task<IReadOnlyList<Log>> GetAllLogs();
        Task<IReadOnlyList<Log>> GetLogsByLevel(LogLevel type);
    }
}