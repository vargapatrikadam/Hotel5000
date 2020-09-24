using Core.Entities.Domain;
using Core.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Domain.LodgingManagementService
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
            int oldLodgingAddressId);

        Task<Result<bool>> RemoveLodgingAddress(int lodgingAddressId);

        Task<Result<bool>> AddLodgingAddress(LodgingAddress lodgingAddress, string countryCode);

        Task<Result<IReadOnlyList<Country>>> GetCountry(int? id = null,
            string name = null,
            string code = null);
    }
}
