using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmItem
    {
        public long ItemID { get; set; }
        public string ArticleNo { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public Nullable<int> ItemGroupID { get; set; }
        public string ItemName { get; set; }
        public long BookingID { get; set; }
        public long BookingDetailID { get; set; }
        public decimal Amount { get; set; }
        public decimal ExRate { get; set; }
        public string Weave { get; set; }
        public string YarnCount { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> ItemSpecificationID { get; set; }
        public Nullable<int> ItemStyleID { get; set; }
        public Nullable<int> ItemSeasonID { get; set; }
        public Nullable<int> ItemYearID { get; set; }
        public Nullable<int> UOMID { get; set; }
        public Nullable<int> ItemBrandID { get; set; }
        public Nullable<int> ItemModelID { get; set; }
        public Nullable<int> OriginCountryID { get; set; }
        public Nullable<int> ItemColorID { get; set; }
        public Nullable<int> ItemSizeID { get; set; }
        public Nullable<int> ItemConditionID { get; set; }
        public Nullable<int> StandardID { get; set; }
        public Nullable<int> WashingTypeID { get; set; }
        public Nullable<int> SustainableID { get; set; }
        public Nullable<int> CompositionID { get; set; }
        public Nullable<int> DyingCompanyID { get; set; }
        public Nullable<int> DyingMethodID { get; set; }
        public Nullable<int> DyingUOMID { get; set; }
        public Nullable<int> WasgingCompanyID { get; set; }
        public Nullable<int> WashingMethodID { get; set; }
        public Nullable<int> WashingUOMID { get; set; }
        public Nullable<decimal> CuttableWidth { get; set; }
        public Nullable<decimal> WeightPerUnit { get; set; }
        public Nullable<decimal> FinishingWeight { get; set; }
        public Nullable<decimal> FinishingWidth { get; set; }

        public Nullable<int> WeightUOMID { get; set; }
        public Nullable<int> FinishingTypeID { get; set; }
        public string UniqueCode { get; set; }
        public string HSCODE { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsSample { get; set; }
        public Nullable<bool> IsPart { get; set; }
        public Nullable<int> ItemGradeID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public int CompanyID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatePc { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool IsDeleted { get; set; }
        public int IsDevelopmentComplete { get; set; }
        
        public Nullable<int> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }
        public string Construction { get; set; }
        public Nullable<decimal> Width { get; set; }
        public string ColorName { get; set; }
        public string ItemGroupName { get; set; }
    }
}
