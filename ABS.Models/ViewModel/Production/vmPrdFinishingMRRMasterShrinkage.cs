using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdFinishingMRRMasterShrinkage
    {
        //Main Master
        public long? FinishingMRRID { get; set; }
        public string FinishingMRRNo { get; set; }
        public long? FinishingMRRTypeID { get; set; }        
        public DateTime? FinishingMRRDate { get; set; }
        public long? ItemID { get; set; }
        public string ArticleNo { get; set; }
        public long? SetID { get; set; }
        public long? SizeMRRID { get; set; }
        public long? WeavingMRRID { get; set; }
        public string WeavingMRRNo { get; set; }
        public long? BuyerID { get; set; }
        public string BuyerName { get; set; }
        public long? PIID { get; set; }
        public string PINO { get; set; }
        public long? FinishingTypeID { get; set; }
        public string FInishTypeName { get; set; }
        public string FinishingProcessName { get; set; }
        public long? MachineID { get; set; }
        public string MachineName { get; set; }
        public int? ShiftID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal? Length { get; set; }
        public int? UnitID { get; set; }
        public int? OperatorID { get; set; }
        public int? ShiftEngineerID { get; set; }
        public string Remarks { get; set; } 

        //Main Detail
        public long? FinishingMRRShrinkageID { get; set; }
        public decimal? ReqWeight { get; set; }
        public decimal? FiniWeight { get; set; }
        public decimal? AWWeight { get; set; }
        public string GreigeEPIPPI { get; set; }
        public decimal? FiniEPI { get; set; }
        public decimal? FiniPPI { get; set; }
        public string AWEPIPPI { get; set; }
        public decimal? CuttableWidth { get; set; }
        public decimal? FiniWidth { get; set; }
        public decimal? AWWidth { get; set; }
        public string WReqd { get; set; }
        public decimal? LShrinkage { get; set; }
        public decimal? WShrinkage { get; set; }
        public decimal? FiniSkew { get; set; }        
        public decimal? AWSkew { get; set; }
        public decimal? MovSkew { get; set; }
        public bool IsAWPercent { get; set; }
        public bool IsFSPercent { get; set; }
        public bool IsMovPercent { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsNextDate { get; set; }
        public string ModelState { get; set; }
    }
}
