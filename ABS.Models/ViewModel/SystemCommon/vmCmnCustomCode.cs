using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
     
    public class vmCmnCustomCode
    {
        [Required(ErrorMessage = "Required")]
        public int RecordID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string CustomCode { get; set; }
        public int? MenuID { get; set; }
        public String MenuName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool? IsCompany { get; set; }
        public bool? IsOrganogramCode { get; set; }
        public int? StatusID { get; set; }
        public int? OrganogramID { get; set; }
        public String OrganogramName { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CompanyID { get; set; }
        public String CompanyName { get; set; }
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
