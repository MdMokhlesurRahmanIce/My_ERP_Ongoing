using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Data.BaseInterfaces
{
    public interface iGenericFactory<T> : IDisposable where T : class
    {
        int ExecuteCommand(string spQuery, Hashtable hTable);
        string ExecuteCommandString(string spQuery, Hashtable ht);
        IEnumerable<T> ExecuteCommandList(string spQuery, Hashtable ht);
        T ExecuteCommandSingle(string spQuery, Hashtable ht);
        IEnumerable<T> ExecuteQuery(string spQuery, Hashtable ht);
        T ExecuteQuerySingle(string spQuery, Hashtable ht);
        IEnumerable<object> ExecuteQueryObjectType(string spQuery, Hashtable ht);

    }
}
