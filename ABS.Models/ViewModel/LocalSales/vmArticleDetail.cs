using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.LocalSales
{
    public class vmArticleDetail
    {
        public Int64? ItemID { get; set; }
        public string ArticleNo { set; get; }
        public string ItemName { set; get; }
        public string StoreCode { set; get; }
        public decimal? CuttableWidth { set; get; }
        public string Description { set; get; }
        public string Unit { set; get; }
        public int? GradeID { set; get; }
        public decimal? CurrentStock { set; get; }
        public decimal? UnitPrice { set; get; }
        public decimal? Qty { set; get; }
        public decimal? Amount { set; get; }
        public int? LotID { set; get; }
        public int? BatchID { set; get; }
        public int? SupplierID { set; get; }
        public string Status { set; get; }

    }
}
