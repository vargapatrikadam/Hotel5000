using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.PasswordHasher;

namespace Infrastructure.Data
{
    public static class LodgingDBContextSeed
    {
        public static async Task SeedAsync(LodgingDBContext Context, IPasswordHasher passwordHasher, bool isProduction)
        {
            //Only run this if using a real database
            if(isProduction)
                Context.Database.Migrate();

            if (!Context.Roles.Any())
            {
                Context.Roles.AddRange(
                    GetPreconfiguredRoles());

                await Context.SaveChangesAsync();
            }

            if (!Context.PaymentTypes.Any())
            {
                Context.PaymentTypes.AddRange(
                    GetPreconfiguredPaymentTypes());

                await Context.SaveChangesAsync();
            }
            if (!Context.Users.Any())
            {
                Context.Users.AddRange(
                    GetPreconfiguredUsers(passwordHasher));

                await Context.SaveChangesAsync();
            }
        }
        private static IEnumerable<Role> GetPreconfiguredRoles()
        {
            return new List<Role>()
            {
                new Role() { Name = Roles.APPROVED_USER},
                new Role() { Name = Roles.COMPANY},
                new Role() { Name = Roles.ADMIN}
            };
        }
        private static IEnumerable<PaymentType> GetPreconfiguredPaymentTypes()
        {
            return new List<PaymentType>()
            {
                new PaymentType() { Name = PaymentTypes.CASH},
                new PaymentType() { Name = PaymentTypes.BANKCARD},
                new PaymentType() { Name = PaymentTypes.CASH_PART},
                new PaymentType() { Name = PaymentTypes.BANKCARD_PART}
            };
        }
        private static IEnumerable<User> GetPreconfiguredUsers(IPasswordHasher passwordHasher)
        {
            return new List<User>()
            {
                new User()
                {
                    Username = "preuser1",
                    Password = passwordHasher.Hash("Preuser1password"),
                    RoleId = 1,
                    Email = "preuser1@email.com",
                    FirstName = "preuser1firstname",
                    LastName = "preuser1lastname"
                },
                new User()
                {
                    Username = "preuser2",
                    Password = passwordHasher.Hash("Preuser2password"),
                    RoleId = 2,
                    Email = "preuser2@email.com",
                    FirstName = "preuser2firstname",
                    LastName = "preuser2lastname"
                },
                new User()
                {
                    Username = "preuser3",
                    Password = passwordHasher.Hash("Preuser3password"),
                    RoleId = 3,
                    Email = "preuser3@email.com",
                    FirstName = "preuser3firstname",
                    LastName = "preuser3lastname"
                }
            };
        }
    }
}
