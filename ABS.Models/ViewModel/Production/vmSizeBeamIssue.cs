using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmSizeBeamIssue
    {
        public Int64? SetID { get; set; }
        public string SetNo { get; set; }
        public decimal? TEnds { set; get; }
        public Int64? SetLength { get; set; }
        public string WeftLot { get; set; }
        public string WeftCount { get; set; }
        public string WeftRatio { get; set; }
        public string WarpLot { get; set; }
        public string WarpCount { get; set; }
        public string WarpRatio { get; set; }
        public string SupplierFullName { get; set; }
        public string BuyerFullName { get; set; }
        public string PINO { set; get; }
        public string ColorName { set; get; }
        public Int64? SIzeMRRID { get; set; }
        public string SizeMRRNo { set; get; }
        public string FlangeNo { set; get; }
        public decimal? LengthYds { set; get; }
        public DateTime? BSDate { set; get; }
        public DateTime? BMDate { set; get; }
        public DateTime? BFDate { set; get; }
        public int? LoomID { set; get; }
        public DateTime? DFDate { set; get; }
        public decimal? Totalfabric { set; get; }
        public string Remarks { set; get; }
        public int TransactionTypeID { set; get; }
        public Int64? ItemID { set; get; }
        public long? SizeMRRID { get; set; }

        public int? SizeDepartmentID { get; set; }

        public bool? IsIssuedSize { get; set; }

        public DateTime? SizeIssueDate { get; set; }

        public int? SizeIssueBy { get; set; }

        public string SizeIssueRemarks { get; set; }

        public int CompanyID { get; set; }

        public int? CreateBy { get; set; }

        public bool IsDeleted { get; set; }
        public int? OutputID { set; get; } // Flange No.

       //-----------------------Master Size Beam Issue---------------------------------
        public Int64 BeamIssueID { get; set; }
        public string ItemName { get; set; }      
        public string IsIssuedSizeStatus { get; set; }
        public string ConSizeIssueDate { set; get; }
        public string ConWeavingReceiveDate { get; set; }
        public string WeavingReceivedRemarks { get; set; }
        public decimal? CSetlength { set; get; }
        public string MachineName { set; get; }
        public string ArticleNo { set; get; }
        public Int64 BeamIssueDetailID { set; get; }
        public string Shade { get; set; }
        public string GPL { get; set; }
        public int? WeavingDepartmentID { set; get; }
        public DateTime? WeavingReceiveDate { set; get; }
        public int? WeavingReceiveBy { set; get; }
        public bool? IsReceivedWeaving { set; get; }
        public string IsReceivedWeavingStatus { set; get; }
    }
}
