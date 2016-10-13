using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Data.BaseInterfaces
{
    public interface iGenericFactory_Async<T> : IDisposable where T : class
    {
        Task<dynamic> ExecuteCommandAsync(string spQuery, Hashtable ht);
        Task<IEnumerable<T>> GetAllAsync(string spQuery, Hashtable ht);
        Task<T> GetByIdAsync(string spQuery, Hashtable ht);
    }
}
