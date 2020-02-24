using Core.Entities.LodgingEntities;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Lodging.UserManagementService
{
    public interface IUserService
    {
        Task<Result<IReadOnlyList<User>>> GetUsers(int? id = null, string username = null, string email = null);
        Task<Result<bool>> UpdateUser(User newUser, int oldUserId, int resourceAccessorId);
        Task<Result<bool>> RemoveUser(int userId, int resourceAccessorId);
        Task<Result<bool>> AddUser(User newUser, string role);
        Task<Result<bool>> ChangePassword(int userId, string oldPassword, string newPassword);
    }
}
