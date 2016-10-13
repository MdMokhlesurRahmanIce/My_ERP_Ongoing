using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ABS.Service.Sample.Interfaces
{
    public interface iCustomerMgt
    {
        IEnumerable<tbl_Customer> GetCustomers(int? pageNumber, int? pageSize, int? IsPaging);
        tbl_Customer GetCustomerByID(int? id);
        int SaveCustomer(tbl_Customer model);
        int UpdateCustomer(tbl_Customer model);
        int DeleteCustomer(int? CustomerID);
    }
}
