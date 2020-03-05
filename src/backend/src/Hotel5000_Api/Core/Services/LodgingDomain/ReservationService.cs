using Core.Entities.LodgingEntities;
using Core.Enums;
using Core.Enums.Lodging;
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


        public async Task<Result<bool>> DeleteReservation(int reservationId)
        {
            Reservation deleteThis = (await _reservationRepository.GetAsync(new Specification<Reservation>().ApplyFilter(p => p.Id == reservationId))).FirstOrDefault();
            if (deleteThis == null)
                return new NotFoundResult<bool>(Errors.RESERVATION_NOT_FOUND);
            await _reservationRepository.DeleteAsync(deleteThis);
            return new SuccessfulResult<bool>(true);
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
        public async Task<Result<bool>> Reserve(Reservation newReservation, string paymentType)
        {
            List<ReservationItem> reservationItems = newReservation.ReservationItems.ToList();
            newReservation.ReservationItems = null;

            PaymentTypes paymentTypeAsEnum;
            if (!Enum.TryParse(paymentType, out paymentTypeAsEnum))
            {
                return new InvalidResult<bool>(Errors.LODGING_TYPE_NOT_FOUND);
            }
            PaymentType paymentTypeEntity = (await _paymentTypeRepository.GetAsync(new Specification<PaymentType>().ApplyFilter(p => p.Name == paymentTypeAsEnum))).FirstOrDefault();
            newReservation.PaymentTypeId = paymentTypeEntity.Id;

            if (reservationItems.Any(p => p.ReservedFrom > p.ReservedTo))
                return new InvalidResult<bool>(Errors.RESERVATION_DATE_INVALID);

            foreach (ReservationItem newItem in reservationItems)
            {
                Room reservedRoom = (await _lodgingManagementService.GetRoom(id: newItem.RoomId)).Data.FirstOrDefault();
                if (reservedRoom == null)
                    return new NotFoundResult<bool>(Errors.ROOM_NOT_FOUND);

                var activeReservationWindow = (await _lodgingManagementService.GetReservationWindow(lodgingId: reservedRoom.LodgingId)).Data.LastOrDefault();
                if (activeReservationWindow == null || !(newItem.ReservedFrom >= activeReservationWindow.From && newItem.ReservedTo <= activeReservationWindow.To))
                    return new InvalidResult<bool>(Errors.RESERVATION_DATE_INVALID);

                newItem.ReservationWindowId = activeReservationWindow.Id;

                if (await _reservationItemRepository.AnyAsync(existing => (existing.RoomId == newItem.RoomId) &&
                    (existing.ReservedFrom < newItem.ReservedTo && newItem.ReservedFrom < existing.ReservedTo)))
                    return new InvalidResult<bool>(Errors.RESERVATION_DATE_TAKEN);
            }

            await _reservationRepository.AddAsync(newReservation);
            foreach (ReservationItem newItem in reservationItems)
            {
                newItem.ReservationId = newReservation.Id;
                await _reservationItemRepository.AddAsync(newItem);
            }

            return new SuccessfulResult<bool>(true);
        }
    }
}
