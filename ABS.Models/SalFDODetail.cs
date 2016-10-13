//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SalFDODetail
    {
        public long FDODetailsID { get; set; }
        public long FDOMasterID { get; set; }
        public long ItemID { get; set; }
        public decimal RollNo { get; set; }
        public decimal QuantitYds { get; set; }
        public decimal QuantityKg { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public long PIID { get; set; }
        public decimal NetQtyKg { get; set; }
        public Nullable<decimal> GrossQtyKg { get; set; }
        public Nullable<long> LotID { get; set; }
        public Nullable<long> BatchID { get; set; }
        public Nullable<long> GradeID { get; set; }
        public Nullable<bool> IsDCCompleted { get; set; }
        public int CompanyID { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatePc { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatePc { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteOn { get; set; }
        public string DeletePc { get; set; }
    
        public virtual SalFDOMaster SalFDOMaster { get; set; }
    }
}