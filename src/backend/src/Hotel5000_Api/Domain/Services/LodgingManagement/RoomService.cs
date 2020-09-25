using Core.Entities.Authentication;
using Core.Entities.Domain;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain.LodgingManagementService;
using Core.Results;
using Domain.Specifications.LodgingManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.LodgingManagement
{
    class RoomService : IRoomService
    {
        private readonly ILodgingService _lodgingService;
        private readonly IAuthorization _authorizationService;
        private readonly IAsyncRepository<Room> _roomRepository;
        private readonly IAsyncRepository<Currency> _currencyRepository;
        public RoomService(ILodgingService lodgingService,
            IAuthorization authorizationService,
            IAsyncRepository<Room> roomRepository,
            IAsyncRepository<Currency> currencyRepository)
        {
            _lodgingService = lodgingService;
            _authorizationService = authorizationService;
            _roomRepository = roomRepository;
            _currencyRepository = currencyRepository;
        }
        public async Task<Result<bool>> AddRoom(Room room, string currencyName)
        {
            Lodging lodging = ((await _lodgingService.GetLodging(id: room.LodgingId)).Data).FirstOrDefault();

            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Currency currency = (await GetCurrency(name: currencyName)).Data.FirstOrDefault();
            if (currency == null)
                return new NotFoundResult<bool>(Errors.CURRENCY_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Room)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            room.Lodging = null;
            room.LodgingId = lodging.Id;
            room.Currency = null;
            room.CurrencyId = currency.Id;

            if (room.ChildrenCapacity < 0 ||
                room.AdultCapacity < 0)
                return new InvalidResult<bool>(Errors.INVALID_ROOM_CAPACITY);

            await _roomRepository.AddAsync(room);

            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<IReadOnlyList<Currency>>> GetCurrency(int? id = null, string name = null)
        {
            var specification = new GetCurrency(id, name);

            return new SuccessfulResult<IReadOnlyList<Currency>>(await _currencyRepository.GetAsync(specification));
        }
        public async Task<Result<IReadOnlyList<Room>>> GetRoom(int? id = null,
            int? lodgingId = null,
            int? adultCapacity = null,
            int? childrenCapacity = null,
            double? priceMin = null,
            double? priceMax = null)
        {
            var specification = new GetRoom(id, lodgingId, adultCapacity, childrenCapacity, priceMin, priceMax);

            return new SuccessfulResult<IReadOnlyList<Room>>(await _roomRepository.GetAsync(specification));
        }
        public async Task<Result<bool>> RemoveRoom(int roomId)
        {
            Room removeThisRoom = (await GetRoom(id: roomId)).Data.FirstOrDefault();
            if (removeThisRoom == null)
                return new NotFoundResult<bool>(Errors.ROOM_NOT_FOUND);

            Lodging lodging = (await _lodgingService.GetLodging(id: removeThisRoom.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Room)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            await _roomRepository.DeleteAsync(removeThisRoom);
            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> UpdateRoom(Room newRoom, int oldRoomId)
        {
            Room updateThisRoom = (await GetRoom(id: oldRoomId)).Data.FirstOrDefault();
            if (updateThisRoom == null)
                return new NotFoundResult<bool>(Errors.ROOM_NOT_FOUND);

            Lodging lodging = (await _lodgingService.GetLodging(id: updateThisRoom.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Currency newCurrency = (await GetCurrency(name: newRoom.Currency.Name)).Data.FirstOrDefault();
            if (newCurrency == null)
                return new NotFoundResult<bool>(Errors.CURRENCY_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(Room)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            updateThisRoom.Currency = null;
            updateThisRoom.CurrencyId = newCurrency.Id;
            updateThisRoom.AdultCapacity = newRoom.AdultCapacity;
            updateThisRoom.ChildrenCapacity = newRoom.ChildrenCapacity;
            updateThisRoom.Price = newRoom.Price;

            await _roomRepository.UpdateAsync(updateThisRoom);

            return new SuccessfulResult<bool>(true);
        }
    }
}
