using Core.Entities.Logging;
using Core.Enums.Logging;
using Core.Interfaces;
using Core.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly IAsyncRepository<LogEntity> logRepository;
        public LoggingService(IAsyncRepository<LogEntity> LogRepository)
        {
            logRepository = LogRepository;
        }

        public async Task<IReadOnlyList<LogEntity>> GetAllLogs()
        {
            return await logRepository.GetAllAsync();
        }

        public async Task Log(LogEntity log)
        {
            await logRepository.AddAsync(log);
        }

        public async Task Log(string message, LogLevel type)
        {
            await logRepository.AddAsync(new LogEntity() { Message = message, Timestamp = DateTime.Now, Type = type });
        }
    }
}
