using Core.Entities.Domain;
using Core.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Domain.UserManagementService
{
    public interface IUserService
    {
        Task<Result<IReadOnlyList<User>>> GetUsers(int? id = null,
            string username = null,
            string email = null,
            int? skip = null,
            int? take = null);
        Task<Result<bool>> UpdateUser(User newUser,
            int userId);
        Task<Result<bool>> RemoveUser(int userId);
        Task<Result<bool>> AddUser(User newUser,
            string role);
        Task<Result<bool>> ChangePassword(int userId,
            string oldPassword,
            string newPassword);
    }
}
