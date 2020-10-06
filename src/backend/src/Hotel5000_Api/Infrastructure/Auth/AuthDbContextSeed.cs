using Core.Entities.Authentication;
using Core.Entities.Domain;
using Core.Enums.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Auth
{
    public class AuthDbContextSeed
    {
        public static async Task SeedAsync(AuthDbContext context, bool isProduction)
        {
            if (context.Database.IsSqlServer())
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
                new BaseRole() {Name = RoleType.APPROVED_USER, CanEditOthers = false},
                new BaseRole() {Name = RoleType.COMPANY, CanEditOthers = false},
                new BaseRole() {Name = RoleType.ADMIN, CanEditOthers = true},
                new BaseRole() {Name = RoleType.ANONYMOUS, CanEditOthers = false}
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
                #region all of the rules
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 1, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 2, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 3, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 1, OperationId = 4, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 1, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 2, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 3, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 2, OperationId = 4, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 1, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 2, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 3, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 3, OperationId = 4, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 1, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 2, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 3, EntityId = 19, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 1, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 2, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 3, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 4, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 5, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 6, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 7, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 8, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 9, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 10, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 11, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 12, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 13, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 14, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 15, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 16, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 17, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 18, IsAllowed = true},
                new Rule() {RoleId = 4, OperationId = 4, EntityId = 19, IsAllowed = true}
#endregion
            };
        }

    }
}
