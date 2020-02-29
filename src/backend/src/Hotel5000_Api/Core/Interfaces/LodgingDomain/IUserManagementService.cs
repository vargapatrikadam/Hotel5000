using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces.LodgingDomain.UserManagementService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain
{
    public interface IUserManagementService : IApprovingDataService, IContactService, IUserService
    { 
        
    }
}
