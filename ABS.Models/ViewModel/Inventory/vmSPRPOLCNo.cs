using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
   public class vmSPRPOLCNo
    {
         public long SPRPOLCID { get; set; }
         public string SPRPOLCNo { get; set; }
         public bool IsDeleted { get; set; }
         public DateTime? SPRPOLCDate { get; set; }
    }
}
