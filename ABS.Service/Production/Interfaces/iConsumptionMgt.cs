using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Service.Production.Interfaces
{
   public interface iConsumptionMgt
    {

        IEnumerable<RndConsumptionType> GetConsumptions(int? pageNumber, int? pageSize, int? IsPaging);
        RndConsumptionType GetConsumptionByID(int? id);
        int SaveConsumption(RndConsumptionType model);
        int UpdateConsumption(RndConsumptionType model);
        int DeleteConsumption(RndConsumptionType model);
    }
}
