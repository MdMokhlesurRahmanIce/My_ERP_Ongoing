using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmAppStatus
    {
        public int StatusID{ get;  set;}
        public string StatusName { get; set; }
        public int Sequence { get; set; }
        public string SequenceName { get; set; }
        public int EmployeeID { get; set; }
        public int TransactionID { get; set; }
        public bool IsApprove { get; set; }
    }
}
