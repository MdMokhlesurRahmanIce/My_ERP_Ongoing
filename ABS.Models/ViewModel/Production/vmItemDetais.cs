using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models.ViewModel.SystemCommon;

namespace ABS.Models.ViewModel.Production
{
   public class vmItemDetais:vmCmnParameters
    {
      public int ItemId { get; set; }
      public string ItemName { get; set; }
    }
}
