using ABS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iUnitOfMeasurementMgt
    {
        IEnumerable<CmnUOM> GetUnitOfMeasurement(int? pageNumber, int? pageSize, int? IsPaging);
        IEnumerable<CmnUOMGroup> GetUOMGroup(int? pageNumber, int? pageSize, int? IsPaging);
        int SaveUpdateUnitOfMeasurement(CmnUOM model);
        IEnumerable<CmnUOM> GetUnitOfMeasurementById(int Id);
        int DeleteUnitOfMeasurement(int Id);  
    }
}
