using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public class vmModulePermission
    {


        [Display(Name = "Module Permission I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int ModulePermissionID { get; set; }

        [Display(Name = "Custom Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string CustomCode { get; set; }

        [Display(Name = "Module I D")]
        public int? ModuleID { get; set; }

        [Display(Name = "Company I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CompanyID { get; set; }

        [Display(Name = "Status I D")]
        public int? StatusID { get; set; }

        [Display(Name = "Menu List I D")]
        public int? MenuListID { get; set; }

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

        public string MenuName { get; set; }

        public string CompanyName { get; set; }

        public string ModuleName { get; set; }

        public string StatusName { get; set; }
        

    }
}
