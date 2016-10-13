using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public  class ConsumptionMaster
    {
        public int ItemID { get; set; }
        public string Description { set; get; }
        public string Note { set; get; }
        public int CompanyID { set; get; }     
        public int CreateBy { set; get; }
    }
}
