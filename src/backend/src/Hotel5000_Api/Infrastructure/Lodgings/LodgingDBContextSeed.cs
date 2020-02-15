using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.PasswordHasher;

namespace Infrastructure.Lodgings
{
    public static class LodgingDbContextSeed
    {
        public static async Task SeedAsync(LodgingDbContext context, IPasswordHasher passwordHasher, bool isProduction)
        {
            //context.Database.Migrate();
            //Only run this if using a real database
            //if (isProduction)
            //    context.Database.Migrate();
            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    GetPreconfiguredRoles());

                await context.SaveChangesAsync();
            }

            if (!context.PaymentTypes.Any())
            {
                context.PaymentTypes.AddRange(
                    GetPreconfiguredPaymentTypes());

                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    GetPreconfiguredUsers(passwordHasher));

                await context.SaveChangesAsync();
            }

            if (!context.ApprovingData.Any())
            {
                context.ApprovingData.AddRange(
                    GetPreconfiguredApprovingData());

                await context.SaveChangesAsync();
            }

            if (!context.Countries.Any())
            {
                context.Countries.AddRange(
                    GetPreconfiguredCountries());

                await context.SaveChangesAsync();
            }

            if (!context.Contacts.Any())
            {
                context.Contacts.AddRange(
                    GetPreconfiguredContacts());

                await context.SaveChangesAsync();
            }

            if (!context.LodgingTypes.Any())
            {
                context.LodgingTypes.AddRange(
                    GetPreconfiguredLodgingTypes());

                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<Role> GetPreconfiguredRoles()
        {
            return new List<Role>()
            {
                new Role() {Name = Roles.ApprovedUser},
                new Role() {Name = Roles.Company},
                new Role() {Name = Roles.Admin}
            };
        }

        private static IEnumerable<PaymentType> GetPreconfiguredPaymentTypes()
        {
            return new List<PaymentType>()
            {
                new PaymentType() {Name = PaymentTypes.Cash},
                new PaymentType() {Name = PaymentTypes.Bankcard},
                new PaymentType() {Name = PaymentTypes.CashPart},
                new PaymentType() {Name = PaymentTypes.BankcardPart}
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

        private static IEnumerable<ApprovingData> GetPreconfiguredApprovingData()
        {
            return new List<ApprovingData>()
            {
                new ApprovingData()
                {
                    IdentityNumber = "12345678",
                    TaxNumber = "1234567891234",
                    UserId = 3
                },
                new ApprovingData()
                {
                    RegistrationNumber = "123456789123",
                    UserId = 2
                }
            };
        }
        private static IEnumerable<Country> GetPreconfiguredCountries()
        {
            return new List<Country>
            {
                new Country()
                {
                    Code = "HU",
                    Name = "Hungary"
                },
                new Country()
                {
                    Code = "SK",
                    Name = "Slovakia"
                }
            };
        }
        private static IEnumerable<Contact> GetPreconfiguredContacts()
        {
            return new List<Contact>
            {
                new Contact()
                {
                    MobileNumber = "06 30 666 666",
                    UserId = 2
                },
                new Contact()
                {
                    MobileNumber = "06 20 666 666",
                    UserId = 2
                },
                new Contact()
                {
                    MobileNumber = "06 90 555 555",
                    UserId = 3
                }
            };
        }
        private static IEnumerable<LodgingType> GetPreconfiguredLodgingTypes()
        {
            return new List<LodgingType>()
            {
                new LodgingType() {Name = LodgingTypes.Company},
                new LodgingType() {Name = LodgingTypes.Private}
            };
        }
        //private static IEnumerable<Lodging> GetPreconfiguredLodgings()
        //{
        //    return new List<Lodging>()
        //    {
        //        new Lodging()
        //        {
        //            Name = "Test lodging for preuser1 approved user",
        //            UserId = 3,
        //            LodgingTypeId = 
        //        }
        //    }
        //}
    }
}