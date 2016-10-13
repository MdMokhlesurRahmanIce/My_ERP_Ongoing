using ABS.Data.BaseInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Data.BaseFactories
{
    public abstract class GenericFactory<C, T> : iGenericFactory<T> where T : class where C : DbContext, new()
    {
        private C _dbctx = new C();
        protected C Context
        {
            get { return _dbctx; }
            set { _dbctx = value; }
        }

        /// <summary>
        /// Insert/Update/Delete Data To Database
        /// Get int Data after CRUD
        /// <para>Use it when to Insert/Update/Delete data through a stored procedure</para>
        /// </summary>
        public virtual int ExecuteCommand(string spQuery, Hashtable ht)
        {
            int result = 0;
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

                    IDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        result = dr.GetInt32(0);
                    }
                    cmd.Parameters.Clear();
                    _dbctx.Database.Connection.Close();
                    _dbctx.Database.Connection.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
                //e.Message.ToString();
                //e.StackTrace.ToString();
            }
            return result;
        }

        /// <summary>
        /// Insert/Update/Delete Data To Database
        /// Get int Data after CRUD
        /// <para>Use it when to Insert/Update/Delete data through a stored procedure</para>
        /// </summary>
        public virtual string ExecuteCommandString(string spQuery, Hashtable ht)
        {
            string result = string.Empty;
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

                    IDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        result = Convert.ToString(dr.GetString(0));
                    }

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                throw e;
                //e.Message.ToString();
                //e.StackTrace.ToString();
            }
            return result;
        }

        /// <summary>
        /// Insert/Update/Delete Data To Database
        /// Get List Data after CRUD
        /// <para>Use it when to Insert/Update/Delete data through a stored procedure</para>
        /// </summary>
        public virtual IEnumerable<T> ExecuteCommandList(string spQuery, Hashtable ht)
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

                    Results = DataReaderMapToList<T>(cmd.ExecuteReader());
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                throw e;
                //e.Message.ToString();
                //e.StackTrace.ToString();
            }
            return Results;
        }

        /// <summary>
        /// Insert/Update/Delete Data To Database
        /// Get Single Data after CRUD
        /// <para>Use it when to Insert/Update/Delete data through a stored procedure</para>
        /// </summary>
        public virtual T ExecuteCommandSingle(string spQuery, Hashtable ht)
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

                    Results = DataReaderMapToList<T>(cmd.ExecuteReader()).FirstOrDefault();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                throw e;
                //e.Message.ToString();
                //e.StackTrace.ToString();
            }
            return Results;
        }



        /// <summary>
        /// Get Single Data From Database
        /// <para>Use it when to retive single data through a stored procedure</para>
        /// </summary>
        public virtual T ExecuteQuerySingle(string spQuery, Hashtable ht)
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

                    Results = DataReaderMapToList<T>(cmd.ExecuteReader()).FirstOrDefault();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                throw e;
                //e.Message.ToString();
                //e.StackTrace.ToString();
            }

            return Results;
        }

        public void getMaxVal_int(string strTblName)
        {

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


        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through a stored procedure</para>
        /// </summary>
        public virtual IEnumerable<T> ExecuteQuery(string spQuery, Hashtable ht)
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

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        Results = DataReaderMapToList<T>(reader).ToList();
                    }

                    cmd.Parameters.Clear();
                    _dbctx.Database.Connection.Close();
                    _dbctx.Database.Connection.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
                //e.Message.ToString();
                //e.StackTrace.ToString();
            }
            return Results;
        }
        public virtual List<T> DataReaderMapToList<Tentity>(IDataReader reader)
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


        private object NlChk(object val)
        {
            if (val == null)
                return DBNull.Value;
            return val;
        }

        public virtual IEnumerable<object> ExecuteQueryObjectType(string spQuery, Hashtable ht)
        {
            IEnumerable<object> returnList = null;
            using (_dbctx.Database.Connection)
            {
                _dbctx.Database.Connection.Open();
                DbCommand cmd = _dbctx.Database.Connection.CreateCommand();
                cmd.CommandText = spQuery;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (object obj in ht.Keys)
                {
                    string str = Convert.ToString(obj);
                    SqlParameter parameter = new SqlParameter("@" + NlChk(str), ht[obj]);
                    cmd.Parameters.Add(parameter);
                }

                IDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();

                var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                DataRow _RowHeader = dt.NewRow();
                foreach (var name in names)
                {
                    dt.Columns.Add(name);
                    _RowHeader[name] = name;
                }
                dt.Rows.Add(_RowHeader);
                foreach (IDataRecord record in reader as IEnumerable)
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;
                    DataRow row = dt.NewRow();
                    foreach (var name in names)
                    {
                        row[name] = record[name];
                        expando[name] = record[name];
                    }
                    dt.Rows.Add(row);
                }
                returnList = dt.AsEnumerable().ToList();
             
                ///Second
                var results = new List<T>();
                var columnCount = reader.FieldCount;
                while (reader.Read())
                {
                    var item = Activator.CreateInstance<T>();
                    var rdrProperties = Enumerable.Range(0, columnCount).Select(i => reader.GetName(i)).ToArray();
                    results.Add(item);
                }
                cmd.Parameters.Clear();
                _dbctx.Database.Connection.Close();
                _dbctx.Database.Connection.Dispose();
            }

            return returnList;
        }
    }

}
