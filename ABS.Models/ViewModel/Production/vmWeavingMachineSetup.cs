using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmWeavingMachineSetup
    {
        public long MachineSetupID { get; set; }
        public Nullable<long> ItemID { get; set; }
        public string ArticleNo { get; set; }
        public Nullable<decimal> Selvedge { get; set; }
        public Nullable<decimal> Brackrest { get; set; }
        public Nullable<decimal> ShadeAngle { get; set; }
        public Nullable<decimal> SFHight { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int CompanyID { get; set; }
    }
}
