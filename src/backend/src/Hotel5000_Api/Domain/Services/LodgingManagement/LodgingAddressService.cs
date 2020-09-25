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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.LodgingManagement
{
    class LodgingAddressService : ILodgingAddressService
    {
        private readonly ILodgingService _lodgingService;
        private readonly IAuthorization _authorizationService;
        private readonly IAsyncRepository<Country> _countryRepository;
        private readonly IAsyncRepository<LodgingAddress> _lodgingAddressRepository;
        public LodgingAddressService(ILodgingService lodgingService,
            IAuthorization authorizationService,
            IAsyncRepository<Country> countryRepository,
            IAsyncRepository<LodgingAddress> lodgingAddressRepository)
        {
            _lodgingService = lodgingService;
            _authorizationService = authorizationService;
            _countryRepository = countryRepository;
            _lodgingAddressRepository = lodgingAddressRepository;
        }
        public async Task<Result<bool>> AddLodgingAddress(LodgingAddress lodgingAddress, string countryCode)
        {
            Lodging lodging = ((await _lodgingService.GetLodging(id: lodgingAddress.LodgingId)).Data).FirstOrDefault();

            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(LodgingAddress)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

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
        public async Task<Result<IReadOnlyList<Country>>> GetCountry(int? id = null, string name = null, string code = null)
        {
            var specification = new GetCountry(id, name, code);

            return new SuccessfulResult<IReadOnlyList<Country>>(await _countryRepository.GetAsync(specification));
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
            var specification = new GetLodgingAddress(id,
                lodgingId,
                countryCode,
                countryName,
                county,
                city,
                postalCode,
                lodgingName);

            return new SuccessfulResult<IReadOnlyList<LodgingAddress>>(await _lodgingAddressRepository.GetAsync(specification));
        }
        public async Task<Result<bool>> RemoveLodgingAddress(int lodgingAddressId)
        {
            LodgingAddress removeThisLodgingAddress = (await GetLodgingAddress(id: lodgingAddressId)).Data.FirstOrDefault();
            if (removeThisLodgingAddress == null)
                return new NotFoundResult<bool>(Errors.LODGING_ADDRESS_NOT_FOUND);

            Lodging lodging = (await _lodgingService.GetLodging(id: removeThisLodgingAddress.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(LodgingAddress)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            await _lodgingAddressRepository.DeleteAsync(removeThisLodgingAddress);
            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> UpdateLodgingAddress(LodgingAddress newLodgingAddress, int oldLodgingAddressId)
        {
            LodgingAddress updateThisLodgingAddress = (await GetLodgingAddress(id: oldLodgingAddressId)).Data.FirstOrDefault();
            if (updateThisLodgingAddress == null)
                return new NotFoundResult<bool>(Errors.LODGING_ADDRESS_NOT_FOUND);

            Lodging lodging = (await _lodgingService.GetLodging(id: updateThisLodgingAddress.LodgingId)).Data.FirstOrDefault();
            if (lodging == null)
                return new NotFoundResult<bool>(Errors.LODGING_NOT_FOUND);

            Country newCountry = (await GetCountry(code: newLodgingAddress.Country.Code)).Data.FirstOrDefault();
            if (newCountry == null)
                return new NotFoundResult<bool>(Errors.COUNTRY_NOT_FOUND);

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(LodgingAddress)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(lodging.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

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
    }
}
