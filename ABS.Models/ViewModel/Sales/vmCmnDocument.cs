using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sales
{
    public class vmCmnDocument
    {
        public long DocumentID { get; set; }
        public Nullable<int> DocumentPahtID { get; set; }
        public string DocumentName { get; set; }
        public Nullable<long> TransactionID { get; set; }
        public Nullable<int> TransactionTypeID { get; set; }
        public Nullable<int> CompanyID { get; set; }
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
        public string FullDocumentPath { get; set; }
    }
}
