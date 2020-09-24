using Ardalis.Specification;
using Core.Entities.Logging;
using Core.Enums.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.Specifications
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
