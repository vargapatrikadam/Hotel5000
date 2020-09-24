using Ardalis.Specification;
using Core.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Specifications.LodgingManagement
{
    public class GetRoom : Specification<Room>
    {
        public GetRoom(int? id = null,
            int? lodgingId = null,
            int? adultCapacity = null,
            int? childrenCapacity = null,
            double? priceMin = null,
            double? priceMax = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                            (!adultCapacity.HasValue || p.AdultCapacity == adultCapacity.Value) &&
                            (!childrenCapacity.HasValue || p.ChildrenCapacity == childrenCapacity.Value) &&
                            (!priceMin.HasValue || p.Price <= priceMin.Value) &&
                            (!priceMax.HasValue || p.Price >= priceMax.Value));
            Query
                .Include(i => i.Currency);
            Query
                .Include(i => i.ReservationItems);
            Query
                .Include(i => i.Lodging);
        }
    }
}
