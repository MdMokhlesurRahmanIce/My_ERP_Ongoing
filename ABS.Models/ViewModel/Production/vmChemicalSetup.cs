using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
   public class vmChemicalSetup
    {
        [Display(Name = "Chemical Setup I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ChemicalSetupID { get; set; }

        [Display(Name = "Color I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int ColorID { get; set; }
        public String ColorName { get; set; }
        [Display(Name = "Qty")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal Qty { get; set; }

        [Display(Name = "Department I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DepartmentID { get; set; }

        [Display(Name = "User I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int UserID { get; set; }

        [Display(Name = "Unit I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int UnitID { get; set; }
        public String UnitName { get; set; }

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
