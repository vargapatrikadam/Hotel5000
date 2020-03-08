using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers.Results
{
    public class ConflictResult<T> : Result<T>
    {
        public ConflictResult(params string[] messages) : base(messages)
        { }
        public ConflictResult(params Errors[] messages) : base(messages)
        { }
        public ConflictResult(Errors error, params string[] messages) : base(error, messages)
        { }
        public override ResultType ResultType => ResultType.Conflict;

        public override List<string> Errors => _messages.ToList() ?? new List<string> { Enums.Errors.DATA_UNIQUENESS_CONFLICT.ToString() };

        public override T Data => default;
    }
}
