using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdBallMRRDetail
    {
        public long BalMRRDetailID { get; set; }
        public long BalMRRID { get; set; }
        public Nullable<int> OutputUnitID { get; set; }
        public Nullable<int> BalMachineStopID { get; set; }
        public Nullable<int> TotalStop { get; set; }
        public Nullable<long> BallBreackageMasterID { get; set; }
        public Nullable<int> TotalBreakage { get; set; }
        public Nullable<decimal> SetLength { get; set; }
        public Nullable<long> MachineID { get; set; }
        public Nullable<int> ShiftID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Nullable<int> MachineSpeed { get; set; }
        public Nullable<int> OperatorID { get; set; }
        public Nullable<int> ShiftEngineerID { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> WarpingDate { get; set; }
        public int CompanyID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatePc { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }

        public virtual CmnItemMaster CmnItemMaster { get; set; }
        public virtual CmnUser CmnUser { get; set; }
        public virtual CmnUser CmnUser1 { get; set; }
        public virtual HRMShift HRMShift { get; set; }
        public virtual PrdBallMRRMaster PrdBallMRRMaster { get; set; }
        public virtual PrdOutputUnit PrdOutputUnit { get; set; }
    }
}
