using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces.Lodging.UserManagementService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Lodging
{
    public interface IUserManagementService : IApprovingDataService, IContactService, IUserService
    { 
        
    }
}
