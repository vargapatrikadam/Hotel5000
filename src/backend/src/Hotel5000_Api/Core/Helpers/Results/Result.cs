using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helpers.Results
{
    public abstract class Result<T>
    {
        public abstract ResultType ResultType { get;}
        public abstract List<Errors> Errors { get; }
        public abstract T Data { get;}
    }
}
