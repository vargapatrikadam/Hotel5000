using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.LodgingDomain;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.LodgingDomain
{
    public class ReservationService : IReservationService
    {
        private readonly ILodgingManagementService _lodgingManagementService;
        private readonly IAsyncRepository<PaymentType> _paymentTypeRepository;
        private readonly IAsyncRepository<Reservation> _reservationRepository;
        private readonly IAsyncRepository<ReservationItem> _reservationItemRepository;
        public ReservationService(ILodgingManagementService lodgingManagementService,
            IAsyncRepository<PaymentType> paymentTypeRepository,
            IAsyncRepository<Reservation> reservationRepository,
            IAsyncRepository<ReservationItem> reservationItemRepository)
        {
            _lodgingManagementService = lodgingManagementService;
            _paymentTypeRepository = paymentTypeRepository;
            _reservationItemRepository = reservationItemRepository;
            _reservationRepository = reservationRepository;
        }
        public Task<Result<bool>> DeleteReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IReadOnlyList<Reservation>>> GetReservation(int? id = null,
            int? roomId = null,
            int? reservationWindowId = null,
            string email = null,
            int? lodgingId = null)
        {
            ISpecification<Reservation> specification = new Specification<Reservation>();
            specification.AddInclude(p => p.PaymentType)
                .AddInclude(p => p.ReservationItems)
                .AddInclude(p => (p.ReservationItems as ReservationItem).Room.Currency)
                .AddInclude(p => (p.ReservationItems as ReservationItem).ReservationWindow)
                .AddInclude(p => (p.ReservationItems as ReservationItem).Room.Lodging);
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id) &&
                (!roomId.HasValue || p.ReservationItems.Any(p => p.RoomId == roomId)) &&
                (!reservationWindowId.HasValue || p.ReservationItems.Any(p => p.ReservationWindowId == reservationWindowId)) &&
                (email == null || p.Email == email) ||
                (!lodgingId.HasValue || p.ReservationItems.Any(p => p.Room.LodgingId == lodgingId)));

            return new SuccessfulResult<IReadOnlyList<Reservation>>(await _reservationRepository.GetAsync(specification));
        }

        public Task<Result<bool>> Reserve(Reservation newReservation)
        {
            throw new NotImplementedException();
        }
    }
}
