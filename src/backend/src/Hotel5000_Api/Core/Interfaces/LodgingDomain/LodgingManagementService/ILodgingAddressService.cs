using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain.LodgingManagementService
{
    public interface ILodgingAddressService
    {
        Task<Result<IReadOnlyList<LodgingAddress>>> GetLodgingAddress(int? id = null,
            int? lodgingId = null,
            string countryCode = null,
            string countryName = null,
            string county = null,
            string city = null,
            string postalCode = null,
            string lodgingName = null);

        Task<Result<bool>> UpdateLodgingAddress(LodgingAddress newLodgingAddress,
            int oldLodgingAddressId,
            int resourceAccessorId);

        Task<Result<bool>> RemoveLodgingAddress(int lodgingAddressId,
            int resourceAccessorId);

        Task<Result<bool>> AddLodgingAddress(LodgingAddress lodgingAddress, string countryCode, int resourceAccessorId);

        Task<Result<IReadOnlyList<Country>>> GetCountry(int? id = null,
            string name = null,
            string code = null);
    }
}
