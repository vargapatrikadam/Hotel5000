﻿using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Reservation
{
    public class IsReservationAvailable : Specification<ReservationItem>
    {
        public void SetFilter(ReservationItem newReservationItem)
        {
            Query
                .Where(existing => (existing.RoomId == newReservationItem.RoomId) &&
                                   (existing.ReservedFrom < newReservationItem.ReservedTo && 
                                       newReservationItem.ReservedFrom < existing.ReservedTo));
        }
    }
}
