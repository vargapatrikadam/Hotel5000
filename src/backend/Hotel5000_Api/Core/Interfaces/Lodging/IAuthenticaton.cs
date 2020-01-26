using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Lodging
{
    public interface IAuthenticaton
    {
        Task<User> AuthenticateAsync(string username, string password, string email);
        void Register(User user);
    }
}
