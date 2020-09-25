using Core.Entities.Authentication;
using Core.Entities.Domain;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain.LodgingManagementService;
using Core.Results;
using Domain.Specifications.LodgingManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.LodgingManagement
{
    class ReservationWindowService : IReservationWindowService
    {
        private readonly ILodgingService _lodgingService;
        private readonly IAuthorization _authorizationService;
        private readonly IAsyncRepository<ReservationWindow> _reservationWindowRepository;
        public ReservationWindowService(ILodgingService lodgingService,
            IAuthorization authorizationService,
            IAsyncRepository<ReservationWindow> reservationWindowRepository)
        {
            _lodgingService = lodgingService;
            _authorizationService = authorizationService;
            _reservationWindowRepository = reservationWindowRepository;
        }
        public async Task<Result<bool>> AddReservationWindow(ReservationWindow reservationWindow)
        {
            Lodging lodging = ((await _lodgingService.GetLodging(id: reservationWindow.LodgingId)).Data).FirstOrDefault();

            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ReservationWindow)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            reservationWindow.Lodging = null;
            reservationWindow.LodgingId = lodging.Id;

            if (reservationWindow.To < reservationWindow.From ||
                reservationWindow.From < DateTime.Now)
                return new InvalidResult<bool>(Errors.INVALID_RESERVATION_WINDOW_DATES);

            await _reservationWindowRepository.AddAsync(reservationWindow);

            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<IReadOnlyList<ReservationWindow>>> GetReservationWindow(int? id = null,
            int? lodgingId = null,
            DateTime? isAfter = null,
            DateTime? isBefore = null)
        {
            var specification = new GetReservationWindow(id, lodgingId, isAfter, isBefore);

            return new SuccessfulResult<IReadOnlyList<ReservationWindow>>(await _reservationWindowRepository.GetAsync(specification));
        }
        public async Task<Result<bool>> RemoveReservationWindow(int reservationWindowId)
        {
            ReservationWindow removeThisReservationWindow = (await GetReservationWindow(id: reservationWindowId)).Data.FirstOrDefault();
            if (removeThisReservationWindow == null)
                return new NotFoundResult<bool>(Errors.RESERVATION_WINDOW_NOT_FOUND);

            Lodging lodging = (await _lodgingService.GetLodging(id: removeThisReservationWindow.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ReservationWindow)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            await _reservationWindowRepository.DeleteAsync(removeThisReservationWindow);
            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> UpdateReservationWindow(ReservationWindow newReservationWindow, int oldReservationWindowId)
        {
            ReservationWindow updateThisReservationWindow = (await GetReservationWindow(id: oldReservationWindowId)).Data.FirstOrDefault();
            if (updateThisReservationWindow == null)
                return new NotFoundResult<bool>(Errors.RESERVATION_WINDOW_NOT_FOUND);

            Lodging lodging = (await _lodgingService.GetLodging(id: updateThisReservationWindow.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ReservationWindow)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (newReservationWindow.To < newReservationWindow.From ||
                newReservationWindow.From < DateTime.Now)
                return new InvalidResult<bool>(Errors.INVALID_RESERVATION_WINDOW_DATES);

            updateThisReservationWindow.From = newReservationWindow.From;
            updateThisReservationWindow.To = newReservationWindow.To;

            await _reservationWindowRepository.UpdateAsync(updateThisReservationWindow);

            return new SuccessfulResult<bool>(true);
        }
    }
}
