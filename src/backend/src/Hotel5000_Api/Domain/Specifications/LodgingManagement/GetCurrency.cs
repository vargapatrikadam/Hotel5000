using Ardalis.Specification;
using Core.Entities.Domain;

namespace Domain.Specifications.LodgingManagement
{
    public class GetCurrency : Specification<Currency>
    {
        public GetCurrency(int? id = null,
            string name = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (name == null || p.Name == name));
        }
    }
}
