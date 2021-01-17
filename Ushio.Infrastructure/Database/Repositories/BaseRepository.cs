using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ushio.Infrastructure.Database.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected UshioDbContext dbContext;

        public BaseRepository(UshioDbContext context)
        {
            dbContext = context;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var addedItem = await dbContext.AddAsync(entity);
            return addedItem.Entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            dbContext.Update(entity);
            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            dbContext.Remove(entity);
            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await dbContext.Set<T>().AsQueryable().Where(expression).ToListAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await dbContext.FindAsync<T>(id);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
