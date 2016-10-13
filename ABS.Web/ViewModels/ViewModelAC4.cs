using ABS.Models;
using ABS.Web.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ABS.Web.ViewModels
{
    public class ViewModelAC4
    {
        [DisplayName("Name (Level-1)")]
        [Required(ErrorMessage = "This field is required.")]
        public int AC1Id { get; set; }
        [DisplayName("Name (Level-2)")]
        [Required(ErrorMessage = "This field is required.")]
        public int AC2Id { get; set; }
        [DisplayName("Name (Level-3)")]
        [Required(ErrorMessage = "This field is required.")]
        public int AC3Id { get; set; }
        public int Id { get; set; }
        [DisplayName("Level-4 Code")]
        [Required(ErrorMessage = "Code is required.")]
        public string AC4ManualCode { get; set; }
        [DisplayName("Level-4")]

        [Required(ErrorMessage = "Name is required.")]
        [Remote("IsNameAvailble4", "AC3", ErrorMessage = "Name Already Exist.")]

        public string AC4Name { get; set; }
        [DisplayName("Level-3")]
        public string AC3Name { get; set; }


        [DisplayName("Level-2")]
        public string AC2Name { get; set; }


        [DisplayName("Level-1")]
        public string AC1Name { get; set; }
        public Nullable<bool> Transfered { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [DisplayName("Is Level Completed")]
        public bool IsAccountLedger { get; set; }

        public virtual AccAC3 AC3 { get; set; }
        public virtual ICollection<AccACDetail> ACDetails { get; set; }


        public AccAC4 AC4Entity()
        {

            var objAC4 = new AccAC4();
            objAC4.AC1Id = this.AC1Id;
            objAC4.AC2Id = this.AC2Id;
            objAC4.AC3Id = this.AC3Id;
            objAC4.Id = this.Id;
            objAC4.AC4ManualCode = this.AC4ManualCode;
            objAC4.AC4Name = this.AC4Name;
            objAC4.Transfered = this.Transfered;
            objAC4.AddedBy = this.AddedBy;
            objAC4.DateAdded = this.DateAdded;
            objAC4.UpdatedBy = this.UpdatedBy;
            objAC4.DateUpdated = this.DateUpdated;
            objAC4.IsActive = this.IsActive;
            objAC4.IsAccountLedger = this.IsAccountLedger;
            return objAC4;


        }

        public AccAC4 MakeAc4(int userId)
        {
            var ac4 = AC4Entity();
            var autoNumber1 = UniqueCode.GetAutoNumber("AC4");
            ac4.Id = Convert.ToInt32(autoNumber1);
            ac4.AC1Id = ac4.AC1Id;
            ac4.AC2Id = ac4.AC2Id;
            ac4.AC3Id = ac4.AC3Id;

            ac4.AC4ManualCode = UniqueCode.GetAccountLedgerCode("AL4", autoNumber1);
            ac4.AC4Name = ac4.AC4Name;
            ac4.IsActive = true;
            ac4.Transfered = false;
            ac4.DateAdded = DateTime.Now;
            ac4.AddedBy = userId;
            ac4.DateUpdated = DateTime.Now;
            ac4.UpdatedBy = userId;
            return ac4;
        }
    }
}