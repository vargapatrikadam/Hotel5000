using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.LodgingManagement
{
    public class GetReservationWindow : Specification<ReservationWindow>
    {
        public GetReservationWindow(int? id = null,
            int? lodgingId = null,
            DateTime? isAfter = null,
            DateTime? isBefore = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (!lodgingId.HasValue || p.LodgingId == lodgingId.Value) &&
                            (!isAfter.HasValue || p.From >= isAfter.Value) &&
                            (!isBefore.HasValue || p.To >= isBefore.Value));
            Query
                .Include(i => i.Lodging);
            Query
                .Include(i => i.ReservationItems)
                    .ThenInclude(i => i.Room);
        }
    }
}
