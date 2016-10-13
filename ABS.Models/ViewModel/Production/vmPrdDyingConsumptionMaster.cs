using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmPrdDyingConsumptionMaster
    {
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

        public string EntityState { get; set; }
        public string SetNo { get; set; }
        public string ArticleNo { get; set; }
        public string Department { get; set; }
        public string OperationName { get; set; }
    }
}