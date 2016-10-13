using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Production.Interfaces;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class DyingOperationSetupMgt : iDyingOperationSetupMgt
    {
        private iGenericFactory_EF<PrdDyingOperationSetup> GFactory_EF_PrdDyingOperationSetup = null;


        public IEnumerable<vmPrdDyingOperationSetup> GetAllOperationSetup(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            List<vmPrdDyingOperationSetup> list = new List<vmPrdDyingOperationSetup>();
            recordsTotal = 0;
            try
            {
                #region Convert It Into Sp
                using (ERP_Entities dbContext = new ERP_Entities())
                {
                    list = (from master in dbContext.PrdDyingOperationSetups
                            join process in dbContext.PrdDyingProcesses on master.DyingProcessID equals process.DyingProcessID
                            join operation in dbContext.PrdDyingOperations on master.OperationID equals operation.OperationID
                            join item in dbContext.CmnItemMasters on master.ItemID equals item.ItemID into leftItemGroup
                            from lig in leftItemGroup.DefaultIfEmpty()
                            join chemical in dbContext.CmnItemMasters on master.ChemicalItemID equals chemical.ItemID into leftChemicalGroup
                            from lcg in leftChemicalGroup.DefaultIfEmpty()
                            join unit in dbContext.CmnUOMs on master.UnitID equals unit.UOMID into leftUnitGroup
                            from lug in leftUnitGroup.DefaultIfEmpty()
                            where master.IsDeleted == false
                            select new vmPrdDyingOperationSetup
                            {
                                OperationSetupID = master.OperationSetupID,
                                ItemID = lig.ItemID,
                                ArticleNo = lig.ArticleNo,
                                ChemicalItemID = lcg.ItemID,
                                ChemicalArticleNo = lcg.ArticleNo,
                                DyingProcessID = process.DyingProcessID,
                                ProcessName = process.ProcessName,
                                OperationID = operation.OperationID,
                                OperationName = operation.OperationName,
                                MinQty = master.MinQty,
                                MaxQty = master.MaxQty,
                                UnitID = lug.UOMID,
                                UOMName = lug.UOMName,
                                IsDeleted = false
                            }).Distinct().ToList();
                    recordsTotal = list.Count();
                    list = list.OrderByDescending(x => x.OperationSetupID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
                #endregion Convert It Into Sp
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return list;
        }

        public int SaveOperationSetup(List<PrdDyingOperationSetup> masterList, UserCommonEntity commonEntity)
        {
            GFactory_EF_PrdDyingOperationSetup = new PrdDyingOperationSetup_EF();
            List<PrdDyingOperationSetup> listInsertion = new List<PrdDyingOperationSetup>();
            List<PrdDyingOperationSetup> listUpDate = new List<PrdDyingOperationSetup>();

            int masterID = (int)masterList.FirstOrDefault().OperationSetupID;
            int result = 0;
            try
            {
                listInsertion = masterList.Where(x => x.OperationSetupID == 0 && x.IsDeleted == false).ToList();
                listUpDate = masterList.Where(x => x.OperationSetupID != 0).ToList();
                Int64 ID = Convert.ToInt64(GFactory_EF_PrdDyingOperationSetup.getMaxID("PrdDyingOperationSetup"));
                Int64 LastID = ID + listInsertion.Count();
                GFactory_EF_PrdDyingOperationSetup.updateMaxID("PrdDyingOperationSetup", Convert.ToInt64(LastID));
                for (int num = 0; num < listInsertion.Count(); num++)
                {
                    listInsertion[num].OperationSetupID = ID + num;
                    if (listInsertion[num].ChemicalItemID == 0)
                    {
                        listInsertion[num].ChemicalItemID = null;
                    }
                    listInsertion[num].CompanyID = (int)commonEntity.loggedCompnyID;
                }

                for (int num = 0; num < listUpDate.Count(); num++)
                {
                    if (listUpDate[num].ChemicalItemID == 0)
                    {
                        listUpDate[num].ChemicalItemID = null;
                    }
                    listUpDate[num].CompanyID = (int)commonEntity.loggedCompnyID;
                }

                if (listInsertion.ToList().Count > 0)
                {
                    GFactory_EF_PrdDyingOperationSetup.InsertList(listInsertion);
                    GFactory_EF_PrdDyingOperationSetup.Save();
                }

                if (listUpDate.Count > 0)
                {
                    GFactory_EF_PrdDyingOperationSetup.UpdateList(listUpDate);
                    GFactory_EF_PrdDyingOperationSetup.Save();
                }
                result = 1;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return result;
        }

        public string DeleteChemicalOperation(vmCmnParameters objcmnParam)
        {
            string result = "";
            using (var transaction = new TransactionScope())
            {
                GFactory_EF_PrdDyingOperationSetup = new PrdDyingOperationSetup_EF();

                var MasterItem = new PrdDyingOperationSetup();

                //For Update Master Detail
                var MasterAll = GFactory_EF_PrdDyingOperationSetup.GetAll().Where(x => x.OperationSetupID == objcmnParam.id && x.CompanyID == objcmnParam.loggedCompany);                
                //-------------------END----------------------
                try
                {
                    MasterItem = MasterAll.First(x => x.OperationSetupID == objcmnParam.id);
                    MasterItem.CompanyID = objcmnParam.loggedCompany;
                    MasterItem.DeleteBy = objcmnParam.loggeduser;
                    MasterItem.DeleteOn = DateTime.Now;
                    MasterItem.DeletePc = HostService.GetIP();
                    MasterItem.IsDeleted = true;

                    if (MasterItem != null)
                    {
                        GFactory_EF_PrdDyingOperationSetup.Update(MasterItem);
                        GFactory_EF_PrdDyingOperationSetup.Save();
                    }

                    transaction.Complete();
                    result = MasterItem.PrdDyingProcess.ProcessName;
                }
                catch (Exception e)
                {
                    result = "";
                    e.ToString();
                }
            }
            return result;
        }
    }
}
