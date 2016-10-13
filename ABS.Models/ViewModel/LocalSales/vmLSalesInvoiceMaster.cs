using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.LocalSales
{
    public class vmLSalesInvoiceMaster
    {
        public long? SIID { get; set; }
        public string SINo { get; set; }
        public Nullable<DateTime> SIDate { get; set; }
        public Nullable<DateTime> DODate { get; set; }
        public Nullable<int> SITypeID { get; set; }
        public Nullable<int> BuyerID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string BuyerName { set; get; }
        public string SalesPerson { set; get; }
        
   

     }
}
