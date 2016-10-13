using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdLCBMRRMasterDetail
    {
        //public long SetID { get; set; }
        public string ArticleNo { get; set; }
        public long SetLength { get; set; }
        public string ColorName { get; set; }
        public string BuyerName { get; set; }
        public string YarnCount { get; set; }
        public string YarnRatioLot { get; set; }
        public int NoOfBall { get; set; }
        public decimal LeaseRepeat { get; set; }
        public int PIID { get; set; }
        public string PINO { get; set; }
        public decimal TotalEnds { get; set; }
        public string YarnRatio { get; set; }
        public decimal EndsPerCreel { get; set; }
        public string SupplierName { get; set; }
        //public long ItemID { get; set; }

        //Main Master
        public long LCBMRRID { get; set; }
        public Nullable<System.DateTime> LCBMRRDate { get; set; }
        public string MachineName { get; set; }
        public Nullable<long> ItemID { get; set; }
        public string ItemName { get; set; }
        public Nullable<long> SetID { get; set; }
        public string SetNo { get; set; }
        public string Description { get; set; }
        public string LCBMRRNo { get; set; }

        //Main Detail
        public long LCBMRRDetailID { get; set; }
        public Nullable<int> OutputUnitID { get; set; }
        public Nullable<long> LCBMachineStopMasterID { get; set; }
        public Nullable<int> TotalStop { get; set; }
        public Nullable<long> LCBBreakageMasterID { get; set; }
        public Nullable<int> TotalBreakage { get; set; }
        public Nullable<long> MachineID { get; set; }
        public Nullable<decimal> BeamLength { get; set; }        
        public Nullable<int> ShiftID { get; set; }
        public System.DateTime DDate { get; set; }
        public string DDateString { get; set; }
        public System.DateTime BeginTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public Nullable<int> MachineSpeed { get; set; }
        public Nullable<int> OperatorID { get; set; }
        public Nullable<int> ShiftEngineerID { get; set; }

        //StopDetail
        public Nullable<int> LCBMachineStopID { get; set; }
        public Nullable<int> BWSID { get; set; }
        public string BWSName { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        //public string StartTime { get; set; }
        public int StopInMin { get; set; }

        //BreakageDetail
        public Nullable<int> LCBBreakageID { get; set; }
        //public Nullable<int> BWSID { get; set; }
        public Nullable<int> NoOfBreakage { get; set; }
        public Nullable<int> SlNo { get; set; }//Breakage
        public Nullable<int> SNo { get; set; }//MachineStop
        public bool IsDeleted { get; set; }
        public bool IsNextDate { get; set; }
        public string ModelState { get; set; }
    }
}
