using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Lodging
{
    public interface IAuthenticatonService
    {
        Task<Token> AuthenticateAsync(string username, string password, string email);
        Task<Token> RefreshAsync(string refreshToken);
        Task<bool> RegisterAsync(User user);
    }
}
