using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Models.ViewModel.SystemCommon
{
    public class vmUser
    {
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> LoggedUser { get; set; }

        public Nullable<int> UserID { get; set; }
        public string CustomCode { get; set; }
        public string UserTypeName { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> UserTypeID { get; set; }

        public Nullable<int> UserGroupID { get; set; }
        public string UserGroup { get; set; }

        public Nullable<int> UserTitleID { get; set; }
        public string UserTitle { get; set; }

        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public string UserFullName { get; set; }

        public Nullable<int> ReligionID { get; set; }

        public Nullable<int> AcDetailID { get; set; }

        public string ACName { get; set; }

        public string Religion { get; set; }

        public Nullable<int> GenderID { get; set; }
        public string Gender { get; set; }

        public string LoginID { get; set; }
        public string LoginEmail { get; set; }
        public string LoginPhone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public Nullable<int> LanguageID { get; set; }
        public Nullable<int> TimezoneID { get; set; }
        public Nullable<bool> AllowMultipleLogin { get; set; }
        public string NoOfLogin { get; set; }

        public Nullable<DateTime> ActivationDate { get; set; }
        public string IsActive { get; set; }
        public int IsOnlineAccount { get; set; }

        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string SpouseNane { get; set; }

        public string ParAddress1 { get; set; }
        public string ParAddress2 { get; set; }
        public int? ParCountryID { get; set; }
        public int? ParStateID { get; set; }
        public int? ParCityID { get; set; }

        public string PreAddress1 { get; set; }
        public string PreAddress2 { get; set; }
        public int? PreCountryID { get; set; }
        public int? PreStateID { get; set; }
        public int? PreCityID { get; set; }

        public string UniqueIdentity { get; set; }

        public Nullable<int> BloodGroupID { get; set; }
        public string BloodGroup { get; set; }

        public decimal Height { get; set; }
        public DateTime DOB { get; set; }
        public string PassportNO { get; set; }
        public string ImageUrl { get; set; }
        public string FingerUrl { get; set; }
        public string SignatUrl { get; set; }
        public string ImageName { set; get; }
        public string SignatName { set; get; }
        public string NID { get; set; }
        public Nullable<int> MobileNo { get; set; }
        public string Email { get; set; }

        public Nullable<int> DesignationID { get; set; }
        public string Designation { get; set; }

        public Nullable<int> DepartmentID { get; set; }
        public string Department { get; set; }

        public Nullable<int> JobContractTypeID { get; set; }
        public string JobContractType { get; set; }


        public string UserType { get; set; }

        public Nullable<int> ReturnValue { get; set; }
        public string RequestedIP { get; set; }
        public string CompanyName { get; set; }
        public string ParCountryName { set; get; }
        public string ParStateName { set; get; }
        public string ParCityName { set; get; }
        public string PreCountryName { get; set; }
        public string PreStateName { set; get; }
        public string PreCityName { get; set; }
       

    }

    public class MemberUserStatusPost
    {
        public string PostBy { get; set; }
        public string PostContent { get; set; }
        public string newfileName { get; set; }
        public string PostType { get; set; }
        public string IsPrivate { get; set; }

    }
}
