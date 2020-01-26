using Core.Entities.LodgingEntities;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class LodgingDBContextSeed
    {
        public static async Task SeedAsync(LodgingDBContext Context, bool isProduction)
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
        }
        private static IEnumerable<Role> GetPreconfiguredRoles()
        {
            return new List<Role>()
            {
                new Role() { Name = "APPROVED_USER"},
                new Role() { Name = "COMPANY"},
                new Role() { Name = "ADMIN"}
            };
        }
        private static IEnumerable<PaymentType> GetPreconfiguredPaymentTypes()
        {
            return new List<PaymentType>()
            {
                new PaymentType() { Name = "CASH"},
                new PaymentType() { Name = "BANKCARD"},
                new PaymentType() { Name = "CASH_PART"},
                new PaymentType() { Name = "BANKCARD_PART"}
            };
        }
    }
}
