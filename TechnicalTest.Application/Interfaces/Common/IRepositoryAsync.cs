using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechnicalTest.Domain.Contracts;

namespace TechnicalTest.Application.Interfaces.Common
{
    public interface IRepositoryAsync<T> where T : class, IEntity
    {
        DbSet<T> Entities { get; }

        Task<T> GetByIdAsync(object id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

        Task<T> GetByPredicate(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);

        Task<bool> AddRangeAsync(List<T> entities);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
