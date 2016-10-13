using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmChallan
    {
        public Nullable<long> CHDetailID { get; set; }
        public Nullable<long> CHID { get; set; }
        public Nullable<long> RequisitionDetailID { get; set; }
        public Nullable<long> RequisitionID { get; set; }
        public Nullable<long> ItemID { get; set; }
       
        public Nullable<long> IssueID { get; set; }
        public string IssueNo { get; set; } 
         public Nullable<long> IssueDetailID { get; set; }

        public Nullable<decimal> RemainingGrrQty { get; set; }
        public Nullable<decimal> REQQty { get; set; }

        public Nullable<decimal> AvailableGrrQty { get; set; }
        public Nullable<bool> IsUrgent { get; set; }
        public Nullable<int> RequisitionTypeID { get; set; }
        public Nullable<bool> IsComplete { get; set; }

        public string Remarks { get; set; }
        public string PINo { get; set; }
        public string LCorVoucherorLcafNo { get; set; }
        public string PONo { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<long> PIID { get; set; }
        public Nullable<long> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatePc { get; set; }
        public Nullable<long> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }

        public string ItemName { get; set; }

        public string ItemCode { get; set; } 
        public string UOMName { get; set; }

        public Nullable<long> LotID { get; set; }

        public string LotNo { get; set; }
        public Nullable<long> BatchID { get; set; }
        public string BatchNo { get; set; }
        public string SizeName { get; set; }

        public string SPLNoName { get; set; }
        public Nullable<long> CIRID { get; set; }
        public string CIRNoName { get; set; }
        public Nullable<int> SPLTypeID { get; set; }
        public string SPLTypeName { get; set; }
        public Nullable<int> CIRTypeID { get; set; }
        public string CIRTypeName { get; set; }

        public Nullable<long> SupplierID { get; set; }

        public string SupplierName { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string StatusBy { get; set; }

        public string CompanyName { get; set; }
        public Nullable<int> CompanyID { get; set; }


        public string FromCompanyName { get; set; }
        public Nullable<int> FromCompanyID { get; set; } 


        public int? RecordTotal { get; set; }

        public Nullable<decimal> UnitPrice { get; set; }

        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> CurrentStock { get; set; }
        public Nullable<decimal> PackingQty { get; set; }

        public Nullable<decimal> GrossWeight { get; set; }

        public Nullable<decimal> NetWeight { get; set; }
        public Nullable<decimal> ExistQty { get; set; }
        public string DischargeLocation { get; set; }
        public string Weave { get; set; }

        public string CHNo { get; set; }
        public string RefCHNo { get; set; }
        public string LoadingLocation { get; set; }
        public int? PackingUnitID { get; set; }
        public string PackingUnit { get; set; }
        public string WeightUnit { get; set; }
        public int? WeightUnitID { get; set; }
        public int? CHTypeID { get; set; }
        public int? PartyID { get; set; }
        public Nullable<System.DateTime> CHDate { get; set; }
        public Nullable<System.DateTime> RefCHDate { get; set; }
        public int? CurrencyID { get; set; }
        public int? TransactionTypeID { get; set; }

        public int? LoadingPortID { get; set; }
        public int? DischargePortID { get; set; }
        public int? DepartmentID { get; set; }

        public string ArticleNo { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public Nullable<int> ItemGroupID { get; set; }
        public Nullable<int> UOMID { get; set; }
        public Nullable<int> ItemBrandID { get; set; }
        public Nullable<int> ItemModelID { get; set; }

        public string CountryName { get; set; }

        public Nullable<int> ItemColorID { get; set; }
        public Nullable<int> ItemSizeID { get; set; }


        public Nullable<decimal> CuttableWidth { get; set; }
        public Nullable<decimal> WeightPerUnit { get; set; }
        public Nullable<int> WeightUOMID { get; set; }
        public Nullable<int> FinishingTypeID { get; set; }
        public string UniqueCode { get; set; }
        public string HSCODE { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }

        public Nullable<int> ItemGradeID { get; set; }

        public Nullable<int> Id { get; set; }

        public Nullable<int> ComboID { get; set; }

        public string Construction { get; set; }

        public Nullable<int> UserID { get; set; }
        public Nullable<decimal> Width { get; set; }
        public string ColorName { get; set; }

        public string ItemGroupName { get; set; }

        public string RequisitionNo { get; set; }

        public string CurrencyName { get; set; }

        public string ComboName { get; set; }
        public string UserFullName { get; set; }
        public Nullable<decimal> AditionalQty { get; set; }

        public Nullable<decimal> AdditionalQty { get; set; }
        public Nullable<decimal> DisAmount { get; set; }
        public Nullable<bool> IsPercent { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }



        //---------------- New Add -----------------------------

        public string DepartmentName { get; set; }
        public Nullable<long> GrrID { get; set; }
        public Nullable<long> GrrDetailID { get; set; }
        public string GrrNo { get; set; }

        public string ManualGrrNo { get; set; }
        public Nullable<System.DateTime> GrrDate { get; set; }

        public Nullable<long> MrrQcID { get; set; }
        public string MrrQcNo { get; set; }

        public Nullable<long> MrrID { get; set; }

        public string MrrNo { get; set; }

        public string ManualQCNoRpt { get; set; }
        public string ManualMRRNoRpt { get; set; }
        public string ManualGRRNoRpt { get; set; }
        public string ManualPoNoRpt { get; set; }
        public string ManualPiNoRpt { get; set; }

        //  PO  

        public Nullable<long> QuotationID { get; set; }

        public string QuotationNo { get; set; } 
        public Nullable<long> CSID { get; set; }
        public string CSNo { get; set; }
        public Nullable<long> CSDetailID { get; set; }
        public Nullable<long> POID { get; set; }
        public Nullable<long> PODetailID { get; set; }
        public Nullable<decimal> FreightCharge { get; set; }
        public Nullable<decimal> FOBValue { get; set; }
        public Nullable<int> OriginCountryID { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<int> MoneyTransactionTypeID { get; set; }
        public Nullable<int> OrderTypeID { get; set; }
        public Nullable<long> FRID { get; set; }
        public Nullable<System.DateTime> LCorVoucherorLcafDate { get; set; }
        public Nullable<int> BankID { get; set; }
        public Nullable<int> BankBranchID { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAccountNo { get; set; }
        public Nullable<decimal> AccAmount { get; set; }
        public Nullable<System.DateTime> ShipmentDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string MoneyTransactionTypeName { get; set; }
        public string OrderTypeName { get; set; }
    }
}
