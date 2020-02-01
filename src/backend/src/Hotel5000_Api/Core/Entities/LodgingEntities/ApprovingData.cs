using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class ApprovingData : BaseEntity
    {
        public string IdentityNumber { get; set; }
        public string TaxNumber { get; set; }
        public string RegistrationNumber { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
