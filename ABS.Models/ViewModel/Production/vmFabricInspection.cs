using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmFabricInspection
    {
        public Int64 FinishingMRRID { get; set; }
        public string FinishingMRRNo { get; set; }
        public int FinishingMRRTypeID { get; set; }
        public Int64 ItemID { get; set; }
        public Int64 SetID { get; set; }

        public string ArticleNo { get; set; }
        public Int64 WeavingMRRID { get; set; }
        public Int64 SizeMRRID { get; set; }
        public string ColorName { get; set; }
        public string SetNo { get; set; }





    }
}
