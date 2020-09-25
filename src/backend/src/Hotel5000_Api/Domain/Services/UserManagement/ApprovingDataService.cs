using Core.Entities.Authentication;
using Core.Entities.Domain;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Authentication;
using Core.Interfaces.Domain.UserManagementService;
using Core.Results;
using Domain.Specifications.UserManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.UserManagement
{
    class ApprovingDataService : IApprovingDataService
    {
        private readonly IAsyncRepository<ApprovingData> _approvingDataRepository;
        private readonly IAuthorization _authorizationService;
        public ApprovingDataService(IAsyncRepository<ApprovingData> approvingDataRepository,
            IAuthorization authorizationService)
        {
            _approvingDataRepository = approvingDataRepository;
            _authorizationService = authorizationService;
        }
        public async Task<Result<bool>> AddApprovingData(ApprovingData newApprovingData)
        {
            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ApprovingData)),
                new Operation(Operation.Type.CREATE),
                new EntityOwner(newApprovingData.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (await _approvingDataRepository.AnyAsync(p => p.UserId == newApprovingData.UserId))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_ALREADY_EXISTS);

            if (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == newApprovingData.IdentityNumber
                    || p.RegistrationNumber == newApprovingData.RegistrationNumber
                    || p.TaxNumber == newApprovingData.TaxNumber))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_NOT_UNIQUE);

            await _approvingDataRepository.AddAsync(newApprovingData);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<IReadOnlyList<ApprovingData>>> GetApprovingData(int? approvingDataOwnerId = null,
            int? approvingDataId = null,
            string username = null,
            string taxNumber = null,
            string identityNumber = null,
            string registrationNumber = null)
        {
            var specification = new GetApprovingData(approvingDataOwnerId, approvingDataId, username, taxNumber, identityNumber, registrationNumber);

            IReadOnlyList<ApprovingData> approvingDatas = await _approvingDataRepository.GetAsync(specification);

            return new SuccessfulResult<IReadOnlyList<ApprovingData>>(approvingDatas);
        }

        public async Task<Result<bool>> RemoveApprovingData(int approvingDataOwnerId)
        {
            ApprovingData approvingData = (await GetApprovingData(approvingDataOwnerId: approvingDataOwnerId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ApprovingData)),
                new Operation(Operation.Type.DELETE),
                new EntityOwner(approvingData.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (approvingData == null)
                return new NotFoundResult<bool>(Errors.APPROVING_DATA_NOT_FOUND);

            await _approvingDataRepository.DeleteAsync(approvingData);
            return new SuccessfulResult<bool>(true);
        }

        public async Task<Result<bool>> UpdateApprovingData(ApprovingData newApprovingData,
            int approvingDataId)
        {
            ApprovingData updateThisApprovingData = (await GetApprovingData(approvingDataId: approvingDataId)).Data.FirstOrDefault();

            AuthorizeAction authorizeAction = new AuthorizeAction(
                new Entity(nameof(ApprovingData)),
                new Operation(Operation.Type.UPDATE),
                new EntityOwner(updateThisApprovingData.UserId));
            Result<bool> authorizationResult = await _authorizationService.Authorize(authorizeAction);

            if (!authorizationResult.Data)
                return authorizationResult;

            if (updateThisApprovingData == null)
                return new NotFoundResult<bool>(Errors.APPROVING_DATA_NOT_FOUND);

            if ((updateThisApprovingData.IdentityNumber != newApprovingData.IdentityNumber &&
                    (await _approvingDataRepository.AnyAsync(p => p.IdentityNumber == newApprovingData.IdentityNumber))) ||
                (updateThisApprovingData.RegistrationNumber != newApprovingData.RegistrationNumber &&
                    (await _approvingDataRepository.AnyAsync(p => p.RegistrationNumber == newApprovingData.RegistrationNumber))) ||
                (updateThisApprovingData.TaxNumber != newApprovingData.TaxNumber &&
                    (await _approvingDataRepository.AnyAsync(p => p.TaxNumber == newApprovingData.TaxNumber))))
                return new ConflictResult<bool>(Errors.APPROVING_DATA_NOT_UNIQUE);

            updateThisApprovingData.IdentityNumber = newApprovingData.IdentityNumber;
            updateThisApprovingData.RegistrationNumber = newApprovingData.RegistrationNumber;
            updateThisApprovingData.TaxNumber = newApprovingData.TaxNumber;

            await _approvingDataRepository.UpdateAsync(updateThisApprovingData);

            return new SuccessfulResult<bool>(true);
        }
    }
}
