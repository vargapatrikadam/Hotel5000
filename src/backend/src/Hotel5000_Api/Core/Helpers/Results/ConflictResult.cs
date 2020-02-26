using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class ConflictResult<T> : Result<T>
    {
        private readonly string[] _messages;
        public ConflictResult(params string[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Conflict;

        public override List<string> Errors => _messages.ToList() ?? new List<string> { "Data not unique" };

        public override T Data => default;
    }
}
