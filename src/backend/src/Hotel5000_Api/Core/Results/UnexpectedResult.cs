using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Core.Results
{
    public class UnexpectedResult<T> : Result<T>
    {
        public UnexpectedResult(params string[] messages) : base(messages)
        { }
        public UnexpectedResult(params Errors[] messages) : base(messages)
        { }
        public UnexpectedResult(Errors error, params string[] messages) : base(error, messages)
        { }
        public override ResultType ResultType => ResultType.Unexpected;
        public override List<string> Errors => _messages.ToList() ?? new List<string> { Enums.Errors.UNDEFINED.ToString() };
        public override T Data => default;
    }
}
