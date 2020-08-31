using Ardalis.Specification;
using Core.Entities.LoggingEntities;
using Core.Enums.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Logging
{
    public class GetLogsByLevel : Specification<Log>
    {
        public GetLogsByLevel(LogLevel level)
        {
            Query
                .Where(p => p.Type == level);
        }
    }
}
