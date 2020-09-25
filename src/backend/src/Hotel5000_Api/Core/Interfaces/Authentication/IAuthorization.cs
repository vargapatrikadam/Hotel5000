using Core.Entities.Authentication;
using Core.Results;
using System.Threading.Tasks;

namespace Core.Interfaces.Authentication
{
    public interface IAuthorization
    {
        Task<Result<bool>> Authorize(AuthorizeAction action);
    }
}
