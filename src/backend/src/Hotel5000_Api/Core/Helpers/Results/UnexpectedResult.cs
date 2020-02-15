using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class UnexpectedResult<T> : Result<T>
    {
        private readonly string[] _messages;
        public UnexpectedResult(params string[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Unexpected;
        public override List<string> Errors => _messages.ToList() ?? new List<string> { "Unexpected error" };
        public override T Data => default;
    }
}
