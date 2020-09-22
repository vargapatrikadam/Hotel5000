using Core.Entities.Authentication;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Authentication
{
    public interface IAuthorization
    {
        Task<Result<bool>> Authorize(AuthorizeAction action);
    }
}
