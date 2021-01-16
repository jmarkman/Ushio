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
        private UshioDbContext dbContext;

        public BaseRepository(UshioDbContext context)
        {
            dbContext = context;
        }

        public async Task<T> Add(T entity)
        {
            var addedItem = await dbContext.AddAsync(entity);
            return addedItem.Entity;
        }

        public async Task<IEnumerable<T>> All()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await dbContext.Set<T>().AsQueryable().Where(expression).ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            return await dbContext.FindAsync<T>(id);
        }

        public async Task<int> SaveChanges()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
