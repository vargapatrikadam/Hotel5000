using Core.Entities.Logging;
using Core.Enums.Logging;
using Core.Interfaces;
using Core.Interfaces.Logging;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly IAsyncRepository<Log> logRepository;
        public LoggingService(IAsyncRepository<Log> LogRepository)
        {
            logRepository = LogRepository;
        }

        public async Task<IReadOnlyList<Log>> GetAllLogs()
        {
            return await logRepository.GetAllAsync();
        }

        public async Task<IReadOnlyList<Log>> GetLogsByLevel(LogLevel type)
        {
            return await logRepository.GetAsync(new Specification<Log>().ApplyFilter(p => p.Type == type));
        }

        public async Task Log(Log log)
        {
            await logRepository.AddAsync(log);
        }

        public async Task Log(string message, LogLevel type)
        {
            await logRepository.AddAsync(new Log() { Message = message, Timestamp = DateTime.Now, Type = type });
        }
    }
}
