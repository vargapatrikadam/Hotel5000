using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class InvalidResult<T> : Result<T>
    {
        private readonly Errors[] _messages;
        public InvalidResult(params Errors[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Invalid;

        public override List<Errors> Errors => _messages.ToList() ?? new List<Errors> { Enums.Errors.INVALID_PARAMETER };

        public override T Data => default;
    }
}
