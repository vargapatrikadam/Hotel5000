using Core.Entities.Logging;
using Core.Enums.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Logging
{
    public interface ILoggingService
    {
        void Log(Log log);
        void Log(string message, LogLevel type);
        Task<IReadOnlyList<Log>> GetAllLogs();
        Task<IReadOnlyList<Log>> GetLogsByLevel(LogLevel type);
    }
}