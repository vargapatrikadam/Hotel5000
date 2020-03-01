using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using Core.Interfaces.PasswordHasher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            context.SaveChanges();

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

            if (!context.Lodgings.Any())
            {
                context.Lodgings.AddRange(
                    GetPreconfiguredLodgings());

                await context.SaveChangesAsync();
            }

            if (!context.LodgingAddresses.Any())
            {
                context.LodgingAddresses.AddRange(
                    GetPreconfiguredLodgingAddresses());

                await context.SaveChangesAsync();
            }

            if (!context.PaymentTypes.Any())
            {
                context.PaymentTypes.AddRange(
                    GetPreconfiguredPaymentTypes());

                await context.SaveChangesAsync();
            }

            if (!context.Reservations.Any())
            {
                context.Reservations.AddRange(
                    GetPreconfiguredReservations());

                await context.SaveChangesAsync();
            }

            if (!context.Currencies.Any())
            {
                context.Currencies.AddRange(
                    GetPreconfiguredCurrencies());

                await context.SaveChangesAsync();
            }

            if (!context.Rooms.Any())
            {
                context.Rooms.AddRange(
                    GetPreconfiguredRooms());

                await context.SaveChangesAsync();
            }

            if (!context.ReservationWindows.Any())
            {
                context.ReservationWindows.AddRange(
                    GetPreconfiguredReservationWindows());

                await context.SaveChangesAsync();
            }

            if (!context.ReservationItems.Any())
            {
                context.ReservationItems.AddRange(
                    GetPreconfiguredReservationItems());

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
        private static IEnumerable<Lodging> GetPreconfiguredLodgings()
        {
            return new List<Lodging>()
            {
                new Lodging()
                {
                    Name = "Test lodging for preuser1 approved user",
                    UserId = 3,
                    LodgingTypeId = 1
                },
                new Lodging()
                {
                    Name = "Test lodging 1 for preuser2 company",
                    UserId = 2,
                    LodgingTypeId = 2
                },
                new Lodging()
                {
                    Name = "Test lodging 2 for preuser2 company",
                    UserId = 2,
                    LodgingTypeId = 2
                }
            };
        }
        private static IEnumerable<LodgingAddress> GetPreconfiguredLodgingAddresses()
        {
            return new List<LodgingAddress>()
            {
                new LodgingAddress()
                {
                    CountryId = 2,
                    County = "Banská Bystrica Region",
                    City = "Banská Bystrica",
                    PostalCode = "97401",
                    Street = "teststreet",
                    HouseNumber = "1",
                    LodgingId = 2
                },
                new LodgingAddress()
                {
                    CountryId = 2,
                    County = "Nitra Region",
                    City = "Nitra",
                    PostalCode = "94901",
                    Street = "teststreet2",
                    HouseNumber = "2",
                    LodgingId = 3
                },
                new LodgingAddress()
                {
                    CountryId = 1,
                    County = "Nógrád",
                    City = "Balassagyarmat",
                    PostalCode = "2660",
                    Street = "teststreet3",
                    HouseNumber = "3",
                    LodgingId = 1
                }
            };
        }
        private static IEnumerable<Reservation> GetPreconfiguredReservations()
        {
            return new List<Reservation>()
            {
                new Reservation()
                {
                    Email = "test1@test.com",
                    PaymentTypeId = 3
                },
                new Reservation()
                {
                    Email = "test2@test.com",
                    PaymentTypeId = 2
                }
            };
        }
        private static IEnumerable<Currency> GetPreconfiguredCurrencies()
        {
            return new List<Currency>()
            {
                new Currency()
                {
                    Name = "Forint"
                },
                new Currency()
                {
                    Name = "Euro"
                }
            };
        }
        private static IEnumerable<Room> GetPreconfiguredRooms()
        {
            return new List<Room>()
            {
                new Room()
                {
                    AdultCapacity = 2,
                    ChildrenCapacity = 0,
                    Price = 5000,
                    CurrencyId = 1,
                    LodgingId = 1
                },
                new Room()
                {
                    AdultCapacity = 2,
                    ChildrenCapacity = 2,
                    Price = 20,
                    CurrencyId = 2,
                    LodgingId = 2
                },
                new Room()
                {
                    AdultCapacity = 2,
                    ChildrenCapacity = 0,
                    Price = 15,
                    CurrencyId = 2,
                    LodgingId = 2
                },
                new Room()
                {
                    AdultCapacity = 4,
                    ChildrenCapacity = 2,
                    Price = 40,
                    CurrencyId = 2,
                    LodgingId = 3
                },
                new Room()
                {
                    AdultCapacity = 2,
                    ChildrenCapacity = 0,
                    Price = 20,
                    CurrencyId = 2,
                    LodgingId = 3
                },
                new Room()
                {
                    AdultCapacity = 2,
                    ChildrenCapacity = 1,
                    Price = 25,
                    CurrencyId = 2,
                    LodgingId = 3
                }
            };
        }
        private static IEnumerable<ReservationWindow> GetPreconfiguredReservationWindows()
        {
            return new List<ReservationWindow>()
            {
                new ReservationWindow()
                {
                    From = new DateTime(2020,3,1),
                    To = new DateTime(2020,4,1),
                    LodgingId = 1
                },
                new ReservationWindow()
                {
                    From = new DateTime(2020,5,2),
                    To = new DateTime(2020,9,1),
                    LodgingId = 2
                },
                new ReservationWindow()
                {
                    From = new DateTime(2020,5,20),
                    To = new DateTime(2020,8,20),
                    LodgingId = 3
                },
            };
        }
        private static IEnumerable<ReservationItem> GetPreconfiguredReservationItems()
        {
            return new List<ReservationItem>()
            {
                new ReservationItem()
                {
                    ReservationId = 1,
                    ReservationWindowId = 3,
                    RoomId = 1,
                    ReservedFrom = new DateTime(2020, 4, 2),
                    ReservedTo = new DateTime(2020, 4,30)
                },
                new ReservationItem()
                {
                    ReservationId = 2,
                    ReservationWindowId = 2,
                    RoomId = 3,
                    ReservedFrom = new DateTime(2020, 5, 3),
                    ReservedTo = new DateTime(2020, 5, 10)
                },
                new ReservationItem()
                {
                    ReservationId = 2,
                    ReservationWindowId = 1,
                    RoomId = 6,
                    ReservedFrom = new DateTime(2020, 5,20),
                    ReservedTo = new DateTime(2020,6,1)
                }
            };
        }
    }
}