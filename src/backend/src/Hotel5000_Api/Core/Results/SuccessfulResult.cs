using System.Collections.Generic;

namespace Core.Results
{
    public class SuccessfulResult<T> : Result<T>
    {
        public SuccessfulResult(T data) : base(data)
        {
        }
        public override ResultType ResultType => ResultType.Ok;
        public override List<string> Errors => null;
        public override T Data => _data;
    }
}
