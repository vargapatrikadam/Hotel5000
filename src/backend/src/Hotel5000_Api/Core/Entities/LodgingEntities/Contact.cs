using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Contact : BaseEntity
    {
        public string MobileNumber { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}