using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Accounting
{
    public class vmBOM
    {
        public long? BOMID { get; set; }

        public string BOMNO { get; set; }

        public string CostingNo { get; set; }

        public string LotNo { get; set; }

        public string Yarn { get; set; }

        public string BomArticleNo { get; set; }
        public string Description { get; set; }
        public DateTime? BOMDate { get; set; }

        public DateTime? CostingDate { get; set; }

        public long? BOMDyingID { get; set; }
        public long? BOMSizeID { get; set; }
        public long? DyingChemicalID { get; set; }
        public long? SizeChemicalID { get; set; }

        public long? CostingYarnID { get; set; }

        public int? ConsumptionTypeID { get; set; } 
        public long? YarnID { get; set; }

        public long? LotID { get; set; }


        public long? CostingDetailID { get; set; }

        public long? CostingID { get; set; }

        public long? CostingDyingID { get; set; }

        public long? CostingSizeID { get; set; }

        public long? CostingFinishID { get; set; }

        public long? FinishingProcessID { get; set; }

        public int? CurrencyID { get; set; }
        public string CurrencyName { get; set; }

        public long? ItemID { get; set; }
        public string ArticleNo { get; set; }
        public string YarnType {get; set;}

        public decimal? CuttableWidth { get; set; } 
        public decimal? FinishingWidth { get; set; }

        public decimal? LastPurchasePrice { get; set; }

        public decimal? Amount { get; set; }
        public decimal? YarnCost { get; set; }
        public decimal? SizeCost { get; set; }
        public decimal? DyingCost { get; set; }
        public decimal? FinishingCost { get; set; }
        public decimal? OverHeadCost { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? yarnTotal { get; set; }

        public decimal? sizTotal { get; set; }
        public decimal? dyngTotal { get; set; }
        public decimal? finishTotal { get; set; }
        public decimal? ttlTotal { get; set; } 



        public string ItemName { get; set; }
        public string Construction { get; set; } 
        public decimal? WeightPerUnit { get; set; }

        public decimal? Qty { get; set; }  
        public string ColorName { get; set; } 
        public string WarpYarn { get; set; }
        public string WeftYarn { get; set; }

        public int? UnitID { get; set; } 
        public string UOMName { get; set; }

        public string ProcessFullName { get; set; }

        public string ProcessCode { get; set; }




        public string Remarks { get; set; }
        public int? CompanyID { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreatePc { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool? IsDeleted { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string DeletePc { get; set; }
    }
}
