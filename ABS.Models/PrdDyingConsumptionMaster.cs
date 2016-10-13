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
    
    public partial class PrdDyingConsumptionMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PrdDyingConsumptionMaster()
        {
            this.PrdDyingConsumptionDetails = new HashSet<PrdDyingConsumptionDetail>();
        }
    
        public long DyingConsumptionID { get; set; }
        public string DyingConsumptionNo { get; set; }
        public long SetID { get; set; }
        public long ItemID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> IndigoStart { get; set; }
        public Nullable<decimal> IndigoStop { get; set; }
        public Nullable<decimal> BlackStart { get; set; }
        public Nullable<decimal> BlackStop { get; set; }
        public Nullable<int> OperationID { get; set; }
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrdDyingConsumptionDetail> PrdDyingConsumptionDetails { get; set; }
        public virtual PrdDyingMRRSet PrdDyingMRRSet { get; set; }
        public virtual PrdDyingOperation PrdDyingOperation { get; set; }
        public virtual CmnItemMaster CmnItemMaster { get; set; }
    }
}