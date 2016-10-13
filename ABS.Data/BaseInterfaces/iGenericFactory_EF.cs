using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace ABS.Data.BaseInterfaces
{
    public interface iGenericFactory_EF<T> : IDisposable where T : class
    {
        //T GetById(T entity);
        IQueryable<T> GetAllQbl();
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        bool HasData(Expression<Func<T, bool>> predicate);
        int getMaxVal_int(string strColName, string strTblName);
        Int64 getMaxVal_int64(string strColName, string strTblName);
        string getMaxID(string strTblName);
        string GetConstruction(long ItemId);
        decimal getStockQty(int colName);
        DateTime getSystemDate();
        void updateStockQty(int itemId, decimal remainingQty);
        string getCustomCode(int customCodeID, DateTime formatDate, int companyID, int userID, int locationID);
        void updateCustomCode(int customCodeID, DateTime formatDate, int companyID, int userID, int locationID);
        void updateMaxID( string strTblName, Int64 maxID);
        Int32 getMaxValBySp(string spName, object[] parameters);
        void Insert(T entity);
        void InsertList(IEnumerable<T> entity);
        void Update(T entity);
        void UpdateList(IEnumerable<T> entity);
        void Delete(T entity);
        void DeleteList(IEnumerable<T> entity);
        void Delete(Expression<Func<T, bool>> predicate);
        void Save();
    }
}
