using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Authentication
{
    public class Rule : BaseEntity
    {
        public BaseRole Role { get; set; }
        public Operation Operation { get; set; }
        public Entity Entity { get; set; }
        public bool IsAllowed { get; set; }
        public int Role_Id { get; set; }
        public int Operation_Id { get; set; }
        public int Entity_Id { get; set; }
    }
}
