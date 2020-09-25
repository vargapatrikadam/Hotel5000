using Ardalis.Specification;
using Core.Entities.Domain;

namespace Domain.Specifications.LodgingManagement
{
    public class GetLodgingAddress : Specification<LodgingAddress>
    {
        public GetLodgingAddress(int? id = null,
            int? lodgingId = null,
            string countryCode = null,
            string countryName = null,
            string county = null,
            string city = null,
            string postalCode = null,
            string lodgingName = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                            (countryCode == null || p.Country.Code == countryCode) &&
                            (countryName == null || p.Country.Name == countryName) &&
                            (county == null || p.County == county) &&
                            (city == null || p.City == city) &&
                            (postalCode == null || p.PostalCode == postalCode) &&
                            (lodgingName == null || p.Lodging.Name == lodgingName));
            Query
                .Include(i => i.Country);
            Query
                .Include(i => i.Lodging);
        }
    }
}
