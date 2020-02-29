using Core.Interfaces.LodgingDomain.LodgingManagementService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.LodgingDomain
{
    public interface ILodgingManagementService : ILodgingAddressService, IReservationWindowService, IRoomService, ILodgingService
    {
    }
}
