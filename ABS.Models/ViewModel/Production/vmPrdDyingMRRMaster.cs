using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdDyingMRRMaster
    {

        public Int64 DyingMRRID { get; set; }

        public string DyingMRRNo { get; set; }

        public string DyingSetNo { get; set; }

        public Int64? MachineID { get; set; }
        public string MachineConfigNo { get; set; }
        public Int64 ItemID { get; set; }
        public String ArticleNo { get; set; }

        public decimal KGPreMin { get; set; }

        public decimal Moiture { get; set; }

        public decimal Speed { get; set; }

        public decimal TotalLength { get; set; }

        public int ShiftID { get; set; }
        public string ShiftName { get; set; }

        public TimeSpan EndTime { get; set; }
         
        public TimeSpan StartTime { get; set; }
        public string Description { get; set; }
        public int? BuyerID { get; set; }
        public Int64? RefSetID { get; set; }
        public DateTime? RefSetDate { get; set; }
        public DateTime Date { get; set; }

        public int CompanyID { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatePc { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UpdatePc { get; set; }
         
        public bool IsDeleted { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string DeletePc { get; set; }
        public String EntityState { get; set; }

    }
}
