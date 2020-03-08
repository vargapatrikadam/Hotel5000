using Core.Interfaces.LodgingDomain.LodgingManagementService;

namespace Core.Interfaces.LodgingDomain
{
    public interface ILodgingManagementService : ILodgingAddressService, IReservationWindowService, IRoomService, ILodgingService
    {
    }
}
