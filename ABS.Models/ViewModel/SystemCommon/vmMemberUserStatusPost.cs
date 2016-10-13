using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmMemberUserStatusPost
    {
        public string PostBy { get; set; }
        public string PostContent { get; set; }
        public string newfileName { get; set; }
        public string PostType { get; set; }
        public string IsPrivate { get; set; }
    }
}
