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
        private readonly IAsyncRepository<Lodging> _lodgingRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAsyncRepository<LodgingType> _lodgingTypeRepository;
        private readonly IAsyncRepository<LodgingAddress> _lodgingAddressRepository;
        private readonly IAsyncRepository<ReservationWindow> _reservationWindowRepository;
        private readonly IAsyncRepository<Room> _roomRepository;
        private readonly IAsyncRepository<Country> _countryRepository;
        private readonly IAsyncRepository<Currency> _currencyRepository;
        public LodgingManagementService(IAsyncRepository<Lodging> lodgingRepository,
            IAuthenticationService authenticationService,
            IAsyncRepository<LodgingType> lodgingTypeRepository,
            IAsyncRepository<LodgingAddress> lodgingAddressRepository,
            IAsyncRepository<ReservationWindow> reservationWindowRepository,
            IAsyncRepository<Room> roomRepository,
            IAsyncRepository<Country> countryRepository,
            IAsyncRepository<Currency> currecyRepository)
        {
            _lodgingRepository = lodgingRepository;
            _authenticationService = authenticationService;
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
                await AddLodgingAddress(address, address.Country.Code, resourceAccessorId);
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

            ISpecification<Country> specification = new Specification<Country>().ApplyFilter(p => p.Code == countryCode);
            Country country = (await _countryRepository.GetAsync(specification)).FirstOrDefault();
            if (country == null) 
            {
                try
                {
                    await _countryRepository.AddAsync(new Country()
                    {
                        Code = countryCode,
                        Name = new RegionInfo(countryCode).EnglishName

                    });
                    country = (await _countryRepository.GetAsync(specification)).FirstOrDefault();
                }
                catch (ArgumentException e)
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

        public Task<Result<bool>> AddRoom(Room room, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region get
        public Task<Result<IReadOnlyList<Lodging>>> GetLodging(int? id = null, string name = null, LodgingTypes? lodgingType = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IReadOnlyList<LodgingAddress>>> GetLodgingAddress(int? id = null, string countryCode = null, string countryName = null, string county = null, string city = null, string postalCode = null, string lodgingName = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IReadOnlyList<ReservationWindow>>> GetReservationWindow(int? id = null, DateTime? from = null, DateTime? to = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IReadOnlyList<Room>>> GetRoom(int? id = null, int? adultCapacity = null, int? childrenCapacity = null, double? priceMin = null, double? priceMax = null)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region remove
        public Task<Result<bool>> RemoveLodging(int lodgingId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> RemoveLodgingAddress(int lodgingAddressId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> RemoveReservationWindow(int reservationWindowId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> RemoveRoom(int roomId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region update
        public Task<Result<bool>> UpdateLodging(Lodging newLodging, int oldLodgingId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UpdateLodgingAddress(LodgingAddress newLodgingAddress, int oldLodgingAddressId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UpdateReservationWindow(ReservationWindow newReservationWindow, int oldReservationWindowId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> UpdateRoom(Room newRoom, int oldRoomId, int resourceAccessorId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
