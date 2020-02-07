using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class UnauthorizedResult<T> : AResult<T>
    {
        private readonly string[] _messages;
        public UnauthorizedResult(params string[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Unauthorized;

        public override List<string> Errors => _messages.ToList() ?? new List<string> { "Unauthorized" };

        public override T Data => default;
    }
}
