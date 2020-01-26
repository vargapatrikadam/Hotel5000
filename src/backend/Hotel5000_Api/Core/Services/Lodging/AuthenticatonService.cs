using Core.Entities.Lodging;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Lodging;
using Core.Specifications;
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
            User user = (await UserRepository.GetAsync(
                new Specification<User>()
                .ApplyFilter(p => (p.Email == email || p.Username == username) && p.Password == password)))
                .FirstOrDefault();

            //This way we don't need to implement a replica of ThenInclude from EF Core, because we cause eager loading on entities from the context.
            //Specification<User> spec = new Specification<User>()
            //    .Include(p => p.Lodgings
            //        .Select(s => s.Rooms
            //            .Select(s => s.ReservationWindows)));
            return user.WithoutPassword();
        }

        public async void Register(User user)
        {
            string errorMessage = null;
            user.Email.ValidateEmail(out errorMessage);
            user.Password.ValidatePassword(out errorMessage);
            if (errorMessage != null)
                throw new ArgumentException(errorMessage);

            await UserRepository.AddAsync(user);
        }
    }
}
