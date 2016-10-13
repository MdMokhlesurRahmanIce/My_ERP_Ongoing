using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;

using ABS.Models.ViewModel.Sales;
using ABS.Service.AllServiceClasses;
using ABS.Service.Inventory.Interfaces;
using System.Collections;
using ABS.Models.ViewModel.Inventory;
using System.Transactions;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Utility;


namespace ABS.Service.Inventory.Factories
{
    public class FundRequisitionMgt : iFundRequisitionMgt
    {
       private iGenericFactory_EF<PurchaseFR> GenericFactory_EF_FundRequisition = null;

       /// No CompanyID Provided
       
        public IEnumerable<CmnBank> GetAllBank(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnBank> objBank = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {                   
                    objBank = _ctxCmn.CmnBanks.Where(x => x.IsDeleted == false).OrderBy(s => s.BankID).ToList();                                    
                                    
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }            
            return objBank;
        }

        public string SaveFundRequisition(PurchaseFR purchaseFR, int menuID)
        {
            GenericFactory_EF_FundRequisition = new PurchaseFR_EF();
            string result = "";

            if (purchaseFR.FRID > 0)
            {
            //    int Result = 0;
            //    try
            //    {
            //        Hashtable htmaster = new Hashtable();
            //        htmaster.Add("IssueID", IssueMaster.IssueID);
            //        htmaster.Add("IssueDate", IssueMaster.IssueDate);
            //        htmaster.Add("IssueBy", IssueMaster.IssueBy);
            //        htmaster.Add("ToDepartmentID", IssueMaster.ToDepartmentID);
            //        htmaster.Add("ToCompanyID", IssueMaster.ToCompanyID);
            //        htmaster.Add("Comments", IssueMaster.Comments);
            //        htmaster.Add("UpdateBy", 1);
            //        htmaster.Add("UpdateOn", DateTime.Now);
            //        htmaster.Add("UpdatePc",  HostService.GetIP());
            //        string Query = "[Put_InvIssueMaster]";
            //        //using (GenericFactory_GF_IssueMaster = new InvIssueMaster_GF())
            //        //{
            //        //    Result = GenericFactory_GF_IssueMaster.ExecuteCommand(Query, htmaster);

            //        //}
            //        foreach (InvIssueDetail ivrd in IssueDetails)
            //        {
            //            Hashtable ht = new Hashtable();
            //            ht.Add("IssueDetailID", ivrd.IssueDetailID);
            //            ht.Add("ItemID", ivrd.ItemID);
            //            ht.Add("IssueQty", ivrd.IssueQty);
            //            ht.Add("Amount", ivrd.Amount);
            //            ht.Add("UpdateBy", 1);
            //            ht.Add("UpdateOn", DateTime.Now);
            //            ht.Add("UpdatePc",  HostService.GetIP());
            //            string spQuery = "[Put_InvIssuedDetail]";
            //            //using (GenericFactory_GF_IssueDetail = new InvIssueDetail_GF())
            //            //{
            //            //    Result = GenericFactory_GF_IssueDetail.ExecuteCommand(spQuery, ht);
            //            //}

            //        }

                //}
                //catch (Exception e)
                //{
                //    e.ToString();
                //    Result = 0;
                //}
                //result = Result.ToString();
            }
            else
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        using(GenericFactory_EF_FundRequisition = new PurchaseFR_EF())
                        {
                        
                        long NextId = Convert.ToInt16(GenericFactory_EF_FundRequisition.getMaxID("PurchaseFR"));

                        //.........END for custom code............ //
                        string customCode = "";
                        string CustomNo = customCode = GenericFactory_EF_FundRequisition.getCustomCode(menuID, DateTime.Now, purchaseFR.CompanyID, 1, 1);
                        if (customCode != "")
                        {
                            customCode = CustomNo;
                        }
                        else
                        {
                            customCode = NextId.ToString();
                        }
                        //.........END for custom code............ //

                        purchaseFR.FRID = NextId;                     
                        purchaseFR.CreatePc =  HostService.GetIP();
                        purchaseFR.FRNo = customCode;

                        GenericFactory_EF_FundRequisition.Insert(purchaseFR);
                        GenericFactory_EF_FundRequisition.Save();
                        //............Update MaxID.................//
                        GenericFactory_EF_FundRequisition.updateMaxID("PurchaseFR", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                       GenericFactory_EF_FundRequisition.updateCustomCode(menuID, DateTime.Now, purchaseFR.CompanyID, 1, 1);
                       
                        transaction.Complete();
                        result = customCode;

                        }
                       
                    }
                    catch (Exception e)
                    {
                        result = "";
                    }
                }

            }
            return result;
        }

    }
}
