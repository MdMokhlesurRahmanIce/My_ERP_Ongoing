using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmBallConsumption
    {
        public int? Unit { get; set; }
        public long? BallConsumptionID { get; set; }
        public long? YarnCountID { get; set; }
        public long? LotID { get; set; }
        public long? SupplierID { get; set; }
        public long? DepartmentID { get; set; }
        public decimal? LengthM { get; set; }
        public decimal? LengthYds { get; set; }
        public decimal? Qty { get; set; }
        public string Remarks { get; set; }
        public string YarnCount { get; set; }
        public string UOMName { get; set; }
        public int? SlNo { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string ModelState { get; set; }
    }
}
