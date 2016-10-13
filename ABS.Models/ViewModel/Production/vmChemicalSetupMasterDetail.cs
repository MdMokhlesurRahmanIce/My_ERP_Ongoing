using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmChemicalSetupMasterDetail
    {
        public int? DepartmentID { get; set; }

        public long? ChemicalSetupID { get; set; }
        public long? ItemID { get; set; }
        public string ArticleNo { get; set; }
        public long? SetID { get; set; }
        public long? WeavingMRRID { get; set; }
        public string WeavingMRRNo { get; set; }
        public string SetNo { get; set; }
        public decimal? Qty { get; set; }
        public long? UnitID { get; set; }
        public string UOMName { get; set; }
        public string OrganogramName { get; set; }

        public long? SupplierID { get; set; }
        public long? BatchID { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CurrentStock { get; set; }

        public long? ChemicalSetupDetailID { get; set; }
        public long? ChemicalID { get; set; }
        public string ItemName { get; set; }

        public long? ChemicalConsumptionID { get; set; }
        public long? ChemicalConsumptionDetailID { get; set; }

        public int? FinChemicalStupID { get; set; }
        public long? FinishingConsumptionID { get; set; }
        public long? FinishingConsumptionDetailID { get; set; }
        public string FinishingConsumptionNo { get; set; }
        public long? FinishingTypeID { get; set; }
        public string FInishTypeName { get; set; }
        public decimal? Volume { get; set; }
        public DateTime? ConsumptionDate { get; set; }
        public string Remarks { get; set; }
        public string RequiredQty { get; set; }
        public decimal? AccQty { get; set; }

        public decimal? MinQty { get; set; }
        public decimal? MaxQty { get; set; }

        public List<vmCmnBatch> Batch { get; set; }
        public List<vmBallInfo> Supplier { get; set; }

        public bool IsDeleted { get; set; }
    }
}