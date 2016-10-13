using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models;



namespace ABS.Service.Sample.Interfaces
{
   public interface iEmpMasterMgt
    {

        IEnumerable<EmpMaster> GetEmployee(int? pageNumber, int? pageSize, int? IsPaging);
        EmpMaster GetEmployeeByID(int? id);
        int SaveEmployee(EmpMaster model);
        int UpdateEmployee(EmpMaster model);
        int DeleteEmployee(int? EmployeeID);

    }
}
