using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications.LodgingManagement
{
    public class GetLodging : Specification<Lodging>
    {
        public GetLodging(int? id = null,
            string name = null,
            LodgingTypes? lodgingType = null,
            DateTime? reservableFrom = null,
            DateTime? reservableTo = null,
            string address = null,
            string owner = null,
            Country country = null,
            int? skip = null,
            int? take = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (name == null || p.Name == name) &&
                            (lodgingType == null || p.LodgingType.Name == lodgingType) &&
                            (!reservableFrom.HasValue || p.ReservationWindows.Any(p => reservableFrom >= p.From && p.To > reservableFrom)) &&
                            (!reservableTo.HasValue || p.ReservationWindows.Any(p => reservableTo >= p.From && p.To > reservableTo)) &&
                            (owner == null || p.User.Email == owner || p.User.Username == owner) &&
                            (address == null || p.LodgingAddresses.Any(p => p.County.StartsWith(address) ||
                                   p.City.StartsWith(address) ||
                                   p.PostalCode.StartsWith(address) ||
                                   p.Street.StartsWith(address)) ||
                                   (country != null &&
                                   p.LodgingAddresses.Any(p => p.CountryId == country.Id))));
            Query
                .Include(i => i.LodgingType);
            Query
                .Include(i => i.User);
            Query
                .Include(i => i.LodgingAddresses)
                    .ThenInclude(i => i.Country);
            Query
                .Include(i => i.Rooms)
                    .ThenInclude(i => i.Currency);
            Query
                .Include(i => i.ReservationWindows);

            if (skip.HasValue && take.HasValue)
                Query.Paginate(skip.Value, take.Value);
        }
    }
}
