using TechnicalTest.Domain.Contracts;

namespace TechnicalTest.Application.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepositoryAsync<T> Repository<T>() where T : class, IEntity;

        public Task<int> Commit(CancellationToken cancellationToken);

        public Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        public Task Rollback();
    }
}
