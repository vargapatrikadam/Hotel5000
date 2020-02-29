using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain.LodgingManagementService
{
    public interface IRoomService
    {
        Task<Result<IReadOnlyList<Room>>> GetRoom(int? id = null,
            int? adultCapacity = null,
            int? childrenCapacity = null,
            double ? priceMin = null,
            double? priceMax = null);

        Task<Result<bool>> UpdateRoom(Room newRoom,
            int oldRoomId,
            int resourceAccessorId);

        Task<Result<bool>> RemoveRoom(int roomId,
            int resourceAccessorId);

        Task<Result<bool>> AddRoom(Room room, int resourceAccessorId);
    }
}
