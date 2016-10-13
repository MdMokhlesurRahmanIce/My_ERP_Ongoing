using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.EmailService
{
    public class RecoveryModel
    {
        public string UserID { get; set; }
        public string LoginID { get; set; }
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public string RequestedIP { get; set; }
    }
}
