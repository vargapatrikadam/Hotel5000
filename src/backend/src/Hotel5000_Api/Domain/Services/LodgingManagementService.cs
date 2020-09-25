using Core.Entities.Domain;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain;
using Core.Interfaces.Domain.LodgingManagementService;
using Core.Results;
using Domain.Services.LodgingManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class LodgingManagementService : ILodgingManagementService
    {
        private readonly ILodgingService _lodgingService;
        private readonly IRoomService _roomService;
        private readonly IReservationWindowService _reservationWindowService;
        private readonly ILodgingAddressService _lodgingAddressService;
        public LodgingManagementService(IAuthorization authorizationService,
            IUserManagementService userManagementService,
            IAsyncRepository<Lodging> lodgingRepository,
            IAsyncRepository<LodgingType> lodgingTypeRepository,
            IAsyncRepository<LodgingAddress> lodgingAddressRepository,
            IAsyncRepository<ReservationWindow> reservationWindowRepository,
            IAsyncRepository<Room> roomRepository,
            IAsyncRepository<Country> countryRepository,
            IAsyncRepository<Currency> currecyRepository)
        {
            _lodgingAddressService = new LodgingAddressService(_lodgingService,
                authorizationService,
                countryRepository,
                lodgingAddressRepository);

            _lodgingService = new LodgingService(authorizationService,
                lodgingTypeRepository,
                userManagementService,
                _roomService,
                _reservationWindowService,
                _lodgingAddressService,
                lodgingRepository);

            _reservationWindowService = new ReservationWindowService(_lodgingService, authorizationService, reservationWindowRepository);

            _roomService = new RoomService(_lodgingService, authorizationService, roomRepository, currecyRepository);
        }
        public async Task<Result<bool>> AddLodging(Lodging lodging, string lodgingType)
        {
            return await _lodgingService.AddLodging(lodging, lodgingType);
        }

        public async Task<Result<bool>> AddLodgingAddress(LodgingAddress lodgingAddress, string countryCode)
        {
            return await _lodgingAddressService.AddLodgingAddress(lodgingAddress, countryCode);
        }

        public async Task<Result<bool>> AddReservationWindow(ReservationWindow reservationWindow)
        {
            return await _reservationWindowService.AddReservationWindow(reservationWindow);
        }

        public async Task<Result<bool>> AddRoom(Room room, string currencyName)
        {
            return await _roomService.AddRoom(room, currencyName);
        }

        public async Task<Result<IReadOnlyList<Country>>> GetCountry(int? id = null, string name = null, string code = null)
        {
            return await _lodgingAddressService.GetCountry(id, name, code);
        }

        public async Task<Result<IReadOnlyList<Currency>>> GetCurrency(int? id = null, string name = null)
        {
            return await _roomService.GetCurrency(id, name);
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
            return await _lodgingService.GetLodging(id, name, lodgingType, reservableFrom, reservableTo, address, owner, skip, take);
        }

        public async Task<Result<IReadOnlyList<LodgingAddress>>> GetLodgingAddress(int? id = null,
            int? lodgingId = null,
            string countryCode = null,
            string countryName = null,
            string county = null,
            string city = null,
            string postalCode = null,
            string lodgingName = null)
        {
            return await _lodgingAddressService.GetLodgingAddress(id, lodgingId, countryCode, countryName, county, city, postalCode, lodgingName);
        }

        public async Task<Result<IReadOnlyList<ReservationWindow>>> GetReservationWindow(int? id = null,
            int? lodgingId = null,
            DateTime? isAfter = null,
            DateTime? isBefore = null)
        {
            return await _reservationWindowService.GetReservationWindow(id, lodgingId, isAfter, isBefore);
        }
        public async Task<Result<IReadOnlyList<Room>>> GetRoom(int? id = null,
            int? lodgingId = null,
            int? adultCapacity = null,
            int? childrenCapacity = null,
            double? priceMin = null,
            double? priceMax = null)
        {
            return await _roomService.GetRoom(id, lodgingId, adultCapacity, childrenCapacity, priceMin, priceMax);
        }
        public async Task<Result<bool>> RemoveLodging(int lodgingId)
        {
            return await _lodgingService.RemoveLodging(lodgingId);
        }

        public async Task<Result<bool>> RemoveLodgingAddress(int lodgingAddressId)
        {
            return await _lodgingAddressService.RemoveLodgingAddress(lodgingAddressId);
        }

        public async Task<Result<bool>> RemoveReservationWindow(int reservationWindowId)
        {
            return await _reservationWindowService.RemoveReservationWindow(reservationWindowId);
        }

        public async Task<Result<bool>> RemoveRoom(int roomId)
        {
            return await _roomService.RemoveRoom(roomId);
        }
        public async Task<Result<bool>> UpdateLodging(Lodging newLodging, int oldLodgingId)
        {
            return await _lodgingService.UpdateLodging(newLodging, oldLodgingId);
        }

        public async Task<Result<bool>> UpdateLodgingAddress(LodgingAddress newLodgingAddress, int oldLodgingAddressId)
        {
            return await _lodgingAddressService.UpdateLodgingAddress(newLodgingAddress, oldLodgingAddressId);
        }

        public async Task<Result<bool>> UpdateReservationWindow(ReservationWindow newReservationWindow, int oldReservationWindowId)
        {
            return await _reservationWindowService.UpdateReservationWindow(newReservationWindow, oldReservationWindowId);
        }

        public async Task<Result<bool>> UpdateRoom(Room newRoom, int oldRoomId)
        {
            return await _roomService.UpdateRoom(newRoom, oldRoomId);
        }
    }
}
