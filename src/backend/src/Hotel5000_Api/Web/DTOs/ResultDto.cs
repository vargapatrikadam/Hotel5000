using Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class ResultDto<TResult>
    {
        public ResultDto(TResult result)
        {
            Result = result;
            PageCount = 0;
            AllCount = 0;
            IsPaginated = false;
        }
        public ResultDto(TResult result, int pageCount, int allCount, bool isPaginated)
        {
            Result = result;
            PageCount = pageCount;
            AllCount = allCount;
            IsPaginated = isPaginated;
        }
        public int PageCount { get; }
        public int AllCount { get;}
        public bool IsPaginated { get; }
        public TResult Result { get; set; }
    }
}
