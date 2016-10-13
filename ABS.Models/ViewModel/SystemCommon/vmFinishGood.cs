using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmFinishGood
    {

        public string ArticleNo { get; set; }
        public decimal? FinishingWeigth { set; get; } // Fini Wt
        public string WarpYarnRatio { get; set; }
        public string WarpYarnRatioLot { set; get; }
        public string Constraction { set; get; }
        public string WeftpYarnRatio { set; get; }
        public string WeftYarnRatioLot { set; get; }
        public string GerigeEPIxPPI { get; set; }
        public string Color { get; set; }
        public string Weave { get; set; }
        public string Width { get; set; }
        public decimal? CuttableWidth { get; set; }
        public string LengthShrinkage { set; get; }
        public string WidthShrinkage { set; get; }
        public decimal? Cotton { set; get; }
        public decimal? Spandex { set; get; }
        public decimal? Polyester { set; get; }
        public decimal? Lycra { set; get; }
        public decimal? C4100 { set; get; }
        public decimal? T400 { set; get; }
        public decimal? Viscos { set; get; }
        public decimal? Modal { set; get; }
        public decimal? Tencel { set; get; }
        public string OtherComp { get; set; }


        public string SetNo { set; get; }
        public string Remark { get; set; }
        public string MachineName { set; get; }
        public DateTime? WeavingDate { set; get; }
        public decimal? Length { set; get; }
        public decimal? GGSM { set; get; }
        public decimal? GWidth { set; get; }
        public string WarpYarnCount { get; set; }
        public string WeftYarnCount { set; get; }
        public string ItemGroupName { get; set; }
        public string ItemTypeName { get; set; }
        public string UOMName { get; set; }
        public string SizeName { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string UniqueCode { set; get; }
        public string Note { set; get; }
        public string WeftsupplierFullName { get; set; }
        public string FlangeNo { get; set; }
        public string BuyerFullName { get; set; }
        public string BuyerRefFullName { get; set; }
        public string FinishingProcessName { set; get; }
        public decimal? GPPI { get; set; }
        public string HSCODE { get; set; }
        public decimal? WeightPerUnit { get; set; }
        public decimal? GEPI { get; set; }
        public decimal? TotalEnds { get; set; }
        public string Need { get; set; }
        public decimal? WeavingLength { set; get; }
        public decimal? MinLShrinkage { set; get; }
        public decimal? MaxLshrinkage { set; get; }
        public decimal? MinWshrinkage { set; get; }
        public decimal? MaxWShrinkage { set; get; }
        public decimal? FinishingWidth { set; get; }
        public decimal? WashingWeigth { set; get; }
        public decimal? WashingWidth { set; get; }
        public decimal? MinLShrinkageW { set; get; }
        public decimal? MaxLshrinkageW { set; get; }
        public decimal? MinWshrinkageW { set; get; }
        public decimal? MaxWShrinkageW { set; get; }
        public decimal? Skew { set; get; }
        public decimal? SkewW { set; get; }
        public decimal? WEPI { set; get; }
        public decimal? WPPI { set; get; }
        public decimal? Count { get; set; }



        //  ---------------ID------------------------
        public int? ItemGroupID { get; set; }
        public long? ItemID { get; set; }
        public long? WarpYarnID { set; get; }
        public long? WeftYarnID { get; set; }
        public int? WeftSupplierID { set; get; }
        public int? ItemColorID { set; get; }
        public int? BuyerID { get; set; }
        public int? BuyerRefID { get; set; }
        public int? WashingTypeID { get; set; }
        public int? WeightUOMID { get; set; }
        public int? FinishingTypeID { get; set; }
        public int? FinishingWeightID { get; set; }
        public int? WeavingMachineID { get; set; }
        public int? ParentID { get; set; }
        public int? ItemSpecificationID { get; set; }
        public int? ItemStyleID { get; set; }
        public int? ItemSeasonID { get; set; }
        public int? ItemYearID { get; set; }
        public int? UOMID { get; set; }
        public int? ItemBrandID { get; set; }
        public int? ItemModelID { get; set; }
        public int? OriginCountryID { get; set; }
        public int? ItemSizeID { get; set; }
        public int? ItemConditionID { get; set; }
        public int? StandardID { get; set; }
        public int? ItemGradeID { get; set; }
        public int? DepartmentID { get; set; }
        public int? StatusID { get; set; }
        public int? CompanyID { get; set; }
        public int ItemTypeID { get; set; }



        // ---------------ENd------------------
        public string PDLRef { get; set; }

        public decimal? FiniWt { get; set; }


        public string WarpRatio { get; set; }

        public string WarpSLlot { get; set; }

        public string WeftYCount { get; set; }

        public string WeftRatio { get; set; }

        public string WeftSLlot { get; set; }

        public string WeftSuppliter { get; set; }
        public string Buyer { get; set; }

        public string Buyerref { get; set; }

        public string ItemGroup { get; set; }

        public decimal? PPI { get; set; }

        public string FinishProcess { get; set; }

        public long? FinishProcessId { get; set; }
        public int? WeftSupplierId { get; set; }

        public int? ColorID { get; set; }

        public int? BuyerrefId { get; set; }

        //public int? BuyerID { get; set; }

        public decimal? EPI { get; set; }

        public int? FiniWtID { get; set; }

        public int IsDevelopmentComplete { get; set; }

        public int? CoatingID { get; set; }
        public int? SPCoatingID { get; set; }
        public int? OverDyedID { get; set; }
        public int? WeftColorID { set; get; }

        public string CoatingName { get; set; }
        public string SpecialCoatingName { get; set; }
        public string OverDyedName { set; get; }
        public string WeftColorName { set; get; }
    }
}
