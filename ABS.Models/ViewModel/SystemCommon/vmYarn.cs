using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public class vmYarn
    {
       public Int64 ItemID { get; set; }
       public string ItemName { get; set; }
       public string CompnayID { set; get; }
       public string LoginID { set; get; }
       public int Yarn { set; get; }
       public string YarnName { set; get; }
       public string LotName { set; get; }
       public string YarnRatio { set; get; }      
       public int? LotID { set; get; }
       public decimal Ratio { set; get; }
       public string YarnType { get; set; }
    }
}
