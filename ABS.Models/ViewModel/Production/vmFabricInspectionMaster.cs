using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmFabricInspectionMaster
    {
        public long? InspactionID { get; set; }
        public string InspactionNo { get; set; }
        public long? FinishingMRRID { get; set; }
        public long? ItemID { get; set; }
        public long? SetID { get; set; }
        public long? WeivingMRRID { get; set; }
        public long? SizeMRRID { get; set; }
        public int? MachineConfigID { get; set; }
        public int? ShiftID { get; set; }
        public int? PlateID { get; set; }
        public int? OperatorID { get; set; }
        public string FinishingMRRNo { get; set; }
        public string ArticleNo { get; set; }
        public string ColorName { get; set; }
        public string SetNo { get; set; }
        public string ShiftName { get; set; }
        public string PlateNo { get; set; }
        public string MachineConfigNo { get; set; }
        public string WeavingMRRNo { get; set; }
        public string UserFullName { get; set; }
        public string Remarks { get; set; }
        public string FabricInspectionDate { get; set; }
    }
}
