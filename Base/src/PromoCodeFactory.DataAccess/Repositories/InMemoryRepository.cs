using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected List<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data.ToList();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            Data.Add(entity);
            return Task.FromResult(entity);
        }
        public Task<T> UpdateAsync(T entity)
        {
            var index = Data.FindIndex(x => x.Id == entity.Id);
            if (index == -1)
                throw new ArgumentException("Entity not found");

            Data[index] = entity;
            return Task.FromResult(entity);
        }

        public Task DeleteAsync(Guid id)
        {
            var entity = Data.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                throw new ArgumentException("Entity not found");

            Data.Remove(entity);
            return Task.CompletedTask;
        }
    }
}