using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public partial class vmCmnMenuPermission
    {
        [Display(Name = "Menu I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MenuID { get; set; }

        [Display(Name = "Menu Name")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string MenuName { get; set; }

        [Display(Name = "Sequence")]
        public int? Sequence { get; set; }

        [Display(Name = "Module I D")]
        public int? ModuleID { get; set; }

        [Display(Name = "Module Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string ModuleName { get; set; }

        [Display(Name = "Menu Permission I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MenuPermissionID { get; set; }

        [Display(Name = "User Group I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int UserGroupID { get; set; }

        [Display(Name = "Group Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string GroupName { get; set; }

        [Display(Name = "User I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int UserID { get; set; }

        [Display(Name = "User Full Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string UserFullName { get; set; }

        [Display(Name = "Organogram I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OrganogramID { get; set; }

        [Display(Name = "Organogram Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string OrganogramName { get; set; }

        [Display(Name = "Enable View")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableView { get; set; }

        [Display(Name = "Enable Insert")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableInsert { get; set; }

        [Display(Name = "Enable Update")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableUpdate { get; set; }

        [Display(Name = "Enable Delete")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool EnableDelete { get; set; }

        [Display(Name = "Company I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CompanyID { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string CompanyName { get; set; }

        [Display(Name = "Status I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public int StatusID { get; set; }

        [Display(Name = "Status Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string StatusName { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Effective Date")]
        public DateTime? EffectiveDate { get; set; }

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
        public bool? IsDeleted { get; set; }

        [Display(Name = "Delete By")]
        public int? DeleteBy { get; set; }

        [Display(Name = "Delete On")]
        public DateTime? DeleteOn { get; set; }

        [Display(Name = "Delete Pc")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string DeletePc { get; set; }

 
      
    }
}
