using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Lodging.UserManagementService
{
    public interface IApprovingDataService
    {
        Task<Result<bool>> AddApprovingData(ApprovingData newApprovingData, int resourceAccessorId);
        Task<Result<bool>> RemoveApprovingData(int approvingDataOwnerId, int resourceAccessorId);
        Task<Result<bool>> UpdateApprovingData(ApprovingData newApprovingData, int approvingDataOwnerId, int resourceAccessorId);
        Task<Result<ApprovingData>> GetApprovingData(int approvingDataOwnerId);
        Task<Result<IReadOnlyList<ApprovingData>>> GetAllApprovingData();
    }
}
