using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;

namespace Core.Interfaces.LodgingDomain.LodgingManagementService
{
    public interface ILodgingService
    {
        Task<Result<IReadOnlyList<Lodging>>> GetLodging(int? id = null,
            string name = null,
            LodgingTypes? lodgingType = null);

        Task<Result<bool>> UpdateLodging(Lodging newLodging,
            int oldLodgingId,
            int resourceAccessorId);

        Task<Result<bool>> RemoveLodging(int lodgingId,
            int resourceAccessorId);

        Task<Result<bool>> AddLodging(Lodging lodging, string lodgingType, int resourceAccessorId);
    }
}
