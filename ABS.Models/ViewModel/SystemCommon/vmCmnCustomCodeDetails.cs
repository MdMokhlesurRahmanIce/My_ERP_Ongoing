using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmCmnCustomCodeDetails
    {
        [Required(ErrorMessage = "Required")]
        public int RecordDetailID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string CustomCode { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CustomCodeID { get; set; }
        public string ParameterName { get; set; }
        public int? Length { get; set; }
        public string Seperator { get; set; }
        public int? Sequence { get; set; }
        public int? StatusID { get; set; }
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
