using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.Sample
{
    public partial class vmCustomer
    {
        public long CustomerID { get; set; }

        [Display(Name = "Player Name")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
