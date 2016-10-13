using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Accounting;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Accounting.Interfaces;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Accounting.Factories
{
    public class FinishingProcessPriceMgt : iFinishingProcessPriceMgt
    {

        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnUser> GenericFactoryFor_CmnUser = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_CmnCombo = null;
        
        public List<PrdFinishingProcess> GetFinishingProcess(vmCmnParameters objcmnParam)
        {
            List<PrdFinishingProcess> lstPrdFinishingProcess = null;
            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstPrdFinishingProcess = (from spl in _ctxCmn.PrdFinishingProcesses.Where(m => m.IsActive == true && m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)
                                              select spl).ToList().Select(m => new PrdFinishingProcess { FinishingProcessID = m.FinishingProcessID, FinishingProcessName = m.FinishingProcessName }).ToList();

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            return lstPrdFinishingProcess;
        } 

        public IEnumerable<vmFinishingProcessPriceSetup> GetFinPricChngeGrdByFProcessID(vmCmnParameters objcmnParam, Int32 finishProcessID, out int recordsTotal)
        {
            IEnumerable<vmFinishingProcessPriceSetup> lstFinishPrice = null;
            IEnumerable<vmFinishingProcessPriceSetup> lstFinishPriceWithoutPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstFinishPriceWithoutPaging = (from rm in _ctxCmn.prdFinishingProcessPriceSetups.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false && m.FinishingProcessID == finishProcessID)

                                                   join fp in _ctxCmn.PrdFinishingProcesses on rm.FinishingProcessID equals fp.FinishingProcessID
                                                   join crncy in _ctxCmn.AccCurrencyInfoes on rm.CurrencyID equals crncy.Id

                                                   select new
                                                   {
                                                       ProcessPriceID = rm.ProcessPriceID,
                                                       ProcessPriceNo = rm.ProcessPriceNo,
                                                       FinishingProcessID = rm.FinishingProcessID,
                                                       Price = rm.Price,
                                                       CurrencyID = rm.CurrencyID,
                                                       CurrencyName = crncy.CurrencyName,
                                                       PriceDate = rm.PriceDate,
                                                       IsActive = rm.IsActive,
                                                       Remarks = rm.Remarks,
                                                       CompanyID = rm.CompanyID,
                                                       FinishingProcessName = fp.FinishingProcessName


                                                   }).ToList().Select(x => new vmFinishingProcessPriceSetup
                                                   {
                                                       ProcessPriceID = x.ProcessPriceID,
                                                       ProcessPriceNo = x.ProcessPriceNo,
                                                       FinishingProcessID = x.FinishingProcessID,
                                                       Price = x.Price,
                                                       CurrencyID = x.CurrencyID,
                                                       CurrencyName = x.CurrencyName,
                                                       PriceDate = x.PriceDate,
                                                       IsActive = x.IsActive,
                                                       Remarks = x.Remarks,
                                                       CompanyID = x.CompanyID,
                                                       FinishingProcessName = x.FinishingProcessName
                                                   }).ToList();

                    lstFinishPrice = lstFinishPriceWithoutPaging.OrderByDescending(x => x.ProcessPriceID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstFinishPriceWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstFinishPrice;
        } 
        public IEnumerable<vmFinishingProcessPriceSetup> GetFPPMasterList(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmFinishingProcessPriceSetup> lstFinishPrice = null;
            IEnumerable<vmFinishingProcessPriceSetup> lstFinishPriceWithoutPaging = null;
            recordsTotal = 0;
            using (_ctxCmn = new ERP_Entities())
            {
                try
                {
                    lstFinishPriceWithoutPaging = (from rm in _ctxCmn.prdFinishingProcessPriceSetups.Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false)

                                                   join fp in _ctxCmn.PrdFinishingProcesses on rm.FinishingProcessID equals fp.FinishingProcessID
                                                   join crncy in _ctxCmn.AccCurrencyInfoes on rm.CurrencyID equals crncy.Id

                                                   select new
                                                   {
                                                       ProcessPriceID = rm.ProcessPriceID,
                                                       ProcessPriceNo = rm.ProcessPriceNo,
                                                       FinishingProcessID = rm.FinishingProcessID,
                                                       Price = rm.Price,
                                                       CurrencyID = rm.CurrencyID,
                                                       CurrencyName = crncy.CurrencyName,
                                                       PriceDate = rm.PriceDate,
                                                       IsActive = rm.IsActive,
                                                       Remarks = rm.Remarks,
                                                       CompanyID = rm.CompanyID,
                                                       FinishingProcessName = fp.FinishingProcessName


                                                   }).ToList().Select(x => new vmFinishingProcessPriceSetup
                                                   {
                                                       ProcessPriceID = x.ProcessPriceID,
                                                       ProcessPriceNo = x.ProcessPriceNo,
                                                       FinishingProcessID = x.FinishingProcessID,
                                                       Price = x.Price,
                                                       CurrencyID = x.CurrencyID,
                                                       CurrencyName = x.CurrencyName,
                                                       PriceDate = x.PriceDate,
                                                       IsActive = x.IsActive,
                                                       Remarks = x.Remarks,
                                                       CompanyID = x.CompanyID,
                                                       FinishingProcessName = x.FinishingProcessName
                                                   }).ToList();

                    lstFinishPrice = lstFinishPriceWithoutPaging.OrderByDescending(x => x.ProcessPriceID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    recordsTotal = lstFinishPriceWithoutPaging.Count();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return lstFinishPrice;
        }
         
        public string SaveFinishProPricSetup(prdFinishingProcessPriceSetup objPrdFinishingProcessPriceSetup, int menuID)
        {
            GenericFactory_EF_CmnCombo = new CmnCombo_EF();
            string result = "";

            using (ERP_Entities _ctxCmn = new ERP_Entities())
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        List<prdFinishingProcessPriceSetup> lstFroUpdate = (from lupdate in _ctxCmn.prdFinishingProcessPriceSetups.Where(m => m.IsActive==true && m.CompanyID == objPrdFinishingProcessPriceSetup.CompanyID && m.FinishingProcessID == objPrdFinishingProcessPriceSetup.FinishingProcessID).ToList() select lupdate).ToList();
                        prdFinishingProcessPriceSetup objFpp = new prdFinishingProcessPriceSetup();
                        foreach(prdFinishingProcessPriceSetup fp in lstFroUpdate)
                        {
                            fp.IsActive = false;

                            //objFpp = fp;
                        }


                        long NextId = Convert.ToInt64(GenericFactory_EF_CmnCombo.getMaxID("prdFinishingProcessPriceSetup"));
                        string customCode = "";
                        string CustomNo = GenericFactory_EF_CmnCombo.getCustomCode(menuID, DateTime.Now, objPrdFinishingProcessPriceSetup.CompanyID, 1, 1);
                        if (!string.IsNullOrEmpty(CustomNo))
                        {
                            customCode = CustomNo;
                        }
                        else if (string.IsNullOrEmpty(CustomNo))
                        {
                            customCode = NextId.ToString();
                        }
                        //.........END for custom code............ //

                        string newProcessPriceNo = customCode;
                        objPrdFinishingProcessPriceSetup.ProcessPriceID = (int)NextId;
                        objPrdFinishingProcessPriceSetup.CreateOn = DateTime.Now;
                        objPrdFinishingProcessPriceSetup.CreatePc = HostService.GetIP();
                        objPrdFinishingProcessPriceSetup.ProcessPriceNo = newProcessPriceNo;


                        _ctxCmn.prdFinishingProcessPriceSetups.Add(objPrdFinishingProcessPriceSetup);
                        //............Update MaxID.................//
                        GenericFactory_EF_CmnCombo.updateMaxID("prdFinishingProcessPriceSetup", Convert.ToInt64(NextId));
                        //............Update CustomCode.............//
                        GenericFactory_EF_CmnCombo.updateCustomCode(menuID, DateTime.Now, objPrdFinishingProcessPriceSetup.CompanyID, 1, 1);

                        _ctxCmn.SaveChanges();

                        transactionScope.Complete();

                        result = newProcessPriceNo;
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
