using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmTransactionType
    {
        public int TransactionTypeID { get; set; }
        public string TransactionTypeName { get; set; }
       public string Menu { set; get; }
       public int MenuID { get; set; }
    }
}
