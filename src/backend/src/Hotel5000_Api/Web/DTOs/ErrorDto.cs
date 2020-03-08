using Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Web.DTOs
{
    public class ErrorDto
    {
        public ErrorDto(params string[] messages)
        {
            Messages = messages.ToList();
        }
        public ErrorDto(ICollection<string> messages) : this(messages.ToArray())
        {
        }
        public ErrorDto(string message) : this(new string[] { message })
        {
        }
        public ErrorDto(params Errors[] errors)
        {
            Messages = new List<string>();
            foreach (Errors error in errors)
            {
                Messages.Add(((int)error).ToString());
                Messages.Add(error.ToString());
            }
        }
        public ErrorDto(Errors error) : this(new Errors[] { error })
        {

        }
        public ErrorDto(ICollection<Errors> errors) : this(errors.ToArray())
        {

        }
        public ICollection<string> Messages { get; private set; }
    }
}