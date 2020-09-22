﻿using Core.Entities.Authentication;
using Core.Entities.LodgingEntities;
using Core.Enums.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth
{
    public class AuthDbContextSeed
    {
        public static async Task SeedAsync(AuthDbContext context, bool isProduction)
        {
            if (isProduction)
                context.Database.Migrate();

            context.SaveChanges();

            if (!context.BaseRoles.Any())
            {
                context.BaseRoles.AddRange(
                    GetPreconfiguredBaseRoles());

                await context.SaveChangesAsync();
            }

            if (!context.Operations.Any())
            {
                context.Operations.AddRange(
                    GetPreconfiguredOperations());

                await context.SaveChangesAsync();
            }

            if (!context.Entities.Any())
            {
                context.Entities.AddRange(
                    GetPreconfiguredEntites());

                await context.SaveChangesAsync();
            }

            if (!context.Rules.Any())
            {
                context.Rules.AddRange(
                    GetPreconfiguredRules());

                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<BaseRole> GetPreconfiguredBaseRoles()
        {
            return new List<BaseRole>()
            {
                new BaseRole() {Name = RoleType.APPROVED_USER},
                new BaseRole() {Name = RoleType.COMPANY},
                new BaseRole() {Name = RoleType.ADMIN },
                new BaseRole() {Name = RoleType.ANONYMOUS }
            };
        }

        private static IEnumerable<Operation> GetPreconfiguredOperations()
        {
            return new List<Operation>()
            {
                new Operation() {Action = Operation.Type.CREATE},
                new Operation() {Action = Operation.Type.READ},
                new Operation() {Action = Operation.Type.UPDATE},
                new Operation() {Action = Operation.Type.DELETE}
            };
        }

        private static IEnumerable<Entity> GetPreconfiguredEntites()
        {
            return new List<Entity>()
            {
                //lodging entites
                new Entity() {Name = nameof(ApprovingData)},
                new Entity() {Name = nameof(Contact)},
                new Entity() {Name = nameof(Country)},
                new Entity() {Name = nameof(Currency)},
                new Entity() {Name = nameof(Lodging)},
                new Entity() {Name = nameof(LodgingAddress)},
                new Entity() {Name = nameof(LodgingType)},
                new Entity() {Name = nameof(PaymentType)},
                new Entity() {Name = nameof(Reservation)},
                new Entity() {Name = nameof(ReservationItem)},
                new Entity() {Name = nameof(ReservationWindow)},
                new Entity() {Name = nameof(Role)},
                new Entity() {Name = nameof(Room)},
                new Entity() {Name = nameof(Token)},
                new Entity() {Name = nameof(User)},

                //auth entites
                new Entity() {Name = nameof(Operation)},
                new Entity() {Name = nameof(Entity)},
                new Entity() {Name = nameof(BaseRole)},
                new Entity() {Name = nameof(Rule)}
            };
        }
        private static IEnumerable<Rule> GetPreconfiguredRules()
        {
            return new List<Rule>()
            {
                //TODO: write rules
            };
        }

    }
}