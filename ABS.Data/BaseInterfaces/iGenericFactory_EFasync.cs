using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Data.BaseInterfaces
{
    public interface iGenericFactory_EFasync<T> : IDisposable where T : class
    {
        Task<T> GetById(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate);
        void InsertAsync(T entity);
        void InsertListAsync(IEnumerable<T> entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        void DeleteListAsync(IEnumerable<T> entity);
        void DeleteAsync(Expression<Func<T, bool>> predicate);
        Task SaveAsync();
    }
}
