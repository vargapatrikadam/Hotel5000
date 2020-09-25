using Core.Entities.Authentication;
using Core.Entities.Domain;
using Core.Enums;
using Core.Enums.Authentication;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain.UserManagementService;
using Core.Interfaces.PasswordHasher;
using Core.Results;
using Domain.Specifications.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.UserManagement
{
    class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAsyncRepository<Role> _roleRepository;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAuthorization _authorizationService;
        public UserService(IPasswordHasher passwordHasher, IAsyncRepository<Role> roleRepository, IAsyncRepository<User> userRepository, IAuthorization authorizationService)
        {
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _authorizationService = authorizationService;
        }
        public async Task<Result<bool>> AddUser(User user, string role)
        {
            if (await _userRepository.AnyAsync(p => p.Email == user.Email))
                return new ConflictResult<bool>(Errors.EMAIL_NOT_UNIQUE);

            if (await _userRepository.AnyAsync(p => p.Username == user.Username))
                return new ConflictResult<bool>(Errors.USERNAME_NOT_UNIQUE);

            user.Email.ValidateEmail(out var errorMessage);
            user.Password.ValidatePassword(out errorMessage);
            if (errorMessage != null)
                return new InvalidResult<bool>(errorMessage.Value);

            user.Password = _passwordHasher.Hash(user.Password);

            RoleType roleAsEnum;
            if (!Enum.TryParse(role, out roleAsEnum))
            {
                return new InvalidResult<bool>(Errors.ROLE_NOT_EXISTS);
            }

            var specification = new GetRole(roleAsEnum);

            var roleEntity = (await _roleRepository.GetAsync(specification)).FirstOrDefault();

            user.Role = null;

            user.RoleId = roleEntity.Id;

            await _userRepository.AddAsync(user);

            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<IReadOnlyList<User>>> GetUsers(int? id = null,
           string username = null,
           string email = null,
           int? skip = null,
           int? take = null)
        {
            var specification = new GetUser(id, username, email, skip, take);

            return new SuccessfulResult<IReadOnlyList<User>>(
                (await _userRepository.GetAsync(specification)));
        }
        public async Task<Result<bool>> RemoveUser(int userId)
        {
            User user = (await GetUsers(id: userId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(User)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(user.Id));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (user == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            await _userRepository.DeleteAsync(user);
            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> UpdateUser(User newUser,
            int userId)
        {
            User updateThisUser = (await GetUsers(id: userId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(User)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(updateThisUser.Id));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (updateThisUser == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            if ((updateThisUser.Email != newUser.Email) && await _userRepository.AnyAsync(p => p.Email == newUser.Email))
                return new ConflictResult<bool>(Errors.EMAIL_NOT_UNIQUE);

            if ((updateThisUser.Username != newUser.Username) && await _userRepository.AnyAsync(p => p.Username == newUser.Username))
                return new ConflictResult<bool>(Errors.USERNAME_NOT_UNIQUE);

            newUser.Email.ValidateEmail(out var errorMessage);
            if (errorMessage != null)
                return new InvalidResult<bool>(errorMessage.Value);

            updateThisUser.Email = newUser.Email;
            updateThisUser.FirstName = newUser.FirstName;
            updateThisUser.LastName = newUser.LastName;
            updateThisUser.Username = newUser.Username;

            await _userRepository.UpdateAsync(updateThisUser);

            return new SuccessfulResult<bool>(true);
        }
        public async Task<Result<bool>> ChangePassword(int userId,
            string oldPassword,
            string newPassword)
        {
            User user = (await GetUsers(id: userId)).Data.FirstOrDefault();

            if (user == null)
                return new NotFoundResult<bool>(Errors.USER_NOT_FOUND);

            if (!_passwordHasher.Check(user.Password, oldPassword))
                return new InvalidResult<bool>(Errors.PASSWORD_INCORRECT);
            else
            {
                user.Password = _passwordHasher.Hash(newPassword);
                await _userRepository.UpdateAsync(user);
                return new SuccessfulResult<bool>(true);
            }
        }
    }
}
