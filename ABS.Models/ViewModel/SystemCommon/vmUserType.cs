using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmUserType
    {
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> LoggedUser { get; set; }

        public Nullable<int> UserTypeID { get; set; }
        public string UserTypeName { get; set; }
        public Nullable<int> ParentID { get; set; }
    }
}
