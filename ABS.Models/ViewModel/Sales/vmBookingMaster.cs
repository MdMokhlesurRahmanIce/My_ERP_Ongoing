using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmBookingMaster
    {
        public Int64 BookingID { get; set; }
        public string BookingNo { get; set; }
        public int? PITypeID { get; set; }
        public DateTime BookingDate { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }     
        public int? BuyerID { get; set; }
        public int? BuyerRefID { get; set; }
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public int? TransactionTypeID { get; set; }
        public bool IsActive { get; set; }
        public bool? IsPICompleted { get; set; }
        public bool? IsLCCompleted { get; set; }
        public bool? IsHDOCompleted { get; set; }
        //public int? StatusID { get; set; }
        //public int? StatusBy { get; set; }
        //public DateTime? StatusOn { get; set; }
        public int? CreateBy { get; set; }
        //public DateTime? CreateOn { get; set; }
        //public string CreatePc { get; set; }
        //public int? UpdateBy { get; set; }
        //public DateTime? UpdateOn { get; set; }
        //public string UpdatePc { get; set; }
        //public bool IsDeleted { get; set; }
        //public int? DeleteBy { get; set; }
        //public DateTime? DeleteOn { get; set; }
        //public string DeletePc { get; set; }
        public string CompanyName { get; set; }
        public string BuyerFullName { get; set; }
        public string BuyerReferenceFullName { get; set; }
        public string CustomCode { get; set; }

    }
}
