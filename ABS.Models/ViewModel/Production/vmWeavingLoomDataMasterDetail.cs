using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmWeavingLoomDataMasterDetail
    {
        public long? LoomRecordDetailID { get; set; }
        public long? LoomRecordID { get; set; }
        public long? LoomStopID { get; set; }
        public long? ShiftEngineerID { get; set; }
        public long? OperatorID { get; set; }
        public Nullable<System.DateTime> ProductionDate { get; set; }
        public string LoomRacordNo { set; get; }

        public long? MachineConfigID { get; set; }
        public string MachineConfigNo { set; get; }
        public int? ShiftID { get; set; }
        public string ShiftName { set; get; }
        public int? LineID { get; set; }
        public string LineName { set; get; }
        public long? SetID { get; set; }
        public long? SizeMRRID { get; set; }
        public string SetNo { set; get; }//This is instance of SizeMRRNo
        public long? ItemID { get; set; }
        public string ArticleNo { set; get; }
        public long? ItemColorID { get; set; }
        public string ColorName { set; get; }

        public decimal WarpStop { set; get; }
        public decimal WarpCMPX { set; get; }
        public decimal WeftStop { set; get; }
        public decimal WeftCMPX { set; get; }
        public decimal OtherStop { set; get; }
        public decimal StartATT { set; get; }
        public decimal RPM { set; get; }
        public decimal Efficiency { set; get; }
        public decimal RunTime { set; get; }
        public decimal Prodn { set; get; }
        public Nullable<int> TotalStop { get; set; }
        public string Remarks { set; get; }

        public Nullable<int> LoomStopDetailID { get; set; }
        public Nullable<int> BWSID { get; set; }
        public string BWSName { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        //public string StartTime { get; set; }
        public int StopInMin { get; set; }
        public string Description { get; set; }

        public bool IsReleased { set; get; }
        public bool IsDeleted { set; get; }

        public Nullable<int> SlNo { get; set; }//Breakage
        public Nullable<int> SNo { get; set; }//MachineStop        
        public bool IsNextDate { get; set; }
        public string ModelState { get; set; }
    }
}
