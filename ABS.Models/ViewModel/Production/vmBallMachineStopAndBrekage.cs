using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmBallMachineStopAndBrekage
    {
        public Nullable<int> BallMachineStopID { get; set; }
        public Nullable<long> BallMachineStopMasterID { get; set; }
        public Nullable<int> BallBreakageID { get; set; }
        public Nullable<long> BallBreakageMasterID { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public string Description { get; set; }
        public int ShiftID { get; set; }
        public DateTime StopDate { get; set; }
        public int BWSID { get; set; }
        public long MachineID { get; set; }
        public int? NoOfBreakage { get; set; }
        public string BreakageType { get; set; }
        public Nullable<int> SlNo { get; set; }//Breakage
        public Nullable<int> SNo { get; set; }//MachineStop
        public int StopInMin { get; set; }
        public Nullable<int> BalMachineStopID { get; set; }

        public Nullable<long> BalMachineStopMasterID { get; set; }
        public long BallBreackageID { get; set; }
        

        public long BalMRRDetailID { get; set; }
        public bool IsNextDate { get; set; }
        public string BWSName { get; set; }
        //public DateTime BreakageDate { get; set; }
        public string ModelState { get; set; }

    }
}
