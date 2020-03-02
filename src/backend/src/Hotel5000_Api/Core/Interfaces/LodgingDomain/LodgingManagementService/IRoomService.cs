using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain.LodgingManagementService
{
    public interface IRoomService
    {
        Task<Result<IReadOnlyList<Room>>> GetRoom(int? id = null,
            int? lodgingId = null,
            int? adultCapacity = null,
            int? childrenCapacity = null,
            double? priceMin = null,
            double? priceMax = null);

        Task<Result<bool>> UpdateRoom(Room newRoom,
            int oldRoomId,
            int resourceAccessorId);

        Task<Result<bool>> RemoveRoom(int roomId,
            int resourceAccessorId);

        Task<Result<bool>> AddRoom(Room room, string currencyName, int resourceAccessorId);

        Task<Result<IReadOnlyList<Currency>>> GetCurrency(int? id = null,
            string name = null);
    }
}
