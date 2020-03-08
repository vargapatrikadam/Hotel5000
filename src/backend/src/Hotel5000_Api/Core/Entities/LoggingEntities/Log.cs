using Core.Enums.Logging;
using System;

namespace Core.Entities.LoggingEntities
{
    public class Log : BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public LogLevel Type { get; set; }
    }
}