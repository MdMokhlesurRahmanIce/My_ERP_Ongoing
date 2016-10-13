using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmItemSetSetup
    {
        public long? SetupID { get; set; }
        public long? PIID { get; set; }
        public long? ItemID { get; set; }
        public long? BuyerID { get; set; }
        public long? MachineID { get; set; }
        public long? MachinePartID { get; set; }
        public long? SetupDetailID { get; set; }
        public long? MachineOperationID { get; set; }

        //public long MachineOperationID { get; set; }
        public string ArticleNo { get; set; }
        public string ItemName { get; set; }
        public string BuyerName { get; set; }
        public string MachineName { get; set; }
        public string MachinePartName { get; set; }
        public string OperationName { get; set; }
        public Nullable<decimal> SQPress { get; set; }
        public Nullable<decimal> Temp { get; set; }

        public Nullable<decimal> Speed { get; set; }
        public Nullable<decimal> Moiture { get; set; }
        public Nullable<decimal> KGPreMin { get; set; }
        public bool IsDeleted { get; set; }
    }
}
