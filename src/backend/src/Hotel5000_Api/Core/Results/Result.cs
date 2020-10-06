using Core.Enums;
using System.Collections.Generic;

namespace Core.Results
{
    public abstract class Result<T>
    {
        protected readonly int _pageCount;
        protected readonly int _allCount;
        protected readonly bool _isPaginated;
        protected readonly T _data;
        protected readonly string[] _messages;
        public Result(T data)
        {
            _pageCount = 0;
            _allCount = 0;
            _isPaginated = false;
            _data = data;
        }
        public Result(T data, int pageCount, int allCount) : this(data)
        {
            _pageCount = pageCount;
            _allCount = allCount;
            _isPaginated = true;
        }
        protected Result(params Errors[] messages)
        {
            _messages = new string[messages.Length * 2];
            for (int i = 0; i < messages.Length; i += 2)
            {
                _messages[i] = ((int)messages[i]).ToString();
                _messages[i + 1] = messages[i].ToString();
            }
        }
        protected Result(params string[] messages)
        {
            _messages = messages;
        }
        protected Result(Errors error, params string[] messages)
        {
            List<string> errorMessages = new List<string>();
            errorMessages.Add(((int)error).ToString());
            errorMessages.Add(error.ToString());
            foreach (string message in messages)
            {
                errorMessages.Add(message);
            }
            _messages = errorMessages.ToArray();
        }
        public abstract ResultType ResultType { get; }
        public abstract List<string> Errors { get; }
        public abstract T Data { get; }
        public bool IsPaginated
        {
            get
            {
                return _isPaginated;
            }
        }
        public int PageCount
        {
            get
            {
                return _pageCount;
            }
        }
        public int AllCount
        {
            get
            {
                return _allCount;
            }
        }
    }
}
