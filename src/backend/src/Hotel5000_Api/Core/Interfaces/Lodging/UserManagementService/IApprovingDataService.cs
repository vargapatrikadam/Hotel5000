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
        Task<Result<bool>> AddApprovingData(ApprovingData approvingData, int userId);
        Task<Result<bool>> RemoveApprovingData(int userId);
        Task<Result<bool>> UpdateApprovingData(ApprovingData approvingData, int userId, int approvingDataId);
        Task<Result<ApprovingData>> GetApprovingData(int userId);
        Task<Result<IReadOnlyList<ApprovingData>>> GetAllApprovingData();
    }
}
