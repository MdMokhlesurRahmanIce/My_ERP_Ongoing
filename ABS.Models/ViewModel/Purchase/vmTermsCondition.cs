using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Purchase
{
    public class vmTermsCondition
    {
        public Boolean Selected { get; set; }
        public Nullable<int> Sequence { get; set; }
        public long TermID { get; set; }
        public string Description { get; set; }
        public int CompanyID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public bool IsDeleted { get; set; }
     
     
    }
}
