using ABS.Data.BaseInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Data.BaseFactories
{
    public abstract class GenericFactory_Async<C, T> : iGenericFactory_Async<T> where T : class where C : DbContext, new()
    {
        private C _dbctx = new C();
        protected C Context
        {
            get { return _dbctx; }
            set { _dbctx = value; }
        }

        public async virtual Task<dynamic> ExecuteCommandAsync(string spQuery, Hashtable ht)
        {
            dynamic result = null;
            try
            {
                using (_dbctx.Database.Connection)
                {
                    _dbctx.Database.Connection.Open();
                    DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                    cmd.CommandText = spQuery;
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        cmd.Parameters.Add(parameter);
                    }

                    var scalar = await cmd.ExecuteScalarAsync();
                    if (scalar != null)
                    {
                        result = scalar.ToString();
                    }

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString(); e.StackTrace.ToString();
                _dbctx.Database.Connection.Close();
            }
            finally
            {
                _dbctx.Database.Connection.Close();
            }
            return result;
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync(string spQuery, Hashtable ht)
        {
            IEnumerable<T> Results = null;
            try
            {
                using (_dbctx.Database.Connection)
                {
                    _dbctx.Database.Connection.Open();
                    DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                    cmd.CommandText = spQuery;
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        cmd.Parameters.Add(parameter);
                    }

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Results = ReaderToList<T>(reader);
                    }

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString(); e.StackTrace.ToString();
                _dbctx.Database.Connection.Close();
            }
            finally
            {
                _dbctx.Database.Connection.Close();
            }
            return Results;
        }

        public async virtual Task<T> GetByIdAsync(string spQuery, Hashtable ht)
        {
            T Results = null;
            try
            {
                using (_dbctx.Database.Connection)
                {
                    _dbctx.Database.Connection.Open();
                    DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                    cmd.CommandText = spQuery;
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (object obj in ht.Keys)
                    {
                        string str = Convert.ToString(obj);
                        SqlParameter parameter = new SqlParameter("@" + str, ht[obj]);
                        cmd.Parameters.Add(parameter);
                    }

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Results = ReaderToList<T>(reader).FirstOrDefault();
                    }

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString(); e.StackTrace.ToString();
                _dbctx.Database.Connection.Close();
            }
            finally
            {
                _dbctx.Database.Connection.Close();
            }
            return Results;
        }

        private List<T> ReaderToList<Tentity>(IDataReader reader)
        {
            var results = new List<T>();

            var columnCount = reader.FieldCount;
            while (reader.Read())
            {
                var item = Activator.CreateInstance<T>();
                try
                {
                    var rdrProperties = Enumerable.Range(0, columnCount).Select(i => reader.GetName(i)).ToArray();
                    foreach (var property in typeof(T).GetProperties())
                    {
                        if ((typeof(T).GetProperty(property.Name).GetGetMethod().IsVirtual) || (!rdrProperties.Contains(property.Name)))
                        {
                            continue;
                        }
                        else
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                            }
                        }
                    }
                    results.Add(item);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return results;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed)
                if (disposing)
                    _dbctx.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
