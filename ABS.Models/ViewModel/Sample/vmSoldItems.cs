using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sample
{
    public class vmSoldItems
    {
        public string SaleNo { get; set; }
        public string OutletName { get; set; }
        public string TypeName { get; set; }
        public string ProductName { get; set; }
        public decimal netPrice { get; set; }
        public decimal grossPrice { get; set; }
        public DateTime SaleDate { get; set; }


        //public string ProductName { get; set; }
        //public decimal Price { get; set; }
        //public string OutletName { get; set; }
        //public string SaleNo { get; set; }
    }
}
