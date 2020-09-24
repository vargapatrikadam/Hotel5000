using Core.Interfaces.Domain.LodgingManagementService;

namespace Core.Interfaces.Domain
{
    public interface ILodgingManagementService : ILodgingAddressService, IReservationWindowService, IRoomService, ILodgingService
    {
    }
}
