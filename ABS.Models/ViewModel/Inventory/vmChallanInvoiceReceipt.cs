using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Inventory
{
   public class vmChallanInvoiceReceipt 
    {
         public long? ChallanInvoiceReceiptID { get; set; } 
         public string ChallanInvoiceReceiptNo { get; set; } 
         public bool? IsDeleted { get; set;}
         public bool? IsCompleted { get; set; }
         public DateTime? ChallanInvoiceReceiptDate { get; set; } 

    }
}
