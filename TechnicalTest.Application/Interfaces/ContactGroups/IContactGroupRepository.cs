using TechnicalTest.Shared.Models.ContactGroups;
using TechnicalTest.Shared.Wrapper;

namespace TechnicalTest.Application.Interfaces.ContactGroups
{
    public interface IContactGroupRepository
    {
        Task<Result<bool>> AddContactGroup(ContactGroupAddRequest request, CancellationToken cancellationToken = default);

        Task<Result<bool>> EditContactGroup(ContactGroupEditRequest request, CancellationToken cancellationToken = default);

        Task<List<ContactGroupGetResponse>> GetAllContactGroups(CancellationToken cancellationToken = default);
    }
}
