using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers.Results
{
    public class NotFoundResult<T> : Result<T>
    {
        private readonly Errors[] _messages;
        public NotFoundResult(params Errors[] messages)
        {
            _messages = messages;
        }
        public override ResultType ResultType => ResultType.NotFound;
        public override List<Errors> Errors => _messages.ToList() ?? new List<Errors> { Enums.Errors.NOT_FOUND };
        public override T Data => default;
    }
}
