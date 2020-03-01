using Core.Entities.LodgingEntities;
using Core.Enums;
using Core.Enums.Lodging;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.LodgingDomain;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.LodgingDomain
{
    public class LodgingManagementService : ILodgingManagementService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAsyncRepository<Lodging> _lodgingRepository;
        private readonly IAsyncRepository<LodgingType> _lodgingTypeRepository;
        private readonly IAsyncRepository<LodgingAddress> _lodgingAddressRepository;
        private readonly IAsyncRepository<ReservationWindow> _reservationWindowRepository;
        private readonly IAsyncRepository<Room> _roomRepository;
        private readonly IAsyncRepository<Country> _countryRepository;
        private readonly IAsyncRepository<Currency> _currencyRepository;
        public LodgingManagementService(IAuthenticationService authenticationService,
            IAsyncRepository<Lodging> lodgingRepository,
            IAsyncRepository<LodgingType> lodgingTypeRepository,
            IAsyncRepository<LodgingAddress> lodgingAddressRepository,
            IAsyncRepository<ReservationWindow> reservationWindowRepository,
            IAsyncRepository<Room> roomRepository,
            IAsyncRepository<Country> countryRepository,
            IAsyncRepository<Currency> currecyRepository)
        {
            _authenticationService = authenticationService;
            _lodgingRepository = lodgingRepository;
            _lodgingTypeRepository = lodgingTypeRepository;
            _lodgingAddressRepository = lodgingAddressRepository;
            _reservationWindowRepository = reservationWindowRepository;
            _roomRepository = roomRepository;
            _countryRepository = countryRepository;
            _currencyRepository = currecyRepository;
        }
        #region add
        public async Task<Result<bool>> AddLodging(Lodging lodging, string lodgingType, int resourceAccessorId)
        {
            Result<bool> authorizationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);

            if (!authorizationResult.Data)
                return authorizationResult;

            LodgingTypes lodgingTypeAsEnum;
            if (!Enum.TryParse(lodgingType, out lodgingTypeAsEnum))
            {
                return new InvalidResult<bool>(Errors.LODGING_TYPE_NOT_FOUND);
            }

            var lodgingTypeEntity = (await _lodgingTypeRepository.GetAsync(new Specification<LodgingType>().ApplyFilter(p => p.Name == lodgingTypeAsEnum))).FirstOrDefault();

            lodging.LodgingType = null;
            lodging.LodgingTypeId = lodgingTypeEntity.Id;

            await _lodgingRepository.AddAsync(lodging);

            foreach (LodgingAddress address in lodging.LodgingAddresses)
            {
                var result = await AddLodgingAddress(address, address.Country.Code, resourceAccessorId);
                if (result.ResultType != ResultType.Ok)
                    return result;
            }

            foreach (Room room in lodging.Rooms)
            {
                var result = await AddRoom(room, resourceAccessorId);
                if (result.ResultType != ResultType.Ok)
                    return result;
            }

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddLodgingAddress(LodgingAddress lodgingAddress, string countryCode, int resourceAccessorId)
        {
            Lodging lodging = ((await GetLodging(id: lodgingAddress.LodgingId)).Data).FirstOrDefault();

            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authorizationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (await _lodgingAddressRepository.AnyAsync(p =>
                 p.CountryId == lodgingAddress.CountryId &&
                 p.County == lodgingAddress.County &&
                 p.City == lodgingAddress.City &&
                 p.PostalCode == lodgingAddress.PostalCode &&
                 p.Street == lodgingAddress.Street))
                return new ConflictResult<bool>(Errors.ADDRESS_NOT_UNIQUE);

            Country country = (await GetCountry(code: countryCode)).Data.FirstOrDefault();
            if (country == null)
            {
                try
                {
                    await _countryRepository.AddAsync(new Country()
                    {
                        Code = countryCode,
                        Name = new RegionInfo(countryCode).EnglishName

                    });
                    country = (await GetCountry(code: countryCode)).Data.FirstOrDefault();
                }
                catch (ArgumentException)
                {
                    return new InvalidResult<bool>(Errors.COUNTRY_NOT_FOUND);
                }
            }

            await _lodgingAddressRepository.AddAsync(lodgingAddress);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> AddReservationWindow(ReservationWindow reservationWindow, int resourceAccessorId)
        {
            Lodging lodging = ((await GetLodging(id: reservationWindow.LodgingId)).Data).FirstOrDefault();

            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authorizationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);

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

        public async Task<Result<bool>> AddRoom(Room room, int resourceAccessorId)
        {
            Lodging lodging = ((await GetLodging(id: room.LodgingId)).Data).FirstOrDefault();

            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Currency currency = (await GetCurrency(id: room.CurrencyId)).Data.FirstOrDefault();
            if (currency == null)
                return new NotFoundResult<bool>(Errors.CURRENCY_NOT_FOUND);

            Result<bool> authorizationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);

            if (!authorizationResult.Data)
                return authorizationResult;

            room.Lodging = null;
            room.LodgingId = lodging.Id;

            if (room.ChildrenCapacity < 0 ||
                room.AdultCapacity < 0)
                return new InvalidResult<bool>(Errors.INVALID_ROOM_CAPACITY);

            await _roomRepository.AddAsync(room);

            return new SuccessfulResult<bool>(true);
        }
        #endregion
        #region get

        public async Task<Result<IReadOnlyList<Country>>> GetCountry(int? id = null, string name = null, string code = null)
        {
            ISpecification<Country> specification = new Specification<Country>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (name == null || p.Name == name) &&
                (code == null || p.Code == code));

            return new SuccessfulResult<IReadOnlyList<Country>>(await _countryRepository.GetAsync(specification));
        }

        public async Task<Result<IReadOnlyList<Currency>>> GetCurrency(int? id = null, string name = null)
        {
            ISpecification<Currency> specification = new Specification<Currency>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (name == null || p.Name == name));

            return new SuccessfulResult<IReadOnlyList<Currency>>(await _currencyRepository.GetAsync(specification));
        }

        public async Task<Result<IReadOnlyList<Lodging>>> GetLodging(int? id = null, string name = null, LodgingTypes? lodgingType = null, int? skip = null, int? take = null)
        {
            ISpecification<Lodging> specification = new Specification<Lodging>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (name == null || p.Name == name) &&
                (lodgingType == null || p.LodgingType.Name == lodgingType))
                .AddInclude(p => p.LodgingType)
                .AddInclude(p => p.User);

            if (skip.HasValue && take.HasValue)
                specification.ApplyPaging(skip.Value, take.Value);

            return new SuccessfulResult<IReadOnlyList<Lodging>>(await _lodgingRepository.GetAsync(specification));
        }

        public async Task<Result<IReadOnlyList<LodgingAddress>>> GetLodgingAddress(int? id = null, int? lodgingId = null, string countryCode = null, string countryName = null, string county = null, string city = null, string postalCode = null, string lodgingName = null)
        {
            ISpecification<LodgingAddress> specification = new Specification<LodgingAddress>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                (countryCode == null || p.Country.Code == countryCode) &&
                (countryName == null || p.Country.Name == countryName) &&
                (county == null || p.County == county) &&
                (city == null || p.City == city) &&
                (postalCode == null || p.PostalCode == postalCode) &&
                (lodgingName == null || p.Lodging.Name == lodgingName))
                .AddInclude(p => p.Country)
                .AddInclude(p => p.Lodging);

            return new SuccessfulResult<IReadOnlyList<LodgingAddress>>(await _lodgingAddressRepository.GetAsync(specification));
        }

        public async Task<Result<IReadOnlyList<ReservationWindow>>> GetReservationWindow(int? id = null, int? lodgingId = null, DateTime? isAfter = null, DateTime? isBefore = null)
        {
            ISpecification<ReservationWindow> specification = new Specification<ReservationWindow>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                (!isAfter.HasValue || p.From > isAfter.Value) &&
                (!isBefore.HasValue || p.To < isBefore.Value));

            return new SuccessfulResult<IReadOnlyList<ReservationWindow>>(await _reservationWindowRepository.GetAsync(specification));
        }

        public async Task<Result<IReadOnlyList<Room>>> GetRoom(int? id = null, int? lodgingId = null, int? adultCapacity = null, int? childrenCapacity = null, double? priceMin = null, double? priceMax = null)
        {
            ISpecification<Room> specification = new Specification<Room>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                (!adultCapacity.HasValue || p.AdultCapacity == adultCapacity.Value) &&
                (!childrenCapacity.HasValue || p.ChildrenCapacity == childrenCapacity.Value) &&
                (!priceMin.HasValue || p.Price <= priceMin.Value) &&
                (!priceMax.HasValue || p.Price >= priceMax.Value));

            return new SuccessfulResult<IReadOnlyList<Room>>(await _roomRepository.GetAsync(specification));
        }
        #endregion
        #region remove
        public async Task<Result<bool>> RemoveLodging(int lodgingId, int resourceAccessorId)
        {
            Lodging removeThisLodging = (await GetLodging(id: lodgingId)).Data.FirstOrDefault();
            if (removeThisLodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(removeThisLodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            await _lodgingRepository.DeleteAsync(removeThisLodging);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> RemoveLodgingAddress(int lodgingAddressId, int resourceAccessorId)
        {
            LodgingAddress removeThisLodgingAddress = (await GetLodgingAddress(id: lodgingAddressId)).Data.FirstOrDefault();
            if (removeThisLodgingAddress == null)
                return new NotFoundResult<bool>(Errors.LODGING_ADDRESS_NOT_FOUND);

            Lodging lodging = (await GetLodging(id: removeThisLodgingAddress.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            await _lodgingAddressRepository.DeleteAsync(removeThisLodgingAddress);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> RemoveReservationWindow(int reservationWindowId, int resourceAccessorId)
        {
            ReservationWindow removeThisReservationWindow = (await GetReservationWindow(id: reservationWindowId)).Data.FirstOrDefault();
            if (removeThisReservationWindow == null)
                return new NotFoundResult<bool>(Errors.RESERVATION_WINDOW_NOT_FOUND);

            Lodging lodging = (await GetLodging(id: removeThisReservationWindow.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            await _reservationWindowRepository.DeleteAsync(removeThisReservationWindow);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> RemoveRoom(int roomId, int resourceAccessorId)
        {
            Room removeThisRoom = (await GetRoom(id: roomId)).Data.FirstOrDefault();
            if (removeThisRoom == null)
                return new NotFoundResult<bool>(Errors.ROOM_NOT_FOUND);

            Lodging lodging = (await GetLodging(id: removeThisRoom.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            await _roomRepository.DeleteAsync(removeThisRoom);
            return new SuccessfulResult<bool>(true);
        }
        #endregion
        #region update
        public async Task<Result<bool>> UpdateLodging(Lodging newLodging, int oldLodgingId, int resourceAccessorId)
        {
            Lodging updateThisLodging = (await GetLodging(id: oldLodgingId)).Data.FirstOrDefault();
            if (updateThisLodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(updateThisLodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            updateThisLodging.Name = newLodging.Name;

            await _lodgingRepository.UpdateAsync(updateThisLodging);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateLodgingAddress(LodgingAddress newLodgingAddress, int oldLodgingAddressId, int resourceAccessorId)
        {
            LodgingAddress updateThisLodgingAddress = (await GetLodgingAddress(id: oldLodgingAddressId)).Data.FirstOrDefault();
            if (updateThisLodgingAddress == null)
                return new NotFoundResult<bool>(Errors.LODGING_ADDRESS_NOT_FOUND);

            Lodging lodging = (await GetLodging(id: updateThisLodgingAddress.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            if (await _lodgingAddressRepository.AnyAsync(p =>
                 p.CountryId == newLodgingAddress.CountryId &&
                 p.County == newLodgingAddress.County &&
                 p.City == newLodgingAddress.City &&
                 p.PostalCode == newLodgingAddress.PostalCode &&
                 p.Street == newLodgingAddress.Street))
                return new ConflictResult<bool>(Errors.ADDRESS_NOT_UNIQUE);

            updateThisLodgingAddress.CountryId = newLodgingAddress.CountryId;
            updateThisLodgingAddress.County = newLodgingAddress.County;
            updateThisLodgingAddress.City = newLodgingAddress.City;
            updateThisLodgingAddress.Street = newLodgingAddress.Street;
            updateThisLodgingAddress.PostalCode = newLodgingAddress.PostalCode;
            updateThisLodgingAddress.HouseNumber = newLodgingAddress.HouseNumber;
            updateThisLodgingAddress.Floor = newLodgingAddress.Floor;
            updateThisLodgingAddress.DoorNumber = newLodgingAddress.DoorNumber;

            await _lodgingAddressRepository.UpdateAsync(updateThisLodgingAddress);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateReservationWindow(ReservationWindow newReservationWindow, int oldReservationWindowId, int resourceAccessorId)
        {
            ReservationWindow updateThisReservationWindow = (await GetReservationWindow(id: oldReservationWindowId)).Data.FirstOrDefault();
            if (updateThisReservationWindow == null)
                return new NotFoundResult<bool>(Errors.RESERVATION_WINDOW_NOT_FOUND);

            Lodging lodging = (await GetLodging(id: updateThisReservationWindow.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            updateThisReservationWindow.From = newReservationWindow.From;
            updateThisReservationWindow.To = newReservationWindow.To;

            await _reservationWindowRepository.UpdateAsync(updateThisReservationWindow);

            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateRoom(Room newRoom, int oldRoomId, int resourceAccessorId)
        {
            Room updateThisRoom = (await GetRoom(id: oldRoomId)).Data.FirstOrDefault();
            if (updateThisRoom == null)
                return new NotFoundResult<bool>(Errors.ROOM_NOT_FOUND);

            Lodging lodging = (await GetLodging(id: updateThisRoom.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            updateThisRoom.CurrencyId = newRoom.CurrencyId;
            updateThisRoom.AdultCapacity = newRoom.AdultCapacity;
            updateThisRoom.ChildrenCapacity = newRoom.ChildrenCapacity;
            updateThisRoom.Price = newRoom.Price;
            updateThisRoom.CurrencyId = newRoom.CurrencyId;

            await _roomRepository.UpdateAsync(updateThisRoom);

            return new SuccessfulResult<bool>(true);
        }
        #endregion
    }
}
