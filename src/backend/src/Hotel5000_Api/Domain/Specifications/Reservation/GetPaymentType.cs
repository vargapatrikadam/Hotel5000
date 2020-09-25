using Ardalis.Specification;
using Core.Entities.Domain;
using Core.Enums.Lodging;

namespace Domain.Specifications.Reservation
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
