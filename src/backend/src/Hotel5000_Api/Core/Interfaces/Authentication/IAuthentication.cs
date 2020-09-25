using Core.Entities.Domain;
using Core.Results;
using System.Threading.Tasks;

namespace Core.Interfaces.Authentication
{
    public interface IAuthentication
    {
        Task<Result<User>> AuthenticateAsync(string identifier, string password);
        Task<Result<bool>> LogoutAsync(string refreshToken);
        Task<Result<User>> RefreshAsync(string refreshToken);
    }
}
