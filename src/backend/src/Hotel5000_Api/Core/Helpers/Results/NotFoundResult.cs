using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers.Results
{
    public class NotFoundResult<T> : Result<T>
    {
        public NotFoundResult(params string[] messages) : base(messages)
        { }
        public NotFoundResult(params Errors[] messages) : base(messages)
        { }
        public NotFoundResult(Errors error, params string[] messages) : base(error, messages)
        { }
        public override ResultType ResultType => ResultType.NotFound;
        public override List<string> Errors => _messages.ToList() ?? new List<string> { Enums.Errors.NOT_FOUND.ToString() };
        public override T Data => default;
    }
}
