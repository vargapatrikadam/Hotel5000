using Core.Entities.Domain;
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
        private static LodgingDbContext GetNewLodgingDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LodgingDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new LodgingDbContext(options);
        }
        public static IAsyncRepository<User> GetTestUserRepository(string dbName)
        {
            return new LodgingDbRepository<User>(GetNewLodgingDbContext(dbName));
        }
        public static IAsyncRepository<Token> GetTestTokenRepository(string dbName)
        {
            return new LodgingDbRepository<Token>(GetNewLodgingDbContext(dbName));
        }
    }
}
