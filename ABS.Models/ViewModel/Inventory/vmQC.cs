using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
    public class vmQC 
    {
        public Nullable<long> MrrQcDetailID { get; set; }
        public Nullable<long> MrrQcID { get; set; }
        public Nullable<long> GrrDetailID { get; set; }
        public Nullable<long> GrrID { get; set; }
        public string MrrNo { get; set; }
        public Nullable<System.DateTime> MrrDate { get; set; } 
        public Nullable<long> TransactionTypeID { get; set; }  
        public Nullable<long> IssueID { get; set; } 
        public string IssueNo { get; set; }

        public string OrganogramName { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }

        public Nullable<System.DateTime> GrrDate { get; set; } 
        
        public Nullable<long> ItemID { get; set; }
        public Nullable<decimal> RemainingGrrQty { get; set; } 
        public Nullable<decimal> GrrQty { get; set; } 
        public Nullable<decimal> GrrAdditionalQty { get; set; }

        public Nullable<decimal> PassQty { get; set; }
        public Nullable<decimal> RejectQty { get; set; }

        public Nullable<decimal> AdditionalGrrQty { get; set; }
        public Nullable<decimal> AdditionalPassQty { get; set; }
        public Nullable<decimal> AdditionalRejectQty { get; set; }  

        public string Remarks { get; set; }
        public Nullable<int> UnitID { get; set; }
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

       // public long UOMID { get; set; }
        public string UOMName { get; set; }

        public Nullable<long> LotID { get; set; }

        public string LotNo { get; set; }
        public Nullable<long> BatchID { get; set; }
        public string BatchNo { get; set; }

        public bool? IsMrrCompleted { get; set; }

        public bool? IsQCCompleted { get; set; } 

        // qc Master //
        public string MrrQcNo { get; set; }
        public Nullable<System.DateTime> MrrQcDate { get; set; }
        public Nullable<long> SPLID { get; set; }
        public string SPLNoName { get; set; }
        public Nullable<long> CIRID { get; set; }
        public string CIRNoName { get; set; }
        public Nullable<int> SPLTypeID { get; set; } 
        public string SPLTypeName { get; set; }
        public Nullable<int> CIRTypeID { get; set; }
        public string CIRTypeName { get; set; }

        public string GrrNo { get; set; }
        public Nullable<long> SupplierID { get; set; }

        public string SupplierName { get; set; } 
        public Nullable<int> StatusID { get; set; }
        public string StatusBy { get; set; }
        public Nullable<int> FromDepartmentID { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> CompanyID { get; set; }

        public string FromCompanyName { get; set; }
        public Nullable<int> FromCompanyID { get; set; }  


        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; } 

        
        public Nullable<decimal> QtyWithoutQc { get; set; }
        public Nullable<decimal> QtyWithQc { get; set; }

        public Nullable<System.DateTime> SPLDate { get; set; }

        public Nullable<System.DateTime> SPRDate { get; set; }

        public Nullable<System.DateTime> PODate { get; set; }

        public Nullable<System.DateTime> PIDate { get; set; }  

        public Nullable<System.DateTime> CIRDate { get; set; }

        public Nullable<decimal> QcRemainingQty { get; set; }

        public Nullable<decimal> QcRemainingAdditionalQty { get; set; } 

        public Nullable<decimal> QcValidQty { get; set; }

      //  public Nullable<decimal> QcRemainingAdditionalQty { get; set; }

        public Nullable<decimal> QcValidAdditionalQty { get; set; } 
                                         

        public Nullable<decimal> MrrValidQty { get; set; }

        public Nullable<decimal> MrrValidAdditonalQty { get; set; }

        public Nullable<long> MrrDetailID { get; set; } 
        public Nullable<long> MrrID { get; set; }

        public Nullable<long> CurrencyID { get; set; }

        public string CurrencyName { get; set; }  

        public Nullable<decimal> UnitPrice { get; set; }
        
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> QCQty { get; set; } 
        public Nullable<decimal> QCAdditionalQty { get; set; }  

        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> AdditionalQty { get; set; }  

        public Nullable<decimal> CurrentStock { get; set; }

        public Nullable<decimal> AvailableQty { get; set; }

        public Nullable<decimal> AvailableAdditonalQty { get; set; }  

        public Nullable<long> POID {get; set;}
        public Nullable<long> PIID {get; set;}
        public Nullable<long> RequisitionID {get; set;}

        public string SprNo { get; set; }

        public int? UserID { get; set; }

        public string UserFullName { get; set; }
        public string PONO { get; set; } 

        public string PONo {get; set;}
        public string PINo {get;set;}
        public string RefCHNo { get; set; } 
        public string LCNO { get; set; }
        public string QCCertificateNo { get; set; }
        public string Description { get; set; }
        public string DocURL { get; set; }

        public string ManualMRRNo { get; set; }
             

        public string RequisitionNo { get; set; }
         
    }
}
