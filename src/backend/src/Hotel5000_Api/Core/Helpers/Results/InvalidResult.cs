using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers.Results
{
    public class InvalidResult<T> : Result<T>
    {
        public InvalidResult(params string[] messages) : base(messages)
        { }
        public InvalidResult(params Errors[] messages) : base(messages)
        { }
        public InvalidResult(Errors error, params string[] messages) : base(error, messages)
        { }
        public override ResultType ResultType => ResultType.Invalid;

        public override List<string> Errors => _messages.ToList() ?? new List<string> { Enums.Errors.INVALID_PARAMETER.ToString() };

        public override T Data => default;
    }
}
