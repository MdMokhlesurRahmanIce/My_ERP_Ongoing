using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
  public  class vmPrdSetSetup
    {
        [Required(ErrorMessage = "Required")]
        public Int64 SetID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string SetNo { get; set; }

        [Required(ErrorMessage = "Required")]
        public Int64 SetMasterID { get; set; }

        [Required(ErrorMessage = "Required")]
        public Int64 ItemID { get; set; }
        public String ItemName { get; set; }

        [Required(ErrorMessage = "Required")]
        public Int64 YarnID { get; set; }

        public string YarnCount { get; set; }

        public string YarnRatioLot { get; set; }

        public string YarnRatio { get; set; }

        public int? SetTrackingNo { get; set; }

        public int? NoOfBall { get; set; }

        public Int64? SetLength { get; set; }

        public decimal? MachineSpeed { get; set; }

        public decimal? TotalEnds { get; set; }

        public String Weave { get; set; }

        public decimal? EndsPerRope { get; set; }

        public decimal? EndsPerCreel { get; set; }

        public decimal? LeaseReapet { get; set; }

        public Int64? PIID { get; set; }
        public String PINo { get; set; }
        public Int64? SupplierID { get; set; }
        public String SupplierName { get; set; }
        public Int64? BuyerID { get; set; }
        public String BuyerName { get; set; }
        
        public int? ColorID { get; set; }

        public Int64? WeftYarnID { get; set; }

        public Int64? WarpYarnID { get; set; }

        public string Description { get; set; }

        public DateTime? SetDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public int CompanyID { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string CreatePc { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public string UpdatePc { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool IsDeleted { get; set; }

        public int? DeleteBy { get; set; }

        public DateTime? DeleteOn { get; set; }

        public string DeletePc { get; set; }

    }
}
