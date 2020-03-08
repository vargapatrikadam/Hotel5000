using Core.Entities.LodgingEntities;
using Core.Interfaces;
using Infrastructure.Lodgings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel5000_Api_Tests.IntegrationTests.Core.Helpers
{
    public static class RepositoryHelpers
    {
        private static LodgingDbContext GetNewLodgingDbContext()
        {
            var options = new DbContextOptionsBuilder<LodgingDbContext>()
                .UseInMemoryDatabase("Lodging test database")
                .Options;

            return new LodgingDbContext(options);
        }
        public static IAsyncRepository<User> GetTestUserRepository()
        {
            return new LodgingDbRepository<User>(GetNewLodgingDbContext());
        }
        public static IAsyncRepository<Token> GetTestTokenRepository()
        {
            return new LodgingDbRepository<Token>(GetNewLodgingDbContext());
        }
    }
}
