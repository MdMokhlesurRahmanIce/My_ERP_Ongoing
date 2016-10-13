using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Commercial
{
    public class vmSalDCMasterDetail
    {
        public long DCDetailID { get; set; }
        public Nullable<long> DCID { get; set; }
        public Nullable<decimal> QuantityYds { get; set; }
        public int CompanyID { get; set; }
        public int NoOfDC { get; set; }
        public string DCNo { get; set; }
        public Nullable<System.DateTime> DCDate { get; set; }
        public Nullable<int> BankID { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
