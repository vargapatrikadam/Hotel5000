using Core.Entities.Logging;
using Core.Enums.Logging;
using Core.Interfaces;
using Core.Interfaces.Logging;
using Logging.Specifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logging.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly IAsyncRepository<Log> _logRepository;

        public LoggingService(IAsyncRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<IReadOnlyList<Log>> GetAllLogs()
        {
            return await _logRepository.GetAllAsync();
        }

        public async Task<IReadOnlyList<Log>> GetLogsByLevel(LogLevel type)
        {
            var specification = new GetLogsByLevel(type);
            return await _logRepository.GetAsync(specification);
        }

        public async Task Log(Log log)
        {
            await _logRepository.AddAsync(log);
        }

        public async Task Log(string message, LogLevel type)
        {
            await _logRepository.AddAsync(new Log() { Message = message, Timestamp = DateTime.Now, Type = type });
        }
    }
}