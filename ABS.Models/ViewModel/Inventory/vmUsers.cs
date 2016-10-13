using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
  public  class vmUsers
    {
        
        public Nullable<int> UserID { get; set; }      
        public string UserFullName { get; set; }

        public List<vmLot> LotNos { get; set; }
        public List<vmBatch> BatchNos { get; set; }
        public List<vmUser> Suppliers { get; set; }
    }
}
