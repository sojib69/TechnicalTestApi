using TechnicalTest.Shared.Models.Contacts;
using TechnicalTest.Shared.Wrapper;

namespace TechnicalTest.Application.Interfaces.Contacts
{
    public interface IContactRepository
    {
        Task<Result<bool>> AddContact(ContactAddRequest request, CancellationToken cancellationToken = default);

        Task<Result<bool>> EditContact(ContactEditRequest request, CancellationToken cancellationToken = default);

        Task<Result<bool>> DeleteContact(ContactDeleteRequest request, CancellationToken cancellationToken = default);

        Task<List<ContactGetResponse>> GetAllContacts(CancellationToken cancellationToken = default);

        Task<ContactGetResponse?> GetContactById(int id, CancellationToken cancellationToken = default);
    }
}
