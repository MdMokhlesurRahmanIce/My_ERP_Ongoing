using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.LocalSales
{
  public  class vmLSCurrentStock
    {
      public Int64? ItemID { set; get; }
      public decimal? UnitPrice { set; get; }
      public decimal? CurrentStock { set; get; }
    }
}
