using ABS.Models.ViewModel.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.LocalSales
{
    public class vmSLDemageSale
    {
        public Int64? ItemID { set; get; }
        public string StoreCode { set; get; }
        public string ItemName { set; get; }
        public string Description { set; get; }
        public List<vmLot> Lots { get; set; }
        public List<vmSLBatch> Batchs { get; set; }
        public List<vmSLSupplier> Suppliers { get; set; }
        public string Unit { set; get; }
        public decimal? UnitID { set; get; }
        public decimal? CurrentStock { set; get; }
        public decimal? UnitPrice { set; get; }
        public decimal? Qty { set; get; }
        public string Status { set; get; }       

        public int? SupplierID { get; set; }

        public long? BatchID { get; set; }

        public long? GradeID { get; set; }

        public long? LotID { get; set; }

        public decimal? Amount { get; set; }

        public decimal? CuttableWidth { get; set; }

        public long SIDetailID { get; set; }
    }
}
