using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers.Results
{
    public class UnauthorizedResult<T> : Result<T>
    {
        public UnauthorizedResult(params string[] messages) : base(messages)
        { }
        public UnauthorizedResult(params Errors[] messages) : base(messages)
        { }
        public UnauthorizedResult(Errors error, params string[] messages) : base(error, messages)
        { }
        public override ResultType ResultType => ResultType.Unauthorized;

        public override List<string> Errors => _messages.ToList() ?? new List<string> { Enums.Errors.UNAUTHORIZED.ToString() };

        public override T Data => default;
    }
}
