using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTOs
{
    public class ApprovingDataDto
    {
        public int Id { get; set; }
        public string IdentityNumber { get; set; }
        public string TaxNumber { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
