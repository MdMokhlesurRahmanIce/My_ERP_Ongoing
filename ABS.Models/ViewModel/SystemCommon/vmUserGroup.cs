using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmUserGroup
    {
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> LoggedUser { get; set; }

        public Nullable<int> UserGroupID { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> Sequence { get; set; }
    }
}
