using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers.Results
{
    public class ConflictResult<T> : Result<T>
    {
        private readonly Errors[] _messages;
        public ConflictResult(params Errors[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Conflict;

        public override List<Errors> Errors => _messages.ToList() ?? new List<Errors> { Enums.Errors.DATA_UNIQUENESS_CONFLICT };

        public override T Data => default;
    }
}
