using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
  public  class vmPrdDyingOperationSetup
    {
        [Display(Name = "Operation Setup I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 OperationSetupID { get; set; }

        [Display(Name = "Item I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ItemID { get; set; }
        public String ArticleNo { get; set; }

        [Display(Name = "Chemical Item I D")]
        public Int64? ChemicalItemID { get; set; }
        public String ChemicalArticleNo { get; set; }

        [Display(Name = "Dying Process I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DyingProcessID { get; set; }
        public String ProcessName { get; set; }

        [Display(Name = "Operation I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OperationID { get; set; }

        public String OperationName { get; set; }

        [Display(Name = "Min Qty")]
        public decimal? MinQty { get; set; }

        [Display(Name = "Max Qty")]
        public decimal? MaxQty { get; set; }

        [Display(Name = "Unit I D")]
        public int? UnitID { get; set; }
        public String UOMName { get; set; }

        [Display(Name = "Company I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CompanyID { get; set; }

        [Display(Name = "Create By")]
        public int? CreateBy { get; set; }

        [Display(Name = "Create On")]
        public DateTime? CreateOn { get; set; }

        [Display(Name = "Create Pc")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CreatePc { get; set; }

        [Display(Name = "Update By")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Update On")]
        public DateTime? UpdateOn { get; set; }

        [Display(Name = "Update Pc")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string UpdatePc { get; set; }

        [Display(Name = "Is Deleted")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Delete By")]
        public int? DeleteBy { get; set; }

        [Display(Name = "Delete On")]
        public DateTime? DeleteOn { get; set; }

        [Display(Name = "Delete Pc")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string DeletePc { get; set; }
    }
}
