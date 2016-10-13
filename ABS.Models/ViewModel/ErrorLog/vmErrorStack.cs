using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.ErrorLog
{
    public class vmErrorStack
    {
        public int CompanyID { get; set; }
        
        public string ErrorMethod { get; set; }
        public string ErrorFile { get; set; }
        public string ErrorLine { get; set; }
        public string ErrorDate { get; set; }
        public string ErrorPath { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorStack { get; set; }
    }
}
