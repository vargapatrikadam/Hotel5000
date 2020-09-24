using Ardalis.Specification;
using Core.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Specifications.UserManagement
{
    public class GetApprovingData : Specification<ApprovingData>
    {
        public GetApprovingData(int? approvingDataOwnerId = null,
            int? approvingDataId = null,
            string username = null,
            string taxNumber = null,
            string identityNumber = null,
            string registrationNumber = null)
        {
            Query
                .Where(p => (!approvingDataOwnerId.HasValue || p.UserId == approvingDataOwnerId) &&
                            (!approvingDataId.HasValue || p.Id == approvingDataId) &&
                            (username == null || p.User.Username.Contains(username)) &&
                            (taxNumber == null || p.TaxNumber.Contains(taxNumber)) &&
                            (identityNumber == null || p.IdentityNumber.Contains(identityNumber)) &&
                            (registrationNumber == null || p.RegistrationNumber.Contains(registrationNumber)))
                .Include(i => i.User);
        }
    }
}
