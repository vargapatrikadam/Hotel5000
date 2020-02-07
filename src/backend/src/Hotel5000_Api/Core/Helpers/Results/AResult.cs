using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helpers.Results
{
    public abstract class AResult<T>
    {
        public abstract ResultType ResultType { get;}
        public abstract List<string> Errors { get; }
        public abstract T Data { get;}
    }
}
