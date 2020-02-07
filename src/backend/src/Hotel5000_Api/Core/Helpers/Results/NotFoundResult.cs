using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class NotFoundResult<T> : Result<T>
    {
        private readonly string[] _messages;
        public NotFoundResult(params string[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.NotFound;
        public override List<string> Errors => _messages.ToList() ?? new List<string> { "Not found" };
        public override T Data => default;
    }
}
