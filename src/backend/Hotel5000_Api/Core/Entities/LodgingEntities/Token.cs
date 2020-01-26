using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Token : BaseEntity
    {
        public string RefreshToken { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
