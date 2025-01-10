using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories;

public sealed class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext dbContext;

    private DbSet<T> DbSet => dbContext.Set<T>();

    public EfRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> condition = null)
    {
        var query = DbSet.AsQueryable();

        if (condition is not null)
            query = query.Where(condition);

        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id, string[] includeFields = null)
    {
        var query = DbSet.AsQueryable();

        if (includeFields != null)
        {
            query = query.AsSplitQuery();
            foreach (var field in includeFields)
                query = query.Include(field);
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Create(params T[] entity)
    {
        DbSet.AddRange(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        DbSet.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        DbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
