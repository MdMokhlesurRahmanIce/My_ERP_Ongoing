using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmItemTypes
    {
        public int ItemTypeID { get; set; }    
        public string ItemTypeName { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> CreateBy { get; set; }
    }
}
