using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public  class vmNotification
    {
        public int MasterID { get; set; }
        
        [Display(Name = "F D O Type I D")]
        public int? HDOTypeID { get; set; }

        [Display(Name = "Transaction Type Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string TransactionTypeName { get; set; }

        [Display(Name = "Menu I D")]
        public int? MenuID { get; set; }
        public string MenuPath { get; set; }
        
        [Display(Name = "F D O Master I D")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 FDOMasterID { get; set; }


        [Display(Name = "F D O No")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string HDONo { get; set; }

        [Display(Name = "Status I D")]
        public int? StatusID { get; set; }

        [Display(Name = "Status Name")]
        [StringLength(200, ErrorMessage = "Maximum length is {1}")]
        public string StatusName { get; set; }
    }
}
