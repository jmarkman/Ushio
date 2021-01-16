using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ushio.Infrastructure.Database.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Get(int id);
        Task<IEnumerable<T>> All();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
        Task<int> SaveChanges();
    }
}
