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
    
    public partial class PrdSizingMRRBreakage
    {
        public long SizeBreakageID { get; set; }
        public long SizeBreakageMasterID { get; set; }
        public int BreakageID { get; set; }
        public int NoOfBreakage { get; set; }
        public string BreakageType { get; set; }
        public Nullable<System.DateTime> BreakageDate { get; set; }
        public string Description { get; set; }
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
        public Nullable<long> MachineID { get; set; }
    }
}
