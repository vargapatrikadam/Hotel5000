using Core.Entities.Logging;
using Core.Enums.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Logging
{
    public class GetLogsByLevelSpecification : BaseSpecification<Log>
    {
        public GetLogsByLevelSpecification(LogLevel level) 
            :base (p => p.Type == level)
        {

        }
    }
}
