using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> condition = null);

        Task<T> GetByIdAsync(Guid id, string[] includeFields = null);

        Task Create(params T[] entity);

        Task Update(T entity);

        Task Delete(T entity);
    }
}