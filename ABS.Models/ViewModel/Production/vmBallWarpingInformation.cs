using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmBallWarpingInformation
    {
        //public long LUserID { get; set; }
        //public long LCompanyID { get; set; }
        //public long LMenuID { get; set; }
        //public long LTransactionTypeID { get; set; }
        public long? SetMasterID { get; set; }
        public decimal? PIItemlength { get; set; }
        public int? RefSetID { get; set; }
        public DateTime? RefSetDate { get; set; }
        public DateTime? BalMRRDate { get; set; }
        public string StrWarpingDate { get; set; }
        public DateTime? SizeMRRDate { get; set; }
        public string SetNo { get; set; }
        public string BalMRRNo { get; set; }
        public long? BalMRRID { get; set; }
        public int? Ratio { get; set; }
        public int? Unit { get; set; }
        public long? BallConsumptionID { get; set; }
        public long? YarnCountID { get; set; }
        public long? LotID { get; set; }
        public long? SupplierID { get; set; }
        public long? DepartmentID { get; set; }
        public decimal? LengthM { get; set; }
        public decimal? LengthYds { get; set; }
        public decimal? Qty { get; set; }
        public string Remarks { get; set; }
        public string ArticleYarnCount { get; set; }
        public string UOMName { get; set; }

        public string ArticleNo { get; set; }
        public int? ColorID { get; set; }
        public string ColorName { get; set; }
        public int? PIID { get; set; }
        public string PINO { get; set; }
        public long? SetLength { get; set; }
        public long? LengthPerBall { get; set; }
        public int? YarnID { get; set; }
        public string YarnCount { get; set; }
        public string YarnRatio { get; set; }
        public string YarnRatioLot { get; set; }
        public string SupplierName { get; set; }
        public decimal? TotalEnds { get; set; }
        public int? MachineSpeed { get; set; }
        public decimal? EndsPerCreel { get; set; }
        public decimal? LeaseRepeat { get; set; }
        public int? NoOfBall { get; set; }
        public int? BuyerID { get; set; }
        public string BuyerName { get; set; }
        public DateTime? SetDate { get; set; }
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
        public string PINo { get; set; }
        public string RefSetNo { get; set; }
        public long? SetID { get; set; }
        public int? Count { get; set; }
        public decimal? Jog { get; set; }
        public decimal? RFront { get; set; }
        public int? RRear { get; set; }
        public int? Agm { get; set; }
        public int? OutputID { get; set; }
        public string OutputNo { get; set; }
        public long? ItemID { get; set; }
        public string ItemName { get; set; }
        public List<vmBallInfo> OutputNos { get; set; }
        public List<vmMachineNo> MachineNos { get; set; }
        public List<vmShiftName> ShiftNames { get; set; }
        public List<vmOperator> Operators { get; set; }
        public List<vmOperator> DutyOfficers { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int? cnt { get; set; }
        public long? BalMRRDetailID { get; set; }
        public int? OutputUnitID { get; set; }
        public long? BalMachineStopID { get; set; }
        public int? TotalStop { get; set; }
        public long? BallBreackageMasterID { get; set; }
        public int? TotalBreakage { get; set; }
        public long? MachineID { get; set; }
        public int? ShiftID { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public int? OperatorID { get; set; }
        public int? ShiftEngineerID { get; set; }
        public DateTime? WarpingDate { get; set; }
        public int? CompanyID { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatePc { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string DeletePc { get; set; }
        public bool? IsIssued { get; set; }
        public long? SizeMRRID { get; set; }

        public virtual CmnItemMaster CmnItemMaster { get; set; }
        public virtual CmnUser CmnUser { get; set; }
        public virtual CmnUser CmnUser1 { get; set; }
        public virtual HRMShift HRMShift { get; set; }
        public virtual PrdBallMRRMaster PrdBallMRRMaster { get; set; }
        public virtual PrdOutputUnit PrdOutputUnit { get; set; }
        public int? SlNo { get; set; }//Breakage
        public int? SNo { get; set; }//MachineStop

        public string ModelState { get; set; }
        public long? SizeMachineStopMasterID { get; set; }

    }
}
