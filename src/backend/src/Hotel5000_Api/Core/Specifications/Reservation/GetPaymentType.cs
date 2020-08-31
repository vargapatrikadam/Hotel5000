using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Reservation
{
    public class GetPaymentType : Specification<PaymentType>
    {
        public GetPaymentType(PaymentTypes paymentType)
        {
            Query
                .Where(p => p.Name == paymentType);
        }
    }
}
