using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Accounting
{
    public class vmFinishingProcessPriceSetup
    {
        public int? ProcessPriceID { get; set; }
        public string ProcessPriceNo { get; set; }
        public string FinishingProcessName { get; set; }
        public int? FinishingProcessID { get; set; }
        public decimal? Price { get; set; }
        public int? CurrencyID { get; set; }

        public string CurrencyName { get; set; } 

        public DateTime? PriceDate { get; set; }
        public bool? IsActive { get; set; }
        public string Remarks { get; set; }
        public int? CompanyID { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatePc { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool? IsDeleted { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string DeletePc { get; set; }
    }
}
