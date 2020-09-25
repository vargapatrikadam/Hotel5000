using Core.Entities.Authentication;
using Core.Entities.Domain;
using Core.Enums;
using Core.Enums.Lodging;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain.LodgingManagementService;
using Core.Interfaces.Domain.UserManagementService;
using Core.Results;
using Domain.Specifications.LodgingManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.LodgingManagement
{
    class LodgingService : ILodgingService
    {
        private readonly IAuthorization _authorizationService;
        private readonly IAsyncRepository<LodgingType> _lodgingTypeRepository;
        private readonly IApprovingDataService _approvingDataService;
        private readonly IRoomService _roomService;
        private readonly IReservationWindowService _reservationWindowService;
        private readonly ILodgingAddressService _lodgingAddressService;
        private readonly IAsyncRepository<Lodging> _lodgingRepository;
        public LodgingService(IAuthorization authorizationService,
            IAsyncRepository<LodgingType> lodgingTypeRepository,
            IApprovingDataService approvingDataService,
            IRoomService roomService,
            IReservationWindowService reservationWindowService,
            ILodgingAddressService lodgingAddressService,
            IAsyncRepository<Lodging> lodgingRepository)
        {
            _authorizationService = authorizationService;
            _lodgingTypeRepository = lodgingTypeRepository;
            _approvingDataService = approvingDataService;
            _roomService = roomService;
            _reservationWindowService = reservationWindowService;
            _lodgingAddressService = lodgingAddressService;
            _lodgingRepository = lodgingRepository;
        }
        public async Task<Result<bool>> AddLodging(Lodging lodging, string lodgingType)
        {
            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Lodging)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            LodgingTypes lodgingTypeAsEnum;
            if (!Enum.TryParse(lodgingType, out lodgingTypeAsEnum))
            {
                return new InvalidResult<bool>(Errors.LODGING_TYPE_NOT_FOUND);
            }

            var specification = new GetLodgingType(lodgingTypeAsEnum);
            var lodgingTypeEntity = (await _lodgingTypeRepository.GetAsync(specification)).FirstOrDefault();

            lodging.LodgingType = null;
            lodging.LodgingTypeId = lodgingTypeEntity.Id;


            ApprovingData userData = (await _approvingDataService.GetApprovingData(approvingDataOwnerId: lodging.UserId)).Data.FirstOrDefault();
            if (userData == null ||
                (lodgingTypeAsEnum == LodgingTypes.Company &&
                userData.RegistrationNumber == null) ||
                (lodgingTypeAsEnum == LodgingTypes.Private &&
                (userData.IdentityNumber == null || userData.TaxNumber == null)))
                return new UnauthorizedResult<bool>(Errors.APPROVING_DATA_NOT_VALID);


            List<LodgingAddress> lodgingAddresses = lodging.LodgingAddresses.ToList();
            List<Room> rooms = lodging.Rooms.ToList();
            List<ReservationWindow> reservationWindows = lodging.ReservationWindows.ToList();
            lodging.Rooms = null;
            lodging.LodgingAddresses = null;
            lodging.ReservationWindows = null;

            await _lodgingRepository.AddAsync(lodging);

            lodgingAddresses.ForEach(p => p.LodgingId = lodging.Id);
            rooms.ForEach(p => p.LodgingId = lodging.Id);
            reservationWindows.ForEach(p => p.LodgingId = lodging.Id);

            foreach (LodgingAddress address in lodgingAddresses)
            {
                var result = await _lodgingAddressService.AddLodgingAddress(address, address.Country.Code);
                if (result.ResultType != ResultType.Ok)
                    return result;
            }

            foreach (Room room in rooms)
            {
                var result = await _roomService.AddRoom(room, room.Currency.Name);
                if (result.ResultType != ResultType.Ok)
                    return result;
            }

            foreach (ReservationWindow reservationWindow in reservationWindows)
            {
                var result = await _reservationWindowService.AddReservationWindow(reservationWindow);
                if (result.ResultType != ResultType.Ok)
                    return result;
            }

            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<IReadOnlyList<Lodging>>> GetLodging(int? id = null,
            string name = null,
            string lodgingType = null,
            DateTime? reservableFrom = null,
            DateTime? reservableTo = null,
            string address = null,
            string owner = null,
            int? skip = null,
            int? take = null)
        {
            LodgingTypes lodgingTypeAsEnum;
            Enum.TryParse(lodgingType, out lodgingTypeAsEnum);

            Country country = null;
            if (address != null)
                country = (await _lodgingAddressService.GetCountry(name: address)).Data.FirstOrDefault();

            var specification = new GetLodging(id,
                name,
                lodgingTypeAsEnum,
                reservableFrom,
                reservableTo,
                address,
                owner,
                country,
                skip,
                take);

            var data = await _lodgingRepository.GetAsync(specification);

            return new SuccessfulResult<IReadOnlyList<Lodging>>(data);
        }
        public async Task<Result<bool>> RemoveLodging(int lodgingId)
        {
            Lodging removeThisLodging = (await GetLodging(id: lodgingId)).Data.FirstOrDefault();
            if (removeThisLodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Lodging)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(removeThisLodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            await _lodgingRepository.DeleteAsync(removeThisLodging);
            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> UpdateLodging(Lodging newLodging, int oldLodgingId)
        {
            Lodging updateThisLodging = (await GetLodging(id: oldLodgingId)).Data.FirstOrDefault();
            if (updateThisLodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Lodging)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(updateThisLodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            updateThisLodging.LodgingAddresses = null;
            updateThisLodging.Name = newLodging.Name;

            await _lodgingRepository.UpdateAsync(updateThisLodging);
            return new SuccessfulResult<bool>(true);
        }
    }
}
