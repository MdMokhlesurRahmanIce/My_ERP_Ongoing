using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.LocalSales
{
    public class vmLSalesInvoiceDetail
    {
        public long SIDetailID { get; set; }
        public long SIID { get; set; }
        public Nullable<long> ItemID { get; set; }
        public Nullable<long> BatchID { get; set; }
        public Nullable<long> LotID { get; set; }
        public Nullable<long> GradeID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string Status { set; get; }
    }
}
