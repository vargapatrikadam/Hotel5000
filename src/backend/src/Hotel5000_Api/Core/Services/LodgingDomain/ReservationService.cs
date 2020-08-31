using Core.Entities.LodgingEntities;
using Core.Enums;
using Core.Enums.Lodging;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.LodgingDomain;
using Core.Specifications.Reservation;
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
            Reservation deleteThis = (await GetReservation(id: reservationId)).Data.FirstOrDefault();
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
            var specification = new GetReservation(id, roomId, reservationWindowId, email, lodgingId);

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

            var getPaymentTypeSpecification = new GetPaymentType(paymentTypeAsEnum);
            PaymentType paymentTypeEntity = (await _paymentTypeRepository.GetAsync(getPaymentTypeSpecification)).FirstOrDefault();
            newReservation.PaymentTypeId = paymentTypeEntity.Id;

            if (reservationItems.Any(p => p.ReservedFrom > p.ReservedTo))
                return new InvalidResult<bool>(Errors.RESERVATION_DATE_INVALID);

            var specification = new IsReservationAvailable();

            //could make it work like when one is invalid, it doesnt break a loop, and reserves the rest
            foreach (ReservationItem newItem in reservationItems)
            {
                Room reservedRoom = (await _lodgingManagementService.GetRoom(id: newItem.RoomId)).Data.FirstOrDefault();
                if (reservedRoom == null)
                    return new NotFoundResult<bool>(Errors.ROOM_NOT_FOUND);

                var activeReservationWindow = (await _lodgingManagementService.GetReservationWindow(lodgingId: reservedRoom.LodgingId)).Data.LastOrDefault();
                if (activeReservationWindow == null || !(newItem.ReservedFrom >= activeReservationWindow.From && newItem.ReservedTo <= activeReservationWindow.To))
                    return new InvalidResult<bool>(Errors.RESERVATION_DATE_INVALID);

                newItem.ReservationWindowId = activeReservationWindow.Id;

                specification.SetFilter(newItem);

                ReservationItem existingReservation = (await _reservationItemRepository.GetAsync(specification)).FirstOrDefault();
                if (existingReservation != null)
                    return new InvalidResult<bool>(Errors.RESERVATION_DATE_TAKEN, $"This room is already reserved from {existingReservation.ReservedFrom.ToShortDateString()} to {existingReservation.ReservedTo.ToShortDateString()}");
            }

            await _reservationRepository.AddAsync(newReservation);
            foreach (ReservationItem newItem in reservationItems)
            {
                newItem.ReservationId = newReservation.Id;
                await _reservationItemRepository.AddAsync(newItem);
            }

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<IReadOnlyList<ReservationWindow>>> GetAvailableReservationWindowsForRoom(int roomId)
        {
            Room room = (await _lodgingManagementService.GetRoom(id: roomId)).Data.FirstOrDefault();
            if (room == null)
                return new NotFoundResult<IReadOnlyList<ReservationWindow>>(Errors.ROOM_NOT_FOUND);

            ReservationWindow reservationWindowForLodging = (await _lodgingManagementService.GetReservationWindow(lodgingId: room.LodgingId, isBefore: DateTime.Now)).Data.FirstOrDefault();
            if (reservationWindowForLodging == null)
                return new NotFoundResult<IReadOnlyList<ReservationWindow>>(Errors.RESERVATION_WINDOW_NOT_FOUND);

            var specification = new GetReservationItem(reservationWindowForLodging.Id, roomId);
            List<ReservationItem> reservationItems = (await _reservationItemRepository.GetAsync(specification)).ToList();

            List<ReservationWindow> freeReservationWindows = new List<ReservationWindow>();

            freeReservationWindows.Add(reservationWindowForLodging);
            foreach (ReservationItem item in reservationItems)
            {
                ReservationWindow sliceThis = freeReservationWindows.Where(p => item.ReservedFrom >= p.From && item.ReservedTo <= p.To).FirstOrDefault();
                freeReservationWindows.Remove(sliceThis);
                ReservationWindow before = new ReservationWindow
                {
                    From = sliceThis.From,
                    To = item.ReservedFrom
                };
                ReservationWindow after = new ReservationWindow
                {
                    From = item.ReservedTo,
                    To = sliceThis.To
                };
                freeReservationWindows.Add(before);
                freeReservationWindows.Add(after);
            }



            return new SuccessfulResult<IReadOnlyList<ReservationWindow>>(freeReservationWindows);
        }
    }
}
