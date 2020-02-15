using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel5000_Api_Tests.UnitTests.Core.Data
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
                    Name = Roles.ApprovedUser
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
