using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public class vmGroup
    {
        public int? ID { get; set; }
       public string Name { set; get; }
       public List<vmGroup> Children { set; get; }
       
       public int? ParentID { get; set; }

       public int? AcDetailID { get; set; } 

       public bool collapsed { get; set; }
    }
}
