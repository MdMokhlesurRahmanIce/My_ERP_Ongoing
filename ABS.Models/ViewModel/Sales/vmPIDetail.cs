using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmPIDetail
    {
        public long PIDetailID { get; set; }
        public long HDODetailID { get; set; }
        public string PINO { get; set; }
        public string BuyerStyle { get; set; }
        public string CuttableWidth { get; set; }
        public short? ItemConstructionTypeID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public long PIID { get; set; }
        public long ItemID { get; set; }
        public string Description { get; set; }

        public string ArticleNo { get; set; }
        public string ItemName { get; set; }
        public string Construction { get; set; }
        public decimal ExRate { get; set; }
        public int CompanyID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string WarpYarnCount { get; set; }
        public string WeftYarnCount { get; set; }
        public decimal? EPI { get; set; }

        public decimal? PPI { get; set; }
        public int? NoOfPI { get; set; }

    }
}
