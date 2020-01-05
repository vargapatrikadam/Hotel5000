using Core.Enums.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Logging
{
    public class LogEntity : BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public LogLevel Type { get; set; }
    }
}
