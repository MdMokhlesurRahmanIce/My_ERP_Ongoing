using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public  class vmNotificationMail
    {
        public string nextUser { get; set; }
        public string companyName { get; set; }
        public string menuName { get; set; }
        public string customCode { get; set; }
        public string message { get; set; }
        public string currentUser { get; set; }
        public string comments { get; set; }
        public string nextUserEmailAddress { get; set; }
        public bool isApproved { get; set; }

    }
}
