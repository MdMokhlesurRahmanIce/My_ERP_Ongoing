using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
   public class VmItemMater
    {
        public Int64? ItemID { get; set; }
        public string UniqueCode { set; get; }
        public string ArticleNo { get; set; }
        public string ItemType { set; get; }
        public string ItemGroup { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Brand { set; get; }
        public string Model { set; get; }
        public string Description { get; set; }
        public string Note { set; get; }



        public int? UnitId { get; set; }

        public int? ColorId { get; set; }

        public int? SizeId { get; set; }

        public int? BrandId { get; set; }

        public int? ModelId { get; set; }

        public int? ItemGropID { get; set; }

        public decimal? Count { get; set; }
    }
}
