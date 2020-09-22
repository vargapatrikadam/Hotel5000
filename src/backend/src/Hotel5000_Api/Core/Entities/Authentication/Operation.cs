using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Authentication
{
    public class Operation : BaseEntity
    {
        public Operation()
        {

        }
        public Operation(Type action)
        {
            this.Action = action;
        }
        public Type Action { get; set; }
        public enum Type
        {
            READ,
            CREATE,
            UPDATE,
            DELETE
        }
        public ICollection<Rule> Rules { get; set; }

    }
}
