using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ABS.Models
{
    class DbPartialClasses
    {
    }
    public partial class CmnCompany
    {
        public int MenuID { get; set; }
        public int UserLogInID { get; set; }
    }
    public partial class CmnCustomCode
    {
        public String EntryStateStatus { get; set; }
    }

    public partial class CmnCustomCodeDetail
    {
        public String EntryStateStatus { get; set; }
    }

    public partial class PrdDyingChemicalSetup
    {
        public String EntryStateStatus { get; set; }
    }

    public partial class PrdDyingChemicalSetupDetail
    {
        public String EntryStateStatus { get; set; }
    }
    public partial class ChildMenues
    {
        public int MenuID { get; set; }
        public int? ParentID { get; set; }

        public string MenuPath { get; set; }
        public string MenuIconCss { get; set; }
        public int? TransactionTypeID { get; set; }
        public string TransactionTypeName { get; set; }
    }


    public partial class UserCommonEntity
    {
        public int? loggedCompnyID { get; set; }
        public int? loggedUserID { get; set; }

        public int? loggedUserBranchID { get; set; }
        public int? loggedUserDepartmentID { get; set; }
        
        public int? currentMenuID { get; set; }
        public int? currentModuleID { get; set; }

        public int? currentTransactionTypeID { get; set; }

        public object MenuList { get; set; }

        public object ChildMenues { get; set; }
    }

    public partial class NotificationEntity
    {
        public String MenuName { get; set; }
        public string MenuShortName { get; set;}
        public String MenuPath { get; set; }
        
        public String CustomCode { get; set; }
        public String StatusName { get; set; }
        public int? RecordID { get; set; }
        public int? TransactionID { get; set; }
        public int? MenuID { get; set; }

        public int? WorkFlowID { get; set; }
        public int? currentSequence { get; set; }
        public int? WFUserID { get; set; }
        public string WFUserName { get; set; }
        public int? NextSequence { get; set; }
        public string WFDStatusID { get; set; }
        public int? NextWFUserID { get; set; }
        public String NextWFUserName { get; set; }
        public int? PREVSequence { get; set; }
        public int? PrevWFUserID { get; set; }
        public String PrevWFUserName { get; set; }
        public int? CreatorID { get; set; }
        public int? LoggedCompanyID { get; set; }
        public int? LoggedUserID { get; set; }
        public long? TargetUserID { get; set; }
        public long? TeamID { get; set; }
        public bool IsTeam { get; set; }
        public String CreatorName { get; set; }
        public string Comments { get; set; }
        public string MessageName { get; set; }
        public DateTime MessageDate { get; set; }
    }

    public partial class PrdDyingConsumptionChemicalM
    {
        public string EntityState { get; set; }
    }

    public partial class PrdDyingConsumptionChemical
    {
        public string EntityState { get; set; }
    }
    public partial class PrdDyingConsumptionMaster
    {
        public string EntityState { get; set; }
        public string SetNo { get; set; }
        public string ArticleNo { get; set; }
        public string Department { get; set; }
        public string OperationName { get; set; }

    }

    public partial class PrdDyingConsumptionDetail
    {
        public string EntityState { get; set; }
    }

    public partial class CmnItemMaster
    {
        public int? AcDetailID { get; set; }
    }

}
