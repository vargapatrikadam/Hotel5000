using Core.Entities.Domain;
using Core.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Domain.LodgingManagementService
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
            int oldRoomId);

        Task<Result<bool>> RemoveRoom(int roomId);

        Task<Result<bool>> AddRoom(Room room, string currencyName);

        Task<Result<IReadOnlyList<Currency>>> GetCurrency(int? id = null,
            string name = null);
    }
}
