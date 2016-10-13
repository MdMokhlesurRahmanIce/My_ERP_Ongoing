using ABS.Models;
using ABS.Web.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ABS.Web.ViewModels
{
    public class ViewModelAC5
    {

        public ViewModelAC5()
        {
            Ac5 = new AccACDetail();
        }
        [DisplayName("Name (Level-1)")]
        [Required(ErrorMessage = "This field is required.")]
        public int AC1Id { get; set; }
        [DisplayName("Name (Level-2)")]
        [Required(ErrorMessage = "This field is required.")]
        public int AC2Id { get; set; }
        [DisplayName("Name (Level-3)")]
        [Required(ErrorMessage = "This field is required.")]
        public int AC3Id { get; set; }
        [DisplayName("Name (Level-4)")]
        [Required(ErrorMessage = "This field is required.")]
        public int AC4Id { get; set; }
        public int Id { get; set; }
        [DisplayName("Level-5 Code")]
        [Required(ErrorMessage = "Code is required.")]
        public string ACode { get; set; }
        [DisplayName("Level-5")]

        [Required(ErrorMessage = "Name is required.")]
        [Remote("IsNameAvailble5", "AC3", ErrorMessage = "Name Already Exist.")]

        public string ACName { get; set; }
        [DisplayName("Level-4")]
        public string AC4Name { get; set; }
        [DisplayName("Level-3")]
        public string AC3Name { get; set; }


        [DisplayName("Level-2")]
        public string AC2Name

        { get; set; }


        [DisplayName("Level-1")]
        public string AC1Name { get; set; }
        public string EntryType { get; set; }
        public string TransactionType { get; set; }
        public string ReportHead { get; set; }
        public Nullable<bool> Transfered { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<double> OpeningBalance { get; set; }
        public Nullable<System.DateTime> OpeningBalanceDate { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual AccAC4 AC4 { get; set; }
        public AccACDetail Ac5 { get; set; }
        public virtual ICollection<AccVoucherDetail> VoucherDetails { get; set; }

        public AccACDetail AC5Entity()
        {

            Ac5.AC1Id = this.AC1Id;
            Ac5.AC2Id = this.AC2Id;
            Ac5.AC3Id = this.AC3Id;
            Ac5.AC4Id = this.AC4Id;
            Ac5.Id = this.Id;
            Ac5.ACode = this.ACode;
            Ac5.ACName = this.ACName;
            Ac5.EntryType = this.EntryType;
            Ac5.TransactionType = this.TransactionType;
            Ac5.ReportHead = this.ReportHead;
            Ac5.Transfered = this.Transfered;
            Ac5.AddedBy = this.AddedBy;
            Ac5.DateAdded = this.DateAdded;
            Ac5.UpdatedBy = this.UpdatedBy;
            Ac5.DateUpdated = this.DateUpdated;
            Ac5.OpeningBalance = this.OpeningBalance;
            Ac5.OpeningBalanceDate = this.OpeningBalanceDate;
            Ac5.IsActive = this.IsActive;
            return Ac5;
        }




        public AccACDetail MakeAc5(int userId)
        {


            var autoNumber2 = UniqueCode.GetAutoNumber("AC5");
            Ac5.Id = Convert.ToInt32(autoNumber2);

            Ac5.AC1Id = AC1Id;
            Ac5.AC2Id = AC2Id;
            Ac5.AC3Id = AC3Id;
            Ac5.ACName = ACName;
            Ac5.AC4Id = AC4Id;

            Ac5.ACode = UniqueCode.GetAccountLedgerCode("AL5", autoNumber2);
            Ac5.IsActive = true;
            Ac5.Transfered = false;
            Ac5.DateAdded = DateTime.Now;
            Ac5.AddedBy = userId;
            Ac5.DateUpdated = DateTime.Now;
            Ac5.UpdatedBy = userId;

            return Ac5;

        }
    }




}