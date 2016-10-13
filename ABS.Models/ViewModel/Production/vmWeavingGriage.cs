using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmWeavingGriage
    {
        public Int64? MachineBookID { get; set; }
        public Int64? MachineConfigID { get; set; }
        public Int64? BeamIssueDetailID { set; get; }
        public Int64? ItemID { get; set; }
        public Int64? SetID { get; set; }
        public Int64? SizeMRRID { get; set; }
        public string ArticleNo { get; set; }
        public string SetNO { set; get; }
        public string Unit { get; set; }
        public int? DoffingNo { get; set; }
        public int? UnitID { set; get; }
        public decimal? Griege { set; get; }
        public string WeavingMRRDate { get; set; }
        public int? ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string OperatorName { set; get; }
        public int? OperatorID { set; get; }
        public string MachineConfigNo { set; get; }
        public Int64? WeavingMRRID { set; get; }
        public string WeavingMRRNo { set; get; }
        public string Remarks { get; set; }
        public bool IsIssued { get; set; }

    }
}
