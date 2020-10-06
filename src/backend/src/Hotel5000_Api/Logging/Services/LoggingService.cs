using Core.Entities.Logging;
using Core.Enums.Logging;
using Core.Interfaces;
using Core.Interfaces.Logging;
using Logging.Specifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Logging.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly IAsyncRepository<Log> _logRepository;
        private readonly ConcurrentQueue<Log> _logs;
        private readonly BackgroundWorker _writer;
        public LoggingService(IAsyncRepository<Log> logRepository)
        {
            _logRepository = logRepository;
            _logs = new ConcurrentQueue<Log>();
            _writer = new BackgroundWorker();
            _writer.WorkerSupportsCancellation = false;
            _writer.DoWork += new DoWorkEventHandler(WriteToRepository);
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
        public void Log(Log log)
        {
            _logs.Enqueue(log);
            if (!_writer.IsBusy)
                _writer.RunWorkerAsync();
        }
        public void Log(string message, LogLevel type)
        {
            Log log = new Log()
            {
                Message = message,
                Type = type
            };
            Log(log);
        }
        private void WriteToRepository(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Log log = null;
                if (!_logs.TryDequeue(out log))
                    return;
                else
                    Task.WaitAll(Task.Run(async () => await _logRepository.AddAsync(log)));
            }
        }
    }
}