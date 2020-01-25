using Core.Entities.Lodging;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Specifications.Lodging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Lodging
{
    public class AuthenticatonService : IAuthenticaton
    {
        private readonly IAsyncRepository<User> UserRepository;
        public AuthenticatonService(IAsyncRepository<User> userRepository)
        {
            UserRepository = userRepository;
        }
        public async Task<User> AuthenticateAsync(string username, string password, string email)
        {
            return (await UserRepository.GetAsync(new AuthenticateSpecification(username, password, email))).FirstOrDefault();
        }

        public async void Register(User user)
        {
            await UserRepository.AddAsync(user);
        }
    }
}
