using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class UnauthorizedResult<T> : Result<T>
    {
        private readonly Errors[] _messages;
        public UnauthorizedResult(params Errors[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.Unauthorized;

        public override List<Errors> Errors => _messages.ToList() ?? new List<Errors> { Enums.Errors.UNAUTHORIZED };

        public override T Data => default;
    }
}
