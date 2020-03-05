using Core.Enums;
using System.Collections.Generic;

namespace Core.Helpers.Results
{
    public abstract class Result<T>
    {
        protected readonly T _data;
        protected readonly string[] _messages;
        public Result(T data)
        {
            _data = data;
        }
        public Result(params Errors[] messages)
        {
            _messages = new string[messages.Length * 2];
            for (int i = 0; i < messages.Length; i += 2)
            {
                _messages[i] = ((int)messages[i]).ToString();
                _messages[i + 1] = messages[i].ToString();
            }
        }
        public Result(params string[] messages)
        {
            _messages = messages;
        }
        public Result(Errors error, params string[] messages)
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
    }
}
