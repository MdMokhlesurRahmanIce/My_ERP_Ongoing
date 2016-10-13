using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections;
using ABS.Utility;

namespace ABS.Service.Production.Factories
{
    public class SetWiseMachineSetupMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<PrdDyingMachineSetup> GFactory_EF_PrdDyingMachineSetup = null;
        private iGenericFactory_EF<PrdDyingMachineSetupDetail> GFactory_EF_PrdDyingMachineSetupDetail = null;

        public IEnumerable<vmItemSetSetup> GetDetailBox(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objBox = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objBox = (from MP in _ctxCmn.PrdDyingMachineParts
                              where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                               && MP.MachineID == objcmnParam.id
                              //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                              orderby MP.MachineID
                              select new
                              {
                                  //MachinePartID = MP.MachineID,
                                  MachinePartID = MP.MachinePartID,
                                  MachinePartName = MP.MachinePartName
                              }).ToList().Select(x => new vmItemSetSetup
                              {
                                  MachinePartID = x.MachinePartID,
                                  MachinePartName = x.MachinePartName
                              }).ToList();

                    recordsTotal = objBox.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBox;
        }

        public IEnumerable<vmItemSetSetup> GetSetWiseMachineSetupMaster(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objSetWiseMaster = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objSetWiseMaster = (from MP in _ctxCmn.PrdDyingMachineSetups
                                        join CM in _ctxCmn.CmnItemMasters on MP.ItemID equals CM.ItemID
                                        where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                                        //&& MP.MachineID == objcmnParam.id
                                        //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                        orderby MP.SetupID descending
                                        select new
                                        {
                                            SetupID = MP.SetupID,
                                            ItemID = MP.ItemID,
                                            ArticleNo = CM.ArticleNo,
                                            MachineID = MP.MechineID,
                                            MachineName = CM.ItemName,
                                            Speed = MP.Speed == null ? 0 : MP.Speed,
                                            Moiture = MP.Moiture == null ? 0 : MP.Moiture,
                                            KGPreMin = MP.KGPreMin == null ? 0 : MP.KGPreMin
                                        }).ToList().Select(x => new vmItemSetSetup
                                        {
                                            SetupID = x.SetupID,
                                            ItemID = x.ItemID,
                                            ArticleNo = x.ArticleNo,
                                            MachineID = x.MachineID,
                                            MachineName = x.MachineName,
                                            Speed = x.Speed,
                                            Moiture = x.Moiture,
                                            KGPreMin = x.KGPreMin
                                        }).ToList();

                    recordsTotal = objSetWiseMaster.Count();
                    objSetWiseMaster = objSetWiseMaster.OrderByDescending(x => x.SetupID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSetWiseMaster;
        }

        public vmItemSetSetup GetSetWiseMachineSetupMasterByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            vmItemSetSetup objSetWiseMasterByID = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objSetWiseMasterByID = (from MP in _ctxCmn.PrdDyingMachineSetups
                                            join CM in _ctxCmn.CmnItemMasters on MP.ItemID equals CM.ItemID
                                            join MM in _ctxCmn.CmnItemMasters on MP.MechineID equals MM.ItemID
                                            where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                                            && MP.SetupID == objcmnParam.id
                                            //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                            orderby MP.SetupID descending
                                            select new
                                            {
                                                SetupID = MP.SetupID,
                                                ItemID = MP.ItemID,
                                                ArticleNo = CM.ArticleNo,
                                                MachineID = MP.MechineID,
                                                MachineName = MM.ItemName,
                                                Speed = MP.Speed == null ? 0 : MP.Speed,
                                                Moiture = MP.Moiture == null ? 0 : MP.Moiture,
                                                KGPreMin = MP.KGPreMin == null ? 0 : MP.KGPreMin
                                            }).Select(x => new vmItemSetSetup
                                            {
                                                SetupID = x.SetupID,
                                                ItemID = x.ItemID,
                                                ArticleNo = x.ArticleNo,
                                                MachineID = x.MachineID,
                                                MachineName = x.MachineName,
                                                Speed = x.Speed,
                                                Moiture = x.Moiture,
                                                KGPreMin = x.KGPreMin
                                            }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSetWiseMasterByID;
        }

        public IEnumerable<vmItemSetSetup> GetSetWiseMachineSetupDetailByID(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objSetWiseDetailByID = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objSetWiseDetailByID = (from SD in _ctxCmn.PrdDyingMachineSetupDetails
                                            //join SM in _ctxCmn.PrdDyingMachineSetups on SD.SetupID equals SM.SetupID
                                            join MP in _ctxCmn.PrdDyingMachineParts on SD.MachinePartID equals MP.MachinePartID
                                            where SD.CompanyID == objcmnParam.loggedCompany && SD.IsDeleted == false
                                            && SD.SetupID == objcmnParam.id
                                            //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                            orderby SD.SetupDetailID
                                            select new
                                            {
                                                SetupDetailID = SD.SetupDetailID,
                                                SetupID = SD.SetupID,
                                                MachinePartID = SD.MachinePartID,
                                                MachinePartName = MP.MachinePartName,
                                                MachineOperationID = SD.MachineOperationID,
                                                SQPress = SD.SQPress == null ? 0 : SD.SQPress,
                                                Temp = SD.Temp == null ? 0 : SD.Temp

                                            }).ToList().Select(x => new vmItemSetSetup
                                            {
                                                SetupDetailID = x.SetupDetailID,
                                                SetupID = x.SetupID,
                                                MachinePartID = (long)x.MachinePartID,
                                                MachinePartName = x.MachinePartName,
                                                MachineOperationID = (long)x.MachineOperationID,
                                                SQPress = (decimal)x.SQPress,
                                                Temp = (decimal)x.Temp
                                            }).ToList();

                    recordsTotal = objSetWiseDetailByID.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSetWiseDetailByID;
        }

        public string SaveUpdateSetWiseMachineSetupMasterDetail(vmItemSetSetup Master, List<vmItemSetSetup> Detail, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                GFactory_EF_PrdDyingMachineSetup = new PrdDyingMachineSetup_EF();
                GFactory_EF_PrdDyingMachineSetupDetail = new PrdDyingMachineSetupDetail_EF();
                long SetMasterId = 0, SetDetailId = 0, FirstDigit = 0, OtherDigits = 0; //string SetNo = "";

                var Masteritem = new PrdDyingMachineSetup();
                var SetDetail = new List<PrdDyingMachineSetupDetail>();

                vmItemSetSetup item = new vmItemSetSetup();
                vmItemSetSetup items = new vmItemSetSetup();
                //-------------------END----------------------

                if (Detail.Count() > 0)
                {
                    try
                    {
                        if (Master.SetupID == 0)
                        {
                            SetMasterId = Convert.ToInt16(GFactory_EF_PrdDyingMachineSetup.getMaxID("PrdDyingMachineSetup"));
                            SetDetailId = Convert.ToInt64(GFactory_EF_PrdDyingMachineSetupDetail.getMaxID("PrdDyingMachineSetupDetail"));
                            FirstDigit = Convert.ToInt64(SetDetailId.ToString().Substring(0, 1));
                            OtherDigits = Convert.ToInt64(SetDetailId.ToString().Substring(1, SetDetailId.ToString().Length - 1));

                            Masteritem = new PrdDyingMachineSetup
                            {
                                SetupID = SetMasterId,
                                Speed = Master.Speed == null ? 0 : (decimal)Master.Speed,
                                KGPreMin = Master.KGPreMin == null ? 0 : (decimal)Master.KGPreMin,
                                MechineID = (long)Master.MachineID,
                                Moiture = Master.Moiture == null ? 0 : (decimal)Master.Moiture,
                                ItemID = (long)Master.ItemID,

                                CompanyID = objcmnParam.loggedCompany,
                                CreateBy = objcmnParam.loggeduser,
                                CreateOn = DateTime.Now,
                                CreatePc =  HostService.GetIP(),
                                IsDeleted = false
                            };

                            for (int i = 0; i < Detail.Count(); i++)
                            {
                                item = Detail[i];

                                var Detailitem = new PrdDyingMachineSetupDetail
                                {
                                    SetupDetailID = Convert.ToInt64(FirstDigit + "" + OtherDigits),
                                    SetupID = SetMasterId,
                                    SQPress = item.SQPress == null ? 0 : item.SQPress,
                                    Temp = item.Temp == null ? 0 : item.Temp,
                                    MachineOperationID = (int)item.MachineOperationID,
                                    MachinePartID = (int)item.MachinePartID,

                                    CompanyID = objcmnParam.loggedCompany,
                                    CreateBy = objcmnParam.loggeduser,
                                    CreateOn = DateTime.Now,
                                    CreatePc =  HostService.GetIP(),
                                    IsDeleted = false
                                };
                                //***************************************END*******************************************
                                SetDetail.Add(Detailitem);
                                OtherDigits++;
                            }
                        }
                        else
                        {
                            var SetMasterAll = GFactory_EF_PrdDyingMachineSetup.GetAll().Where(x => x.SetupID == Master.SetupID);
                            var SetDetailAll = GFactory_EF_PrdDyingMachineSetupDetail.GetAll().Where(x => x.SetupID == Master.SetupID);

                            Masteritem = SetMasterAll.FirstOrDefault(x => x.SetupID == Master.SetupID);
                            Masteritem.Speed = (decimal)Master.Speed;
                            Masteritem.KGPreMin = (decimal)Master.KGPreMin;
                            Masteritem.MechineID = (long)Master.MachineID;
                            Masteritem.ItemID = (long)Master.ItemID;
                            Masteritem.Moiture = (decimal)Master.Moiture;

                            Masteritem.CompanyID = objcmnParam.loggedCompany;
                            Masteritem.UpdateBy = objcmnParam.loggeduser;
                            Masteritem.UpdateOn = DateTime.Now;
                            Masteritem.UpdatePc =  HostService.GetIP();
                            Masteritem.IsDeleted = false;

                            for (int i = 0; i < Detail.Count(); i++)
                            {
                                item = Detail[i];
                                foreach (PrdDyingMachineSetupDetail d in SetDetailAll.Where(d => d.SetupID == Master.SetupID && d.SetupDetailID == item.SetupDetailID))
                                {
                                    d.SQPress = item.SQPress;
                                    d.Temp = item.Temp;
                                    d.MachineOperationID = (int)item.MachineOperationID;
                                    d.MachinePartID = (int)item.MachinePartID;

                                    d.CompanyID = objcmnParam.loggedCompany;
                                    d.CreateBy = objcmnParam.loggeduser;
                                    d.CreateOn = DateTime.Now;
                                    d.CreatePc =  HostService.GetIP();
                                    d.IsDeleted = false;

                                    SetDetail.Add(d);
                                    break;
                                }
                            }
                        }

                        if (Master.SetupID > 0)
                        {
                            if (Masteritem != null)
                            {
                                GFactory_EF_PrdDyingMachineSetup.Update(Masteritem);
                                GFactory_EF_PrdDyingMachineSetup.Save();
                            }
                            if (SetDetail != null && SetDetail.Count != 0)
                            {
                                GFactory_EF_PrdDyingMachineSetupDetail.UpdateList(SetDetail.ToList());
                                GFactory_EF_PrdDyingMachineSetupDetail.Save();
                            }
                        }
                        else
                        {
                            if (Masteritem != null)
                            {
                                GFactory_EF_PrdDyingMachineSetup.Insert(Masteritem);
                                GFactory_EF_PrdDyingMachineSetup.Save();
                                GFactory_EF_PrdDyingMachineSetup.updateMaxID("PrdDyingMachineSetup", Convert.ToInt64(SetMasterId));
                            }
                            if (SetDetail != null && SetDetail.Count != 0)
                            {
                                GFactory_EF_PrdDyingMachineSetupDetail.InsertList(SetDetail.ToList());
                                GFactory_EF_PrdDyingMachineSetupDetail.Save();
                                GFactory_EF_PrdDyingMachineSetupDetail.updateMaxID("PrdDyingMachineSetupDetail", Convert.ToInt64(FirstDigit + "" + (OtherDigits - 1)));
                            }
                        }

                        transaction.Complete();
                        result = "1";
                    }
                    catch (Exception e)
                    {
                        result = "";
                        e.ToString();
                    }
                }
                else
                {
                    result = "";
                    throw new Exception();
                }
            }
            return result;
        }

        public string DelUpdateSetWiseMachineSetupMasterDetail(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                GFactory_EF_PrdDyingMachineSetup = new PrdDyingMachineSetup_EF();
                GFactory_EF_PrdDyingMachineSetupDetail = new PrdDyingMachineSetupDetail_EF();

                var Masteritem = new PrdDyingMachineSetup();
                var SetDetail = new List<PrdDyingMachineSetupDetail>();

                //For Update Master Detail
                var SetMasterAll = GFactory_EF_PrdDyingMachineSetup.GetAll().Where(x => x.SetupID == objcmnParam.id);
                var SetDetailAll = GFactory_EF_PrdDyingMachineSetupDetail.GetAll().Where(x => x.SetupID == objcmnParam.id);
                //-------------------END----------------------

                try
                {
                    using (_ctxCmn = new ERP_Entities())
                    {
                        Masteritem = SetMasterAll.First(x => x.SetupID == objcmnParam.id);
                        Masteritem.CompanyID = objcmnParam.loggedCompany;
                        Masteritem.DeleteBy = objcmnParam.loggeduser;
                        Masteritem.DeleteOn = DateTime.Now;
                        Masteritem.DeletePc =  HostService.GetIP();
                        Masteritem.IsDeleted = true;

                        foreach (PrdDyingMachineSetupDetail d in SetDetailAll.Where(d => d.SetupID == objcmnParam.id))
                        {
                            d.CompanyID = objcmnParam.loggedCompany;
                            d.DeleteBy = objcmnParam.loggeduser;
                            d.DeleteOn = DateTime.Now;
                            d.DeletePc =  HostService.GetIP();
                            d.IsDeleted = true;

                            SetDetail.Add(d);
                            //break;
                        }
                    }

                    if (Masteritem != null)
                    {
                        GFactory_EF_PrdDyingMachineSetup.Update(Masteritem);
                        GFactory_EF_PrdDyingMachineSetup.Save();
                    }
                    if (SetDetail != null && SetDetail.Count!=0)
                    {
                        GFactory_EF_PrdDyingMachineSetupDetail.UpdateList(SetDetail.ToList());
                        GFactory_EF_PrdDyingMachineSetupDetail.Save();
                    }

                    transaction.Complete();
                    result = "1";
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
