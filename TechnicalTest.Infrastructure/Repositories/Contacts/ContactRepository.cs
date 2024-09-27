using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechnicalTest.Application.Interfaces.Common;
using TechnicalTest.Application.Interfaces.Contacts;
using TechnicalTest.Domain.Entities;
using TechnicalTest.Shared.Models.Contacts;
using TechnicalTest.Shared.Wrapper;
using static TechnicalTest.Domain.Configurations.AppConstants;

namespace TechnicalTest.Infrastructure.Repositories.Contacts
{
    public class ContactRepository(ILogger<ContactRepository> logger, IServiceProvider serviceProvider, IMapper mapper) : IContactRepository
    {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<Result<bool>> AddContact(ContactAddRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Add Contact");
            using var scope = _serviceProvider.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repository = unitOfWork.Repository<Contact>();
            Contact contact = _mapper.Map<Contact>(request);

            contact.CreatedBy = "Test";
            contact.CreatedDate = DateTime.Now;

            await repository.AddAsync(contact);
            bool isSaved = await unitOfWork.Commit(cancellationToken) > 0;
            if (isSaved)
                return await Result<bool>.SuccessAsync(isSaved, ApiMessages.SavedSuccessfully);
            else
                return await Result<bool>.FailAsync(ApiMessages.Failed);
        }

        public async Task<Result<bool>> EditContact(ContactEditRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Edit Contact");
            using var scope = _serviceProvider.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repository = unitOfWork.Repository<Contact>();

            // Check category type is exist or not in DB
            var contact = await repository.Entities.AsNoTracking().Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (contact == null)
            {
                return await Result<bool>.FailAsync($"{ApiMessages.RecordNotFound} for this Id: {request.Id}");
            }

            contact.ContactType = request.ContactType;
            contact.ModifiedBy = "Test";
            contact.ModifiedDate = DateTime.Now;
            await repository.UpdateAsync(contact);

            bool isSaved = await unitOfWork.Commit(cancellationToken) > 0;
            if (isSaved)
                return await Result<bool>.SuccessAsync(isSaved, ApiMessages.UpdatedSuccessfully);
            else
                return await Result<bool>.FailAsync(ApiMessages.Failed);
        }

        public async Task<Result<bool>> DeleteContact(int id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Delete Contact");
            using var scope = _serviceProvider.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var repository = unitOfWork.Repository<Contact>();

            // Check category type is exist or not in DB
            var contact = await repository.Entities.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (contact == null)
            {
                return await Result<bool>.FailAsync($"{ApiMessages.RecordNotFound} for this Id: {id}");
            }

            await repository.DeleteAsync(contact);

            bool isSaved = await unitOfWork.Commit(cancellationToken) > 0;
            if (isSaved)
                return await Result<bool>.SuccessAsync(isSaved, ApiMessages.DeletedSuccessfully);
            else
                return await Result<bool>.FailAsync(ApiMessages.Failed);
        }

        public async Task<List<ContactGetResponse>> GetAllContacts(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get Contacts");
            using var scope = _serviceProvider.CreateAsyncScope();
            var result = await (from contact in scope.ServiceProvider.GetRequiredService<IRepositoryAsync<Contact>>().Entities.AsNoTracking()
                                join contactGroup in scope.ServiceProvider.GetRequiredService<IRepositoryAsync<ContactGroup>>().Entities.AsNoTracking()
                                on contact.ContactGroupId equals contactGroup.Id
                                select new ContactGetResponse
                                {
                                    Id = contact.Id,
                                    Name = contact.Name,
                                    PhoneNumber = contact.PhoneNumber,
                                    ContactType = contact.ContactType,
                                    ContactGroupId = contact.ContactGroupId,
                                    ContactGroupName = contactGroup.GroupName,
                                }).OrderByDescending(c => c.Id).ToListAsync(cancellationToken: cancellationToken);
            return result;
        }

        public async Task<ContactGetResponse?> GetContactById(int id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get Contact By Id");
            using var scope = _serviceProvider.CreateAsyncScope();
            var result = await (from contact in scope.ServiceProvider.GetRequiredService<IRepositoryAsync<Contact>>().Entities
                                where contact.Id == id
                                join contactGroup in scope.ServiceProvider.GetRequiredService<IRepositoryAsync<ContactGroup>>().Entities.AsNoTracking()
                                on contact.ContactGroupId equals contactGroup.Id
                                select new ContactGetResponse
                                {
                                    Id = contact.Id,
                                    Name = contact.Name,
                                    PhoneNumber = contact.PhoneNumber,
                                    ContactType = contact.ContactType,
                                    ContactGroupId = contact.ContactGroupId,
                                    ContactGroupName = contactGroup.GroupName,
                                }).FirstOrDefaultAsync(cancellationToken);
            return result;
        }
    }
}
