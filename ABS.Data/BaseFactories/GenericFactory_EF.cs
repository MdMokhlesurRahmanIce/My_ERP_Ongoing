using ABS.Data.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Data.BaseFactories
{
    public abstract class GenericFactory_EF<C, T> : iGenericFactory_EF<T>
        where T : class
        where C : DbContext, new()
    {
        private C Context = new C();
        private DbSet<T> _dbset;

        protected C _dbctx
        {
            get { return Context; }
            set { Context = value; }
        }

        public GenericFactory_EF()
        {
            _dbset = _dbctx.Set<T>();
        }

        #region ===========readonly=========
        //public virtual T GetById(T entity)
        //{
        //    return _dbset.Find(entity);
        //}

        public virtual IQueryable<T> GetAllQbl()
        {
            IQueryable<T> query = _dbset;
            return query;
        }
        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T> query = _dbset;
            return query;
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate);
            return query;
        }

        public bool HasData(Expression<Func<T, bool>> predicate)
        {
            return FindBy(predicate).Any();
        }
        
        public int getMaxVal_int(string strColName, string strTblName)
        {
            string sql = string.Format("SELECT ISNULL(MAX([{0}]),0) AS [{0}] FROM {1}", strColName, strTblName);
            int max = _dbctx.Database.SqlQuery<int>(sql).SingleOrDefault();
            return Convert.ToInt16(max + 1);
        }
        
        public Int64 getMaxVal_int64(string strColName, string strTblName)
        {
            string sql = string.Format("SELECT ISNULL(MAX([{0}]),0) AS [{0}] FROM {1}", strColName, strTblName);
            Int64 max = _dbctx.Database.SqlQuery<Int64>(sql).SingleOrDefault();
            return Convert.ToInt64(max + 1);
        }
        
        public string getMaxID(string strTblName)
        {
            string sql = string.Format("Exec SPMaxRowID '" + strTblName + "'");
            string max = Convert.ToString(_dbctx.Database.SqlQuery<string>(sql).SingleOrDefault());
            return max;
        }

        public void updateMaxID(string strTblName, Int64 maxID)
        {
            string sql = string.Format("Exec SPUpdateMaxRowID '" + strTblName + "','" + maxID.ToString() + "'");
            _dbctx.Database.ExecuteSqlCommand(sql);
        }

        public string getCustomCode(int customCodeID, DateTime formatDate, int companyID, int userID, int locationID)
        {
            string sql = string.Format("Exec SpCustomCode '" + customCodeID + "','" + formatDate + "','" + companyID + "','" + userID + "','" + locationID + "'");
            string max = _dbctx.Database.SqlQuery<string>(sql).SingleOrDefault();
            return max;
        }
        
        public void updateCustomCode(int customCodeID, DateTime formatDate, int companyID, int userID, int locationID)
        {
            string sql = string.Format("Exec SpUpdateCustomCode '" + customCodeID + "','" + formatDate + "','" + companyID + "','" + userID + "','" + locationID + "'");
            _dbctx.Database.ExecuteSqlCommand(sql);
        }

        public decimal getStockQty(int colName)
        {
            string sql = string.Format("Exec GET_TotalStockQty '" + colName + "'");
            decimal StockQty = _dbctx.Database.SqlQuery<decimal>(sql).SingleOrDefault();
            return StockQty;
        }

        public DateTime getSystemDate()
        {
            string sql = string.Format("Exec GET_SystemDate");
            DateTime sysdate = _dbctx.Database.SqlQuery<DateTime>(sql).SingleOrDefault();
            return sysdate;
        }
        

        public void updateStockQty(int itemId, decimal remainingQty)
        {
            string sql = string.Format("Exec GET_UpdateStockQty '" + itemId + "','" + remainingQty + "'");
            _dbctx.Database.ExecuteSqlCommand(sql);
        }

        public string GetConstruction(long ItemId)
        {
            string sql = string.Format("select [dbo].[CmnItemConstruction]({0})", ItemId);
            return _dbctx.Database.SqlQuery<string>(sql).SingleOrDefault();
        }

        public Int32 getMaxValBySp(string spName, object[] parameters)
        {
            string EachPlace = ""; Int32 max = 0;
            StringBuilder PlaceHolder = new StringBuilder();
            int countParameter = parameters.Count();
            for (int i = 0; i < countParameter; i++)
            {
                if (i == 0) { EachPlace = "{" + i + "}"; }
                else { EachPlace = ",{" + i + "}"; }
                PlaceHolder.Append(EachPlace);
            }
            spName = spName + " " + PlaceHolder;
            string sql = string.Format(spName, parameters);
            string generatedId = _dbctx.Database.SqlQuery<string>(sql).SingleOrDefault();
            return max = Convert.ToInt32(generatedId);
        }
        #endregion

        #region ======crud operations=======
        public virtual void Insert(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void InsertList(IEnumerable<T> entities)
        {
            _dbset.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            _dbctx.Entry(entity).State = EntityState.Modified;
        }
        public virtual void UpdateList(IEnumerable<T> entities)
        {
            try
            {
                foreach (var item in entities)
                {
                    _dbctx.Entry(item).State = EntityState.Modified;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void DeleteList(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> list = _dbset.Where(predicate);
            foreach (var entity in list)
            {
                _dbset.Remove(entity);
            }
        }

        public virtual void Save()
        {
            _dbctx.SaveChanges();
        }

        #endregion

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
