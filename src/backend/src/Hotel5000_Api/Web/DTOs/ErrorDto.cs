using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class ErrorDto
    {
        public ErrorDto(params string[] messages)
        {
            Messages = messages.ToList();
        }
        public ErrorDto(ICollection<string> messages)
        {
            Messages = messages.ToList();
        }
        public ErrorDto(string message)
        {
            Messages = new List<string> { message };
        }
        public ICollection<string> Messages { get; private set; }
    }
}