using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class InvalidResult<T> : AResult<T>
    {
        private readonly string[] _messages;
        public InvalidResult(params string[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Invalid;

        public override List<string> Errors => _messages.ToList() ?? new List<string> { "Invalid parameter"};

        public override T Data => default;
    }
}
