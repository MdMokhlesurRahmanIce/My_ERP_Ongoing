using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Production
{
    public class vmFinishingChemicalPreparation
    {
        //****************Master************************
        public int? FinChemicalStupID { get; set; }
        public int? FinishingProcessID { get; set; }
        public string FinishingProcessName { get; set; }
        public string FinChemicalStupNo { get; set; }
        public DateTime? PreparationDate { get; set; }
        public string Remarks { get; set; }
        //**********************************************

        //****************Detail************************
        public int? FinChemicalStupDetailID { get; set; }
        public long? ChemicalID { get; set; }
        public decimal? MaxQty { get; set; }
        public decimal? MinQty { get; set; }
        public int? UnitID { get; set; }
        public string UOMName { get; set; }

        public int? SlNo { get; set; }
        public string ModelState { get; set; }
    }
}
