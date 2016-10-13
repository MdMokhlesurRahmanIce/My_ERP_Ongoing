using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmPrdDyingMachineSetup
    {
        
        public Int64 SetupID { get; set; }
        
        public Int64 ItemID { get; set; }
        
        public Int64 MechineID { get; set; }
        
        public decimal Speed { get; set; }
        
        public decimal Moiture { get; set; }
        
        public decimal KGPreMin { get; set; }
        
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
    }
}
