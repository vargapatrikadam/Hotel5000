using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class UnexpectedResult<T> : Result<T>
    {
        private readonly Errors[] _messages;
        public UnexpectedResult(params Errors[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Unexpected;
        public override List<Errors> Errors => _messages.ToList() ?? new List<Errors> { Enums.Errors.UNDEFINED };
        public override T Data => default;
    }
}
