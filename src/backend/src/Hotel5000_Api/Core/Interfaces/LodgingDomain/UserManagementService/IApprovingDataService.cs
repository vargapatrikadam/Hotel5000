using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain.UserManagementService
{
    public interface IApprovingDataService
    {
        Task<Result<bool>> AddApprovingData(ApprovingData newApprovingData);
        Task<Result<bool>> RemoveApprovingData(int approvingDataOwnerId);
        Task<Result<bool>> UpdateApprovingData(ApprovingData newApprovingData, int approvingDataId);
        Task<Result<IReadOnlyList<ApprovingData>>> GetApprovingData(int? approvingDataOwnerId = null,
            int? approvingDataId = null,
            string username = null,
            string taxNumber = null,
            string identityNumber = null,
            string registrationNumber = null);
    }
}
