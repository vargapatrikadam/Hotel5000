using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helpers.Results
{
    public class SuccessfulResult<T> : Result<T>
    {
        private readonly T _data;
        public SuccessfulResult(T data)
        {
            _data = data;
        }
        public override ResultType ResultType => ResultType.Ok;
        public override List<Errors> Errors => null;
        public override T Data => _data;
    }
}
