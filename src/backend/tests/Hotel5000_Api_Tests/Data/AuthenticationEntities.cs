using Core.Entities.Domain;
using Core.Enums.Authentication;
using System.Collections.Generic;

namespace Hotel5000_Api_Tests.UnitTests.Data
{
    public static class AuthenticationEntities
    {
        public static User GetTestUser()
        {
            return new User()
            {
                Id = 1,
                Username = "testusername",
                Password = "testpassword",
                Email = "testemail",
                FirstName = "testfirstname",
                LastName = "testlastname",
                Role = new Role()
                {
                    Id = 1,
                    Name = RoleType.APPROVED_USER
                },
                RoleId = 1,
                Contacts = new List<Contact>()
                {
                    new Contact()
                    {
                        Id = 1,
                        MobileNumber = "06 30 555 555",
                        UserId = 1
                    }
                }
            };
        }
    }
}
