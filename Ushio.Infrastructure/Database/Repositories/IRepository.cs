using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ushio.Infrastructure.Database.Repositories
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entityList);
        Task<T> UpdateAsync(T entity);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task<int> SaveChangesAsync();
    }
}
