using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdBallMachineSetup
    {
        public long? LUserID { get; set; }
        public long? LCompanyID { get; set; }
        public long? LMenuID { get; set; }
        public long? LTransactionTypeID { get; set; }

        public int? BallMachineSetupID { get; set; }
        public int? Count { get; set; }
        public Nullable<decimal> Jog { get; set; }
        public Nullable<decimal> RFront { get; set; }
        public Nullable<decimal> RRear { get; set; }
        public Nullable<decimal> Agm { get; set; }
        public Nullable<decimal> Empty { get; set; }
        public Nullable<decimal> Speed { get; set; }
        public bool IsDeleted { get; set; }
    }
}
