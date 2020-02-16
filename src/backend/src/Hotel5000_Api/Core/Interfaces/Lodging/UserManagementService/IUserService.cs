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
        Task<Result<IReadOnlyList<User>>> GetAllUsers();
        Task<Result<User>> GetUser(int userId);
        Task<Result<bool>> UpdateUser(User user, int userId);
        Task<Result<bool>> RemoveUser(int userId);
        Task<Result<bool>> AddUser(User user, string role);
    }
}
