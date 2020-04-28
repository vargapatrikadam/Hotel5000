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
        private readonly IUserManagementService _userManagementService;
        private readonly IAsyncRepository<Lodging> _lodgingRepository;
        private readonly IAsyncRepository<LodgingType> _lodgingTypeRepository;
        private readonly IAsyncRepository<LodgingAddress> _lodgingAddressRepository;
        private readonly IAsyncRepository<ReservationWindow> _reservationWindowRepository;
        private readonly IAsyncRepository<Room> _roomRepository;
        private readonly IAsyncRepository<Country> _countryRepository;
        private readonly IAsyncRepository<Currency> _currencyRepository;
        public LodgingManagementService(IAuthenticationService authenticationService,
            IUserManagementService userManagementService,
            IAsyncRepository<Lodging> lodgingRepository,
            IAsyncRepository<LodgingType> lodgingTypeRepository,
            IAsyncRepository<LodgingAddress> lodgingAddressRepository,
            IAsyncRepository<ReservationWindow> reservationWindowRepository,
            IAsyncRepository<Room> roomRepository,
            IAsyncRepository<Country> countryRepository,
            IAsyncRepository<Currency> currecyRepository)
        {
            _authenticationService = authenticationService;
            _userManagementService = userManagementService;
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


            ApprovingData userData = (await _userManagementService.GetApprovingData(approvingDataOwnerId: resourceAccessorId)).Data.FirstOrDefault();
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
                var result = await AddLodgingAddress(address, address.Country.Code, resourceAccessorId);
                if (result.ResultType != ResultType.Ok)
                    return result;
            }

            foreach (Room room in rooms)
            {
                var result = await AddRoom(room, room.Currency.Name, resourceAccessorId);
                if (result.ResultType != ResultType.Ok)
                    return result;
            }

            foreach (ReservationWindow reservationWindow in reservationWindows)
            { 
                var result = await AddReservationWindow(reservationWindow, resourceAccessorId);
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
            lodgingAddress.Country = null;
            lodgingAddress.CountryId = country.Id;

            bool exists = await _lodgingAddressRepository.AnyAsync(p =>
                 (p.CountryId == lodgingAddress.CountryId &&
                 p.County == lodgingAddress.County &&
                 p.City == lodgingAddress.City &&
                 p.PostalCode == lodgingAddress.PostalCode &&
                 p.Street == lodgingAddress.Street &&
                 p.HouseNumber == lodgingAddress.HouseNumber &&
                 p.Floor == lodgingAddress.Floor &&
                 p.DoorNumber == lodgingAddress.DoorNumber));

            if (exists)
                return new ConflictResult<bool>(Errors.ADDRESS_NOT_UNIQUE);

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

        public async Task<Result<bool>> AddRoom(Room room, string currencyName, int resourceAccessorId)
        {
            Lodging lodging = ((await GetLodging(id: room.LodgingId)).Data).FirstOrDefault();

            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Currency currency = (await GetCurrency(name: currencyName)).Data.FirstOrDefault();
            if (currency == null)
                return new NotFoundResult<bool>(Errors.CURRENCY_NOT_FOUND);

            Result<bool> authorizationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);

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
                country = (await GetCountry(name: address)).Data.FirstOrDefault();

            ISpecification<Lodging> specification = new Specification<Lodging>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (name == null || p.Name == name) &&
                (lodgingType == null || p.LodgingType.Name == lodgingTypeAsEnum) &&
                (!reservableFrom.HasValue || p.ReservationWindows.Any(p => reservableFrom >= p.From && p.To > reservableFrom )) &&
                (!reservableTo.HasValue || p.ReservationWindows.Any(p => reservableTo >= p.From && p.To > reservableTo)) &&
                (owner == null || p.User.Email == owner || p.User.Username == owner) &&
                (address == null || p.LodgingAddresses.Any(p => p.County.StartsWith(address) ||
                                   p.City.StartsWith(address) || 
                                   p.PostalCode.StartsWith(address) || 
                                   p.Street.StartsWith(address)) || 
                                   (country != null &&
                                   p.LodgingAddresses.Any(p => p.CountryId == country.Id))))
                .AddInclude(p => p.LodgingType)
                .AddInclude(p => p.User)
                .AddInclude(p => (p.LodgingAddresses as LodgingAddress).Country)
                .AddInclude(p => (p.Rooms as Room).Currency)
                .AddInclude(p => p.ReservationWindows);

            if (skip.HasValue && take.HasValue)
                specification.ApplyPaging(skip.Value, take.Value);

            var data = await _lodgingRepository.GetAsync(specification);

            return new SuccessfulResult<IReadOnlyList<Lodging>>(data);
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

        public async Task<Result<IReadOnlyList<ReservationWindow>>> GetReservationWindow(int? id = null, 
            int? lodgingId = null,
            DateTime? isAfter = null, 
            DateTime? isBefore = null)
        {
            ISpecification<ReservationWindow> specification = new Specification<ReservationWindow>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                (!isAfter.HasValue || p.From >= isAfter.Value) &&
                (!isBefore.HasValue || p.To >= isBefore.Value))
                .AddInclude(p => (p.ReservationItems as ReservationItem).Room);

            return new SuccessfulResult<IReadOnlyList<ReservationWindow>>(await _reservationWindowRepository.GetAsync(specification));
        }
        public async Task<Result<IReadOnlyList<Room>>> GetRoom(int? id = null, 
            int? lodgingId = null, 
            int? adultCapacity = null, 
            int? childrenCapacity = null, 
            double? priceMin = null, 
            double? priceMax = null)
        {
            ISpecification<Room> specification = new Specification<Room>();
            specification.ApplyFilter(p =>
                (!id.HasValue || p.Id == id.Value) &&
                (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                (!adultCapacity.HasValue || p.AdultCapacity == adultCapacity.Value) &&
                (!childrenCapacity.HasValue || p.ChildrenCapacity == childrenCapacity.Value) &&
                (!priceMin.HasValue || p.Price <= priceMin.Value) &&
                (!priceMax.HasValue || p.Price >= priceMax.Value))
                .AddInclude(p => p.Currency)
                .AddInclude(p => p.ReservationItems);

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

            updateThisLodging.LodgingAddresses = null;
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

            Country newCountry = (await GetCountry(code: newLodgingAddress.Country.Code)).Data.FirstOrDefault();
            if (newCountry == null)
                return new NotFoundResult<bool>(Errors.COUNTRY_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            bool exists = await _lodgingAddressRepository.AnyAsync(p =>
                 p.Id != lodging.Id &&
                 p.CountryId == newCountry.Id &&
                 p.County == newLodgingAddress.County &&
                 p.City == newLodgingAddress.City &&
                 p.PostalCode == newLodgingAddress.PostalCode &&
                 p.Street == newLodgingAddress.Street &&
                 p.HouseNumber == newLodgingAddress.HouseNumber &&
                 p.Floor == newLodgingAddress.Floor &&
                 p.DoorNumber == newLodgingAddress.DoorNumber);

            if (exists)
                return new ConflictResult<bool>(Errors.ADDRESS_NOT_UNIQUE);

            updateThisLodgingAddress.Country = null;
            updateThisLodgingAddress.CountryId = newCountry.Id;
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

            if (newReservationWindow.To < newReservationWindow.From ||
                newReservationWindow.From < DateTime.Now)
                return new InvalidResult<bool>(Errors.INVALID_RESERVATION_WINDOW_DATES);

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

            Currency newCurrency = (await GetCurrency(name: newRoom.Currency.Name)).Data.FirstOrDefault();
            if (newCurrency == null)
                return new NotFoundResult<bool>(Errors.CURRENCY_NOT_FOUND);

            Result<bool> authenticationResult = await _authenticationService.IsAuthorized(lodging.UserId, resourceAccessorId);
            if (!authenticationResult.Data)
                return authenticationResult;

            updateThisRoom.Currency = null;
            updateThisRoom.CurrencyId = newCurrency.Id;
            updateThisRoom.AdultCapacity = newRoom.AdultCapacity;
            updateThisRoom.ChildrenCapacity = newRoom.ChildrenCapacity;
            updateThisRoom.Price = newRoom.Price;

            await _roomRepository.UpdateAsync(updateThisRoom);

            return new SuccessfulResult<bool>(true);
        }
        #endregion
    }
}
