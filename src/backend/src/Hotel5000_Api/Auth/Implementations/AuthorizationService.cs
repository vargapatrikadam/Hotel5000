using Auth.Specifications.Authentication;
using Auth.Specifications.Authorization;
using Core.Entities.Authentication;
using Core.Entities.LodgingEntities;
using Core.Enums;
using Core.Enums.Authentication;
using Core.Helpers.Results;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Implementations
{
    public class AuthorizationService : IAuthorization
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncRepository<BaseRole> _baseRoleRepository;
        private readonly IAsyncRepository<Operation> _operationRepository;
        private readonly IAsyncRepository<Entity> _entityRepository;
        private readonly IAsyncRepository<Rule> _ruleRepository;
        private readonly IUserIdentity _userIdentity;
        public AuthorizationService(IAsyncRepository<User> userRepository,
            IAsyncRepository<BaseRole> baseRoleRepository,
            IAsyncRepository<Operation> operationRepository,
            IAsyncRepository<Entity> entityRepository,
            IAsyncRepository<Rule> ruleRepository,
            IUserIdentity userIdentity)
        {
            _userRepository = userRepository;
            _baseRoleRepository = baseRoleRepository;
            _operationRepository = operationRepository;
            _entityRepository = entityRepository;
            _ruleRepository = ruleRepository;
            _userIdentity = userIdentity;
        }
        private async Task<Result<bool>> CanModify(int resourceOwnerId, int accessingUserId, BaseRole initiatingRole)
        {
            var getResourceOwnerSpecification = new GetUserByIdWithRole(resourceOwnerId);
            User resourceOwner = (await _userRepository.GetAsync(getResourceOwnerSpecification)).FirstOrDefault();
            if (resourceOwner == null)
                return new NotFoundResult<bool>(Errors.RESOURCE_OWNER_NOT_FOUND);

            var getAccessingUserSpecification = new GetUserByIdWithRole(accessingUserId);
            User accessingUser = (await _userRepository.GetAsync(getAccessingUserSpecification)).FirstOrDefault();
            if (accessingUser == null)
                return new NotFoundResult<bool>(Errors.ACCESSING_USER_NOT_FOUND);

            if (initiatingRole.CanEditOthers
                || accessingUser.Id == resourceOwner.Id)
                return new SuccessfulResult<bool>(true);
            else return new UnauthorizedResult<bool>(Errors.UNAUTHORIZED);
        }

        public async Task<Result<bool>> Authorize(AuthorizeAction action)
        {
            var getAccessingUserSpecification = new GetUserByIdWithRole(int.Parse(_userIdentity.Username));
            User accessingUser = (await _userRepository.GetAsync(getAccessingUserSpecification)).FirstOrDefault();
            if (_userIdentity.Role != RoleType.ANONYMOUS && accessingUser == null)
                return new NotFoundResult<bool>(Errors.ACCESSING_USER_NOT_FOUND);

            var getRoleSpecification = new GetRoleByName(accessingUser.Role.Name);
            BaseRole initiatingRole = (await _baseRoleRepository.GetAsync(getRoleSpecification)).FirstOrDefault();
            if (initiatingRole == null)
                return new NotFoundResult<bool>(Errors.ROLE_NOT_EXISTS);

            var getOperationSpecification = new GetOperationByAction(action.Operation);
            Operation initiatedOperation = (await _operationRepository.GetAsync(getOperationSpecification)).FirstOrDefault();
            if (initiatedOperation == null)
                //TODO: new error message in enum for operation
                return new NotFoundResult<bool>(Errors.UNDEFINED);

            var getEntitySpecification = new GetEntityByName(action.EntityName);
            Entity initiatedOnEntity = (await _entityRepository.GetAsync(getEntitySpecification)).FirstOrDefault();
            if (initiatedOnEntity == null)
                //TODO: new error message in enum for entity
                return new NotFoundResult<bool>(Errors.UNDEFINED);

            var getRuleSpecification = new GetRuleByIds(initiatingRole.Id,
                initiatedOperation.Id,
                initiatedOnEntity.Id);

            Rule rule = (await _ruleRepository.GetAsync(getRuleSpecification)).FirstOrDefault();
            bool isAuthorized =
                rule != null ? rule.IsAllowed : false;

            if (!isAuthorized)
                return new UnauthorizedResult<bool>(Errors.UNAUTHORIZED);

            Result<bool> canModify = await CanModify(action.ResourceOwnerId, int.Parse(_userIdentity.Username), initiatingRole);
            if (!canModify.Data)
                return canModify;

            return new SuccessfulResult<bool>(true);
        }
    }
}
