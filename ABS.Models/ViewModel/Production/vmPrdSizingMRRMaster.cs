using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdSizingMRRMaster
    {
        //Main Master
        public long SizeMRRID { get; set; }
        public Nullable<long> MachineID { get; set; }
        public Nullable<System.DateTime> SizeMRRDate { get; set; }
        public string MachineName { get; set; }
        public Nullable<long> ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<long> SetID { get; set; }
        public string SetNo { get; set; }
        public string Description { get; set; }
        public string SizeMRRNo { get; set; }

        //Main Detail
        public long SizeMRRDetailID { get; set; }
        public Nullable<int> OutputUnitID { get; set; }
        public Nullable<long> SizeMachineStopMasterID { get; set; }
        public Nullable<int> TotalStop { get; set; }
        public Nullable<long> SizeBreakageMasterID { get; set; }
        public Nullable<int> TotalBCBreakage { get; set; }
        public Nullable<int> TotalHSBreakage { get; set; }
        public Nullable<decimal> SqueezingSTD { get; set; }
        public Nullable<decimal> SqueezingActual { get; set; }
        public Nullable<decimal> OverallStretch { get; set; }
        public Nullable<decimal> BeamLength { get; set; }
        public Nullable<int> BeamQualityID { get; set; }
        public Nullable<int> ShiftID { get; set; }
        public System.DateTime BeginTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<int> MachineSpeed { get; set; }
        public Nullable<int> OperatorID { get; set; }
        public Nullable<int> ShiftEngineerID { get; set; }

        //StopDetail
        public Nullable<int> SizeMachineStopID { get; set; }
        public Nullable<int> BWSID { get; set; }
        public string BWSName { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        //public string StartTime { get; set; }
        public int StopInMin { get; set; }

        //BreakageDetail
        public Nullable<int> SizeBreakageID { get; set; }
        //public Nullable<int> BWSID { get; set; }
        public Nullable<int> NoOfBreakage { get; set; }
        public string BreakageType { get; set; }
        public Nullable<int> SlNo { get; set; }//Breakage
        public Nullable<int> SNo { get; set; }//MachineStop
        public bool IsDeleted { get; set; }
        public bool IsNextDate { get; set; }
        public string ModelState { get; set; }
    }
}
