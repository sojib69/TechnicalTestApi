using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechnicalTest.Application.Interfaces.Common;
using TechnicalTest.Application.Interfaces.ContactGroups;
using TechnicalTest.Domain.Entities;
using TechnicalTest.Shared.Models.ContactGroups;
using TechnicalTest.Shared.Wrapper;
using static TechnicalTest.Domain.Configurations.AppConstants;

namespace TechnicalTest.Infrastructure.Repositories.ContactGroups
{
    public class ContactGroupRepository(ILogger<ContactGroupRepository> logger, IServiceProvider serviceProvider, IMapper mapper) : IContactGroupRepository
    {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<Result<bool>> AddContactGroup(ContactGroupAddRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Add Contact Group");
            using var scope = _serviceProvider.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repository = unitOfWork.Repository<ContactGroup>();
            ContactGroup contact = _mapper.Map<ContactGroup>(request);

            contact.CreatedBy = "Test";
            contact.CreatedDate = DateTime.Now;

            await repository.AddAsync(contact);
            bool isSaved = await unitOfWork.Commit(cancellationToken) > 0;
            if (isSaved)
                return await Result<bool>.SuccessAsync(isSaved, ApiMessages.SavedSuccessfully);
            else
                return await Result<bool>.FailAsync(ApiMessages.Failed);
        }

        public async Task<Result<bool>> EditContactGroup(ContactGroupEditRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Edit Contact Group");
            using var scope = _serviceProvider.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repository = unitOfWork.Repository<ContactGroup>();

            // Check category type is exist or not in DB
            var contact = await repository.Entities.AsNoTracking().Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (contact == null)
            {
                return await Result<bool>.FailAsync($"{ApiMessages.RecordNotFound} for this Id: {request.Id}");
            }

            contact.ModifiedBy = "Test";
            contact.ModifiedDate = DateTime.Now;
            await repository.UpdateAsync(contact);

            bool isSaved = await unitOfWork.Commit(cancellationToken) > 0;
            if (isSaved)
                return await Result<bool>.SuccessAsync(isSaved, ApiMessages.UpdatedSuccessfully);
            else
                return await Result<bool>.FailAsync(ApiMessages.Failed);
        }

        public async Task<List<ContactGroupGetResponse>> GetAllContactGroups(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get all contact groups");
            using var scope = _serviceProvider.CreateAsyncScope();
            var result = await (from contactGroup in scope.ServiceProvider.GetRequiredService<IRepositoryAsync<ContactGroup>>().Entities.AsNoTracking()
                                select new ContactGroupGetResponse
                                {
                                    Id = contactGroup.Id,
                                    GroupName = contactGroup.GroupName,
                                }).OrderByDescending(c => c.Id).ToListAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
