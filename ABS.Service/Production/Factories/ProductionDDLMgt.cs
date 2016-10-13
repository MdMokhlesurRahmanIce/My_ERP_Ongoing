using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Service.Production.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Factories
{
    public class ProductionDDLMgt : iProductionDDLMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<CmnUOM> GenericFactoryFor_Unit = null;
        private iGenericFactory_EF<CmnItemColor> GenericFactoryFor_Color = null;
        private iGenericFactory_EF<CmnItemMaster> GenericFactoryFor_Item = null;
        private iGenericFactory_EF<PrdDyingProcess> GenericFactoryFor_DyingProcess = null;
        private iGenericFactory_EF<PrdDyingOperation> GenericFactoryFor_PrdDyingOperation = null;
        private iGenericFactory_EF<PrdSetSetup> GenericFactoryFor_PrdSetSetup = null;
        private iGenericFactory_EF<PrdOutputUnit> GenericFactoryFor_OutputUnit = null;
        private iGenericFactory_EF<PrdBWSlist> GenericFactory_PrdBWSlist_EF = null;
        private iGenericFactory_EF<PrdWeavingLine> GenericFactory_PrdWeavingLine_EF = null;
        private iGenericFactory_EF<PrdWeavingMachinConfig> GenericFactory_PrdWeavingMachine_EF = null;
        private iGenericFactory_EF<CmnCombo> GenericFactory_EF_cmnCombo = null;
        private iGenericFactory_EF<PrdFinishingProcess> GenericFactory_PrdFinishingProcess_EF = null;
        private iGenericFactory_EF<PrdDyingMachinePart> GenericFactory_PrdDyingMachinePart_EF = null;
        private iGenericFactory_EF<PrdDyingMRRSet> GenericFactory_PrdDyingMRRSet_EF = null;
        public List<vmPrdSetSetup> GetDyeingSetAll(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            
            try
            {
                using (ERP_Entities _dbContext = new ERP_Entities())
                {
                    return (
                        from set in _dbContext.PrdSetSetups

                        join salPI in _dbContext.SalPIMasters on set.PIID equals salPI.PIID into salPIGroup
                        from spg in salPIGroup.DefaultIfEmpty()

                        join buyer in _dbContext.CmnUsers on set.BuyerID equals buyer.UserID into buyerGroup
                        from bg in buyerGroup.DefaultIfEmpty()

                        join supplier in _dbContext.CmnUsers on set.SupplierID equals supplier.UserID into supplierGroup
                        from sg in supplierGroup.DefaultIfEmpty()

                        where set.IsDeleted == false && set.IsBallComplete== true && set.IsDyeComplete==false

                        select new vmPrdSetSetup
                        {
                            SetID = set.SetID,
                            SetNo = set.SetNo,
                            ItemID = set.ItemID,
                            ItemName = set.CmnItemMaster.ItemName,
                            PIID = spg.PIID,
                            PINo = spg.PINO,
                            BuyerID = set.BuyerID ?? 0,
                            BuyerName = bg.UserFullName ?? "",
                            SupplierID = set.SupplierID,
                            SupplierName = sg.UserFullName ?? ""
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public List<vmProductionUOMDropDown> GetAllUnit(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Unit = new CmnUOM_EF();
            List<vmProductionUOMDropDown> unites = null;
            try
            {
                List<CmnUOM> Allunit = GenericFactoryFor_Unit.GetAll().ToList();
                unites = (from olt in Allunit
                          where olt.StatusID == 1 && olt.CompanyID == companyID && olt.IsDeleted == false
                          orderby olt.UOMID descending
                          select new vmProductionUOMDropDown
                          {
                              UOMID = olt.UOMID,
                              UOMName = olt.UOMName
                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return unites.OrderBy(x => x.UOMID).ToList();
        }

        public List<vmItemMaster> GetArticals(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            GenericFactoryFor_Item = new CmnItemMaster_EF();
            List<vmItemMaster> articals = null;
            try
            {
                // 1 is Finish Good
                List<CmnItemMaster> allArticals = GenericFactoryFor_Item.FindBy(x => x.ItemTypeID == 1 && x.IsDeleted == false).ToList();
                articals = (from olt in allArticals
                            orderby olt.UOMID descending
                            select new vmItemMaster
                            {
                                ItemID = olt.ItemID,
                                ArticalNo = olt.ArticleNo
                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return articals;
        }

        public List<vmItemMaster> GetAllArticalByItemType(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type)
        {
            GenericFactoryFor_Item = new CmnItemMaster_EF();
            List<vmItemMaster> articals = null;
            try
            {
                //Type is Finish Good
                List<CmnItemMaster> allArticals = GenericFactoryFor_Item.GetAll().Where(x => x.ItemTypeID == Type && x.IsDeleted == false && x.CompanyID == CompanyId).ToList();
                articals = (from olt in allArticals
                            orderby olt.ItemID descending
                            select new vmItemMaster
                            {
                                ItemID = olt.ItemID,
                                ArticalNo = olt.ArticleNo
                            }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return articals;
        }

        public List<vmItemMaster> GetMachineByTypeAndGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? ItemTypeID, int? ItemGroupID)
        {

            List<vmItemMaster> articals = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    articals = (from itemMaster in _ctxCmn.CmnItemMasters
                                where itemMaster.CompanyID == CompanyId && itemMaster.ItemGroupID == ItemGroupID && itemMaster.ItemTypeID == ItemTypeID && itemMaster.IsDeleted == false
                                select new vmItemMaster
                                {
                                    ItemID = itemMaster.ItemID,
                                    ItemName = itemMaster.ItemName
                                }).ToList();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return articals;


        }

        public List<vmLoom> GetLoom(int? LoginCompanyID)
        {

            List<vmLoom> looms = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    looms = (from masterWorkFlow in _ctxCmn.PrdWeavingMachinConfigs
                                 //join item in _ctxCmn.CmnItemMasters on masterWorkFlow.MachineID equals item.ItemID
                             where masterWorkFlow.IsBook == false && masterWorkFlow.CompanyID == LoginCompanyID
                             select new vmLoom
                             {
                                 MachineConfigID = masterWorkFlow.MachineConfigID,
                                 MachineName = masterWorkFlow.MachineConfigNo
                             }


                              ).ToList();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return looms;

        }

        public List<vmWeavingLine> GetLines(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            GenericFactory_PrdWeavingLine_EF = new PrdWeavingLine_EF();
            List<vmWeavingLine> _objVmWeavingLines = null;
            try
            {

                List<PrdWeavingLine> allLines = GenericFactory_PrdWeavingLine_EF.GetAll().Where(s => s.IsDeleted == false && s.CompanyID == CompanyId).ToList();
                _objVmWeavingLines = (from olt in allLines
                                      select new vmWeavingLine
                                      {
                                          LineID = olt.LineID,
                                          LineName = olt.LineName,

                                      }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objVmWeavingLines.OrderBy(x => x.LineID).ToList();
        }

        public List<WeavingMachine> GetWeavingMachines(int? pageNumber, int? pageSize, int? IsPaging, int? departmentId)
        {
            GenericFactory_PrdWeavingMachine_EF = new PrdWeavingMachinConfig_EF();
            List<WeavingMachine> WeavingMachines = null;
            try
            {

                List<PrdWeavingMachinConfig> allMachines = GenericFactory_PrdWeavingMachine_EF.GetAll().Where(s => s.IsDeleted == false && s.DepartmentID == departmentId).ToList();

                WeavingMachines = (from olt in allMachines
                                   select new WeavingMachine
                                   {
                                       MachineConfigID = olt.MachineConfigID,
                                       MachineConfigNo = olt.MachineConfigNo

                                   }).ToList();

            }
            catch
            {


            }
            return WeavingMachines;

        }

        public List<vmShiftName> getShiftes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            List<vmShiftName> ShiftNames = null; ;


            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    ShiftNames = (from B in _ctxCmn.HRMShifts
                                  where B.CompanyID == CompanyId && B.IsDeleted == false
                                  orderby B.ShiftName
                                  select new vmShiftName
                                  {
                                      ShiftID = B.ShiftID,
                                      ShiftName = B.ShiftName
                                  }).ToList();
                }
            }
            catch
            {

            }

            return ShiftNames;
        }

        public List<vmOperator> getOperators(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type)
        {
            List<vmOperator> OPerators = null; ;


            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    OPerators = (from B in _ctxCmn.CmnUsers
                                 where B.CompanyID == CompanyId && B.IsDeleted == false
                                 orderby B.UserID
                                 select new vmOperator
                                 {
                                     UserID = B.UserID,
                                     UserFullName = B.UserFullName
                                 }).ToList();
                }
            }
            catch
            {

            }

            return OPerators;
        }

        public List<vmSetDetail> GetSetNoByArticalNo(int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            GenericFactoryFor_PrdSetSetup = new PrdSetSetup_EF();
            List<vmSetDetail> setNos = null;
            try
            {

                List<PrdSetSetup> allsetNo = GenericFactoryFor_PrdSetSetup.FindBy(x => x.ItemID == ItemID && x.IsDeleted == false).ToList();
                setNos = (from olt in allsetNo
                          where olt.ItemID == ItemID && olt.IsDeleted == false
                          orderby olt.SetID descending
                          select new vmSetDetail
                          {
                              SetID = olt.SetID,
                              SetNo = olt.SetNo,

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return setNos.ToList();
        }

        public List<vmOutputUnit> GetCanNoByDeapartmentId(int? pageNumber, int? pageSize, int? IsPaging, int? DepartmentID)
        {
            GenericFactoryFor_OutputUnit = new PrdOutputUnit_EF();
            List<vmOutputUnit> canNos = null;
            try
            {

                List<PrdOutputUnit> allOutputUnits = GenericFactoryFor_OutputUnit.GetAll().ToList();
                canNos = (from olt in allOutputUnits
                          where olt.ProcessID == DepartmentID && olt.IsDeleted == false
                          orderby olt.OutputID descending
                          select new vmOutputUnit
                          {
                              OutputName = olt.OutputName,
                              OutputID = olt.OutputID

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return canNos.OrderBy(x => x.OutputID).ToList();
        }

        public List<vmProductionColorDropDown> GetAllColor(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Color = new CmnItemColor_EF();
            List<vmProductionColorDropDown> colors = null;
            try
            {
                List<CmnItemColor> AllColors = GenericFactoryFor_Color.GetAll().ToList();
                colors = (from olt in AllColors
                          where olt.CompanyID == companyID && olt.IsDeleted == false
                          orderby olt.ItemColorID descending
                          select new vmProductionColorDropDown
                          {
                              ItemColorID = olt.ItemColorID,
                              ColorName = olt.ColorName

                          }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return colors.OrderBy(x => x.ItemColorID).ToList();
        }

        public List<vmProductinItemDropDown> GetAllItem(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactoryFor_Item = new CmnItemMaster_EF();
            List<vmProductinItemDropDown> items = null;
            try
            {
                List<CmnItemMaster> itemList = GenericFactoryFor_Item.GetAll().ToList();
                items = (from olt in itemList
                         where olt.StatusID == 1 && olt.CompanyID == companyID && olt.IsDeleted == false
                         orderby olt.ItemID descending
                         select new vmProductinItemDropDown
                         {
                             ItemID = (int)olt.ItemID,
                             ArticleNo = olt.ArticleNo
                         }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ItemID).ToList();
        }

        public List<vmProductinItemDropDown> GetChemicalItemByGroupID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? GroupID)
        {
            GenericFactoryFor_Item = new CmnItemMaster_EF();
            List<vmProductinItemDropDown> items = null;
            try
            {
                List<CmnItemMaster> itemList = GenericFactoryFor_Item.GetAll().ToList();
                items = (from olt in itemList
                         where olt.CompanyID == companyID && olt.IsDeleted == false && olt.ItemGroupID == GroupID
                         orderby olt.ItemID descending
                         select new vmProductinItemDropDown
                         {
                             ItemID = (int)olt.ItemID,
                             ArticleNo = olt.ArticleNo
                         }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ItemID).ToList();
        }

        public List<vmProductinItemDropDown> GetItemMasterByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            List<vmProductinItemDropDown> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    if (ItemID == 1)//FinishGoods
                    {
                        items = (from olt in _ctxCmn.CmnItemMasters
                                 where olt.CompanyID == companyID && olt.IsDeleted == false && olt.ItemTypeID == ItemID
                                 select new vmProductinItemDropDown
                                 {
                                     ItemID = (int)olt.ItemID,
                                     ArticleNo = olt.ItemName,
                                     ConcatedProperty = olt.ArticleNo + "/" + olt.ItemName
                                 }).ToList();
                    }
                    else
                    {
                        items = (from olt in _ctxCmn.CmnItemMasters
                                 where olt.CompanyID == companyID && olt.IsDeleted == false && olt.ItemTypeID == ItemID
                                 select new vmProductinItemDropDown
                                 {
                                     ItemID = (int)olt.ItemID,
                                     ArticleNo = olt.ItemName,
                                     ConcatedProperty = olt.ArticleNo + "/" + olt.ItemName
                                 }).ToList();
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ItemID).ToList();
        }

        public List<vmProductionPrdSetSetupDDL> GetSetNoByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            List<vmProductionPrdSetSetupDDL> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    items = (from olt in _ctxCmn.PrdSetSetups
                             where olt.CompanyID == companyID && olt.IsDeleted == false && olt.ItemID == (ItemID== 0? (int)olt.ItemID : ItemID)
                             select new vmProductionPrdSetSetupDDL
                             {
                                 id = olt.SetID,
                                 label = olt.SetNo,
                             }).ToList();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.id).ToList();
        }

        public List<vmItemMaster> GetDyingItemMachineByItemTypeGroup(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? ItemGroupID)
        {
            List<vmItemMaster> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    items = (from olt in _ctxCmn.CmnItemMasters
                             join machine in _ctxCmn.PrdDyingMachineSetups on olt.ItemID equals machine.ItemID into machineGroup
                             from mg in machineGroup.DefaultIfEmpty()
                             where olt.CompanyID == companyID && olt.IsDeleted == false && olt.ItemTypeID == ItemTypeID && olt.ItemGroupID == ItemGroupID
                             && mg.IsDeleted == false
                             select new vmItemMaster
                             {
                                 ItemID = olt.ItemID,
                                 ItemTypeID = olt.ItemTypeID,
                                 ItemGroupID = olt.ItemGroupID,
                                 ItemName = olt.ItemName,
                                 ArticalNo = olt.ArticleNo,
                                 Speed = (decimal?)mg.Speed,
                                 Moiture = (decimal?)mg.Moiture,
                                 KGPreMin = (decimal?)mg.KGPreMin,
                             }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ItemID).ToList();
        }


        public List<vmPrdDyingMachineSetup> GetPrdDyingMachineSetupByItemMachine(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID, int? MechineID)
        {
            List<vmPrdDyingMachineSetup> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    items = (from olt in _ctxCmn.PrdDyingMachineSetups
                             where olt.CompanyID == companyID && olt.IsDeleted == false
                             && olt.ItemID == ((ItemID == 0) ? olt.ItemID : (long)ItemID)
                             && olt.MechineID == ((MechineID == 0) ? olt.MechineID : (long)MechineID)
                             select new vmPrdDyingMachineSetup
                             {
                                 ItemID = olt.ItemID ?? 0,
                                 SetupID = olt.SetupID,
                                 Speed = (decimal)olt.Speed,
                                 Moiture = (decimal)olt.Moiture,
                                 KGPreMin = (decimal)olt.KGPreMin,
                             }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ItemID).ToList();
        }
        public List<vmProductionDyingHRMShiftDropDown> GetShift(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmProductionDyingHRMShiftDropDown> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    items = (from olt in _ctxCmn.HRMShifts
                             where olt.CompanyID == companyID && olt.IsDeleted == false
                             select new vmProductionDyingHRMShiftDropDown
                             {
                                 ShiftID = olt.ShiftID,
                                 ShiftName = olt.ShiftName
                             }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ShiftID).ToList();
        }

        public List<vmProductionDyingSetNoDropDown> GetDyingSetNoByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            List<vmProductionDyingSetNoDropDown> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    items = (from master in _ctxCmn.PrdDyingMRRSets
                             join details in _ctxCmn.PrdDyingMRRSetDetails on master.DyingSetID equals details.DyingSetID into leftSetDetailsGroup
                             from lsdg in leftSetDetailsGroup.Take(1).DefaultIfEmpty()
                             join set in _ctxCmn.PrdSetSetups on lsdg.SetID equals set.SetID into leftSetGroup
                             from lsg in leftSetGroup.DefaultIfEmpty()
                             join buyer in _ctxCmn.CmnUsers on lsg.BuyerID equals buyer.UserID into leftBuyerGroup
                             from lbg in leftBuyerGroup.DefaultIfEmpty()
                             join supplier in _ctxCmn.CmnUsers on lsg.SupplierID equals supplier.UserID into leftSuplierGroup
                             from lsupg in leftSuplierGroup.DefaultIfEmpty()
                             join item in _ctxCmn.CmnItemMasters on master.ItemID equals item.ItemID into leftItemGroup
                             from lig in leftItemGroup.DefaultIfEmpty()
                             where (master.ItemID == ((ItemID == 0) ? master.ItemID : (int)ItemID)) && master.CompanyID == companyID && master.IsDeleted == false
                             select new vmProductionDyingSetNoDropDown
                             {
                                 DyingSetID = master.DyingSetID
                                 ,
                                 DyingSetIDDetailID = (int?)lsdg.DyingSetIDDetailID ?? 0
                                 ,
                                 PIID = (int?)lsdg.PIID ?? 0
                                  ,
                                 SetID = (int?)lsdg.SetID ?? 0
                                  ,
                                 ItemID = (int?)master.ItemID ?? 0
                                 ,
                                 ArticleNo = lig.ArticleNo
                                  ,
                                 DyingPINo = master.DyingPINo
                                  ,
                                 DyingSetNo = master.DyingSetNo
                                  ,
                                 BuyerName = lbg.UserFullName
                                 ,
                                 SupplierName = lsupg.UserFullName
                             }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.DyingSetID).ToList();
        }

        public List<vmProductinDyingProcessDropDown> GetDyingProcessByProcessID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ProcessID)
        {
            GenericFactoryFor_DyingProcess = new PrdDyingProcess_EF();
            List<vmProductinDyingProcessDropDown> items = null;
            try
            {
                List<PrdDyingProcess> itemList = GenericFactoryFor_DyingProcess.GetAll().ToList();
                items = (from olt in itemList
                         where olt.CompanyID == companyID && olt.IsDeleted == false && olt.ProcessID == ProcessID
                         orderby olt.DyingProcessID descending
                         select new vmProductinDyingProcessDropDown
                         {
                             DyingProcessID = (int)olt.DyingProcessID,
                             ProcessName = olt.ProcessName
                         }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.DyingProcessID).ToList();
        }


        public IEnumerable<vmPrdDyingOperationDropDown> GetDyingOperationByProcessID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ProcessID)
        {
            GenericFactoryFor_PrdDyingOperation = new PrdDyingOperation_EF();
            List<PrdDyingOperation> items = null;
            List<vmPrdDyingOperationDropDown> list = new List<vmPrdDyingOperationDropDown>();

            try
            {
                if (ProcessID == 0)
                {
                    items = GenericFactoryFor_PrdDyingOperation.FindBy(x => x.IsDeleted == false).ToList();
                }
                else
                {
                    items = GenericFactoryFor_PrdDyingOperation.FindBy(x => x.DyingProcessID == ProcessID && x.IsDeleted == false).ToList();
                }

                foreach (PrdDyingOperation item in items)
                {
                    vmPrdDyingOperationDropDown ent = new vmPrdDyingOperationDropDown();
                    ent.DyingProcessID = item.DyingProcessID;
                    ent.OperationName = item.OperationName;
                    ent.OperationID = item.OperationID;
                    list.Add(ent);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list.OrderByDescending(x => x.DyingProcessID).ThenByDescending(x => x.OperationID).ToList();
        }

        public IEnumerable<vmItemSetSetup> GetArticle(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objArticle = null;
            IEnumerable<CmnItemMaster> objItem = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objItem = _ctxCmn.CmnItemMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false
                                  && objcmnParam.ItemType == 0 ? true : x.ItemTypeID == objcmnParam.ItemType).ToList();
                    objItem = objItem.Where(x => objcmnParam.ItemGroup == 0 ? true : x.ItemGroupID == objcmnParam.ItemGroup).ToList();

                    objArticle = (from IM in objItem.Where(x => objcmnParam.id == 0 ? true : x.ItemID == objcmnParam.id)
                                  orderby IM.ItemID
                                  select new
                                  {
                                      ItemID = IM.ItemID,
                                      ArticleNo = IM.ArticleNo,
                                      ItemName = IM.ItemName
                                  }).ToList().Select(x => new vmItemSetSetup
                                  {
                                      ItemID = x.ItemID,
                                      ArticleNo = x.ArticleNo,
                                      ItemName = x.ItemName
                                  }).ToList();

                    recordsTotal = objArticle.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objArticle;
        }

        public IEnumerable<vmItemSetSetup> GetItemAsMachine(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objitemMachine = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objitemMachine = (from IM in _ctxCmn.CmnItemMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false)
                                      join IG in _ctxCmn.CmnItemGroups.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false)
                                      on IM.ItemGroupID equals IG.ItemGroupID
                                      where IG.ParentID == objcmnParam.ItemGroup
                                      orderby IM.ItemID descending
                                      select new
                                      {
                                          ItemID = IM.ItemID,
                                          ArticleNo = IM.ArticleNo + ", " + IM.ItemName,
                                          ItemName = IM.ArticleNo + ", " + IM.ItemName

                                      }).ToList().Select(x => new vmItemSetSetup
                                      {
                                          ItemID = x.ItemID,
                                          ArticleNo = x.ArticleNo,
                                          ItemName = x.ItemName
                                      }).ToList();

                    recordsTotal = objitemMachine.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objitemMachine;
        }

        public List<vmItemDetais> GetShift(vmCmnParameters objcmnParam)
        {
            List<vmItemDetais> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    items = (from olt in _ctxCmn.HRMShifts
                             where olt.CompanyID == objcmnParam.loggedCompany && olt.IsDeleted == false
                             select new vmItemDetais
                             {
                                 ItemId = olt.ShiftID,
                                 ItemName = olt.ShiftName
                             }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ItemName).ToList();
        }

        public IEnumerable<vmItemDetais> GetItems(vmItemDetais objcmnParam)
        {
            IEnumerable<vmItemDetais> itemDetails = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    itemDetails = (from IM in _ctxCmn.CmnItemMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.ItemTypeID == 1 && x.IsDeleted == false
                                  )
                                   orderby IM.ItemID descending
                                   select new
                                   {
                                       ItemID = IM.ItemID,
                                       ItemName = IM.ItemName
                                   }).ToList().Select(x => new vmItemDetais
                                   {
                                       ItemId = (int)x.ItemID,
                                       ItemName = x.ItemName
                                   }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemDetails;
        }

        public IEnumerable<vmItemDetais> GetSets(vmItemDetais objcmnParam)
        {
            IEnumerable<vmItemDetais> itemDetails = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    itemDetails = (from IM in _ctxCmn.PrdSetSetups.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.ItemID == objcmnParam.ItemId && x.IsDeleted == false
                                  )
                                   orderby IM.ItemID descending
                                   select new
                                   {
                                       ItemID = IM.SetID,
                                       ItemName = IM.SetNo
                                   }).ToList().Select(x => new vmItemDetais
                                   {
                                       ItemId = (int)x.ItemID,
                                       ItemName = x.ItemName
                                   }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemDetails;
        }

        public IEnumerable<vmItemDetais> GetMachines(vmItemDetais objcmnParam)
        {
            IEnumerable<vmItemDetais> itemDetails = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    itemDetails = (from IM in _ctxCmn.CmnItemMasters.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.ItemTypeID == objcmnParam.ItemType && x.ItemGroupID == objcmnParam.ItemGroup && x.IsDeleted == false
                                  )
                                   orderby IM.ItemID descending
                                   select new
                                   {
                                       ItemID = IM.ItemID,
                                       ItemName = IM.ItemName
                                   }).ToList().Select(x => new vmItemDetais
                                   {
                                       ItemId = (int)x.ItemID,
                                       ItemName = x.ItemName
                                   }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemDetails;
        }

        public IEnumerable<vmItemSetSetup> GetMachine(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objItem = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objItem = (from IM in _ctxCmn.CmnItemMasters
                               where IM.CompanyID == objcmnParam.loggedCompany && IM.IsDeleted == false
                               && IM.ItemTypeID == objcmnParam.ItemType && objcmnParam.ItemGroup == 0 ? true : IM.ItemGroupID == objcmnParam.ItemGroup
                               //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                               orderby IM.ItemID descending
                               select new
                               {
                                   MachineID = IM.ItemID,
                                   MachineName = IM.ItemName
                               }).ToList().Select(x => new vmItemSetSetup
                               {
                                   MachineID = x.MachineID,
                                   MachineName = x.MachineName
                               }).ToList();

                    recordsTotal = objItem.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItem;
        }

        public IEnumerable<vmItemSetSetup> GetDetailsMachine(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmItemSetSetup> objDMachine = null;
            IEnumerable<PrdWeavingMachinConfig> objWMC = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objWMC = _ctxCmn.PrdWeavingMachinConfigs.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false && x.DepartmentID == objcmnParam.DepartmentID && x.IsBook == false).ToList();
                    objDMachine = (from IM in objWMC
                                   where objcmnParam.IsTrue == true ? true : IM.IsMaintenance == objcmnParam.IsTrue
                                   orderby IM.MachineConfigID
                                   select new
                                   {
                                       MachineID = IM.MachineConfigID,
                                       MachineName = IM.MachineConfigNo
                                   }).ToList().Select(x => new vmItemSetSetup
                                   {
                                       MachineID = x.MachineID,
                                       MachineName = x.MachineName
                                   }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDMachine;
        }

        public IEnumerable<vmItemSetSetup> GetDetailOperation(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmItemSetSetup> objOperation = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objOperation = (from MP in _ctxCmn.PrdDyingMachineOperations
                                    where MP.CompanyID == objcmnParam.loggedCompany && MP.IsDeleted == false
                                    //&& MP.MachineID == objcmnParam.id
                                    //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                    orderby MP.OperationName descending
                                    select new
                                    {
                                        MachineOperationID = MP.MachineOperationID,
                                        OperationName = MP.OperationName
                                    }).ToList().Select(x => new vmItemSetSetup
                                    {
                                        MachineOperationID = x.MachineOperationID,
                                        OperationName = x.OperationName
                                    }).ToList();

                    recordsTotal = objOperation.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objOperation;
        }

        public IEnumerable<vmSetDetail> GetSetNo(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmSetDetail> objAllSetNo = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objAllSetNo = (from SD in _ctxCmn.PrdSetSetups
                                   where SD.CompanyID == objcmnParam.loggedCompany && SD.IsDeleted == false
                                   //&& SD.ItemID == objcmnParam.id
                                   //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                   orderby SD.SetID
                                   select new
                                   {
                                       SetID = SD.SetID,
                                       SetNo = SD.SetNo
                                   }).ToList().Select(x => new vmSetDetail
                                   {
                                       SetID = x.SetID,
                                       SetNo = x.SetNo
                                   }).ToList();

                    recordsTotal = objAllSetNo.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objAllSetNo;
        }

        public IEnumerable<vmSetDetail> GetSetByArticalNo(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmSetDetail> objSetNo = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objSetNo = (from SD in _ctxCmn.PrdSetSetups
                                where SD.CompanyID == objcmnParam.loggedCompany && SD.IsDeleted == false
                                    && SD.ItemID == objcmnParam.id
                                //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                orderby SD.SetID descending
                                select new
                                {
                                    SetID = SD.SetID,
                                    SetNo = SD.SetNo
                                }).ToList().Select(x => new vmSetDetail
                                {
                                    SetID = x.SetID,
                                    SetNo = x.SetNo
                                }).ToList();

                    recordsTotal = objSetNo.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objSetNo;
        }

        public vmProductionUOMDropDown GetUnitSingle(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            vmProductionUOMDropDown objUnit = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objUnit = (from UM in _ctxCmn.CmnUOMs
                               join IM in _ctxCmn.CmnItemMasters on UM.UOMID equals IM.UOMID
                               where UM.CompanyID == objcmnParam.loggedCompany && UM.IsDeleted == false
                               //&& MP.ItemID == objcmnParam.id
                               && IM.ItemID == objcmnParam.id
                               //orderby UM.UOMID descending
                               select new
                               {
                                   UOMID = UM.UOMID,
                                   UOMName = UM.UOMName
                               }).Select(x => new vmProductionUOMDropDown
                               {
                                   UOMID = x.UOMID,
                                   UOMName = x.UOMName
                               }).FirstOrDefault();

                    //recordsTotal = objUnit.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUnit;
        }

        public IEnumerable<vmProductionUOMDropDown> GetUnit(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            IEnumerable<vmProductionUOMDropDown> objUnits = null;
            recordsTotal = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objUnits = (from UM in _ctxCmn.CmnUOMs
                                where UM.CompanyID == objcmnParam.loggedCompany && UM.IsDeleted == false
                                //&& MP.ItemID == objcmnParam.id
                                //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                orderby UM.UOMID descending
                                select new
                                {
                                    UOMID = UM.UOMID,
                                    UOMName = UM.UOMName
                                }).Select(x => new vmProductionUOMDropDown
                                {
                                    UOMID = x.UOMID,
                                    UOMName = x.UOMName
                                }).ToList();

                    recordsTotal = objUnits.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUnits;
        }

        public IEnumerable<vmBallInfo> GetBeams(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBallInfo> objBeam = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objBeam = (from L in _ctxCmn.PrdOutputUnits
                               where L.CompanyID == objcmnParam.loggedCompany && L.IsDeleted == false
                               && L.ProcessID == objcmnParam.DepartmentID
                               orderby L.OutputNo
                               select new vmBallInfo
                               {
                                   OutputID = L.OutputID,
                                   OutputNo = L.OutputNo
                               }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBeam;
        }

        public IEnumerable<vmShiftName> GetShifts(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmShiftName> ShiftNames = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    ShiftNames = (from B in _ctxCmn.HRMShifts
                                  where B.CompanyID == objcmnParam.loggedCompany && B.IsDeleted == false
                                  orderby B.ShiftName
                                  select new vmShiftName
                                  {
                                      ShiftID = B.ShiftID,
                                      ShiftName = B.ShiftName
                                  }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ShiftNames;
        }

        public IEnumerable<vmOperator> GetOperators(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmOperator> Operators = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Operators = (from B in _ctxCmn.CmnUsers
                                 where B.CompanyID == objcmnParam.loggedCompany && B.IsDeleted == false
                                 && B.UserTypeID == objcmnParam.ItemType //1
                                 orderby B.UserFullName
                                 select new vmOperator
                                 {
                                     UserID = B.UserID,
                                     UserFullName = B.UserFullName
                                 }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Operators;
        }

        public IEnumerable<vmBeamQuality> GetBeamQuality(vmCmnParameters objcmnParam)
        {
            IEnumerable<vmBeamQuality> BeamQuality = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    BeamQuality = (from B in _ctxCmn.PrdSizingBeamQualities
                                   where B.CompanyID == objcmnParam.loggedCompany && B.IsDeleted == false
                                   //&& B.UserTypeID == objcmnParam.tTypeId //1
                                   orderby B.BeamQualityName
                                   select new vmBeamQuality
                                   {
                                       BeamQualityID = B.BeamQualityID,
                                       BeamQualityName = B.BeamQualityName
                                   }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return BeamQuality;
        }

        public IEnumerable<PrdBWSlist> GetLoadMachineStopCauses(vmCmnParameters objcmnParam)
        {
            GenericFactory_PrdBWSlist_EF = new PrdBWSlist_EF();
            IEnumerable<PrdBWSlist> MachineStopCauses = null;
            try
            {
                MachineStopCauses = GenericFactory_PrdBWSlist_EF.GetAll().Select(m => new PrdBWSlist { BWSID = m.BWSID, BWSName = m.BWSName, BWSType = m.BWSType, CompanyID = m.CompanyID, IsDeleted = m.IsDeleted })
                 .Where(m => m.CompanyID == objcmnParam.loggedCompany && m.BWSType == objcmnParam.ItemType && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return MachineStopCauses;
        }

        public IEnumerable<PrdBWSlist> GetLoadMachineBrekages(vmCmnParameters objcmnParam)
        {
            GenericFactory_PrdBWSlist_EF = new PrdBWSlist_EF();
            IEnumerable<PrdBWSlist> MachineBrekages = null;
            try
            {
                MachineBrekages = GenericFactory_PrdBWSlist_EF.GetAll().Select(m => new PrdBWSlist { BWSID = m.BWSID, BWSName = m.BWSName, BWSType = m.BWSType, CompanyID = m.CompanyID, IsDeleted = m.IsDeleted })
                 .Where(m => m.CompanyID == objcmnParam.loggedCompany && m.BWSType == objcmnParam.ItemType && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return MachineBrekages;
        }

        public IEnumerable<vmWeavingLine> GetWeavingLine(vmCmnParameters objcmnParam)
        {
            //GenericFactory_PrdWeavingLine_EF = new PrdWeavingLine_EF();
            IEnumerable<vmWeavingLine> objVmWeavingLines = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    //List<PrdWeavingLine> allLines = GenericFactory_PrdWeavingLine_EF.GetAll().Where(s => s.IsDeleted == false && s.CompanyID == objcmnParam.loggedCompany).ToList();
                    objVmWeavingLines = (from ln in _ctxCmn.PrdWeavingLines.Where(x => objcmnParam.ItemType == 0 ? true : x.DepartmentID == objcmnParam.ItemType)
                                         where ln.IsDeleted == false && ln.CompanyID == objcmnParam.loggedCompany
                                         orderby ln.LineName
                                         select new vmWeavingLine
                                         {
                                             LineID = ln.LineID,
                                             LineName = ln.LineName,

                                         }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objVmWeavingLines;
        }

        public IEnumerable<Models.ViewModel.Production.vmDepartment> GetDepartmentByCompayUserID(int? companyID, int? loggedUser)
        {
            IEnumerable<Models.ViewModel.Production.vmDepartment> objDepartment = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objDepartment = (from con in _ctxCmn.CmnUserJobContracts.Where(x => x.CompanyID == companyID && x.UserID == loggedUser)
                                     join org in _ctxCmn.CmnOrganograms on con.DepartmentID equals org.OrganogramID into departmentGroup
                                     from dg in departmentGroup.DefaultIfEmpty()
                                     where dg.IsDepartment == true
                                     select new Models.ViewModel.Production.vmDepartment
                                     {
                                         DepartmentID = dg.OrganogramID,
                                         DepartmentName = dg.OrganogramName
                                     }).ToList();

                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDepartment;
        }

        public List<vmStyle> GetStyleNoes(vmCmnParameters objcmnParam)
        {
            List<vmStyle> styles = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    styles = (from FM in _ctxCmn.PrdFinishingMRRMasters
                              where FM.CompanyID == objcmnParam.loggedCompany && FM.IsDeleted == false
                              && objcmnParam.id == 0 ? true : FM.ItemID == objcmnParam.id
                              select new vmStyle
                              {
                                  FinishingMRRID = FM.FinishingMRRID,
                                  FinishingMRRNo = FM.FinishingMRRNo
                              }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return styles;
        }

        public List<vmPI> GetAllPI(vmCmnParameters objcmnParam)
        {
            List<vmPI> objPI = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objPI = (from PI in _ctxCmn.SalPIMasters
                             where PI.CompanyID == objcmnParam.loggedCompany && PI.IsDeleted == false && PI.IsHDOCompleted == objcmnParam.IsTrue
                             select new vmPI
                             {
                                 PIID = PI.PIID,
                                 PINO = PI.PINO
                             }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPI;
        }

        public List<vmDefectPoint> GetDefectPoints(vmCmnParameters objcmnParam)
        {
            List<vmDefectPoint> defectPoints = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    defectPoints = (from aitem in _ctxCmn.PrdFinishingDefectPoints
                                    where aitem.CompanyID == objcmnParam.loggedCompany && aitem.IsDeleted == false
                                    select new vmDefectPoint
                                    {
                                        DefectName = aitem.DefectName,
                                        DefectPointID = aitem.DefectPointID
                                    }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return defectPoints;
        }

        public List<vmMachineNo> GetMachine(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type)
        {
            List<vmMachineNo> machines = null;


            try
            {

                using (_ctxCmn = new ERP_Entities())
                {

                    machines = (from aitem in _ctxCmn.PrdWeavingMachinConfigs
                                where aitem.CompanyID == CompanyId && aitem.IsDeleted == false
                                select new vmMachineNo
                                {
                                    ItemID = aitem.MachineConfigID,
                                    ItemName = aitem.MachineConfigNo
                                }).ToList();

                }
            }
            catch
            {


            }
            return machines;

        }

        public List<vmPlate> GetPlates(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type)
        {
            List<vmPlate> pates = null;


            try
            {

                using (_ctxCmn = new ERP_Entities())
                {

                    pates = (from aitem in _ctxCmn.CmnOrganograms
                             where aitem.CompanyID == CompanyId && aitem.IsDeleted == false && aitem.ParentID == Type
                             select new vmPlate
                             {
                                 OrganogramID = aitem.OrganogramID,
                                 OrganogramName = aitem.OrganogramName
                             }).ToList();

                }
            }
            catch
            {


            }
            return pates;

        }

        public List<vmPrdDyingMachinePart> GetMachinePartByMachineID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? MachineID)
        {
            List<vmPrdDyingMachinePart> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    items = (from master in _ctxCmn.PrdDyingMachineParts
                             where master.MachineID == (MachineID == 0 ? master.MachineID : (int)MachineID)
                             && master.CompanyID == companyID
                             && master.IsDeleted == false
                             select new vmPrdDyingMachinePart
                             {
                                 MachinePartID = master.MachinePartID
                                 ,
                                 MachinePartNo = master.MachinePartNo
                                 ,
                                 MachinePartName = master.MachinePartName
                             }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.MachinePartID).ToList();
        }

        public List<vmDyingChemicalConsumptionOperation> GetOperation(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmDyingChemicalConsumptionOperation> items = new List<vmDyingChemicalConsumptionOperation>();
            try
            {
                items.Add(new vmDyingChemicalConsumptionOperation { OperationID = 1, OperationName = "Box" });
                items.Add(new vmDyingChemicalConsumptionOperation { OperationID = 2, OperationName = "Feed" });
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.OperationID).ToList();
        }

        public IEnumerable<PrdWeavingMRRMaster> GetWeavingSetNo(vmCmnParameters objcmnParam)
        {
            IEnumerable<PrdWeavingMRRMaster> objAllWSetNo = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objAllWSetNo = (from WMM in _ctxCmn.PrdWeavingMRRMasters
                                    where WMM.CompanyID == objcmnParam.loggedCompany && WMM.IsDeleted == false
                                    //&& SD.ItemID == objcmnParam.id
                                    //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                    orderby WMM.WeavingMRRNo
                                    select new
                                    {
                                        WeavingMRRID = WMM.WeavingMRRID,
                                        WeavingMRRNo = WMM.WeavingMRRNo
                                    }).ToList().Select(x => new PrdWeavingMRRMaster
                                    {
                                        WeavingMRRID = x.WeavingMRRID,
                                        WeavingMRRNo = x.WeavingMRRNo
                                    }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objAllWSetNo;
        }

        public IEnumerable<PrdFinishingType> GetFinishingType(vmCmnParameters objcmnParam)
        {
            IEnumerable<PrdFinishingType> objAllFinishingType = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objAllFinishingType = (from WMM in _ctxCmn.PrdFinishingTypes
                                           where WMM.CompanyID == objcmnParam.loggedCompany && WMM.IsDeleted == false
                                           //&& SD.ItemID == objcmnParam.id
                                           //&& objcmnParam.id == 0 ? true : IM.ItemID == objcmnParam.id
                                           orderby WMM.FInishTypeName
                                           select new
                                           {
                                               FInishTypeID = WMM.FInishTypeID,
                                               FInishTypeName = WMM.FInishTypeName
                                           }).ToList().Select(x => new PrdFinishingType
                                           {
                                               FInishTypeID = x.FInishTypeID,
                                               FInishTypeName = x.FInishTypeName
                                           }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objAllFinishingType;
        }

        public IEnumerable<CmnOrganogram> GetDepartment(vmCmnParameters objcmnParam)
        {
            IEnumerable<CmnOrganogram> objAllDepartment = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objAllDepartment = (from CO in _ctxCmn.CmnOrganograms
                                        where CO.CompanyID == objcmnParam.loggedCompany && CO.IsDeleted == false && CO.IsDepartment == true
                                        && objcmnParam.id == 0 ? true : CO.ParentID == objcmnParam.id
                                        orderby CO.OrganogramName
                                        select new
                                        {
                                            OrganogramID = CO.OrganogramID,
                                            OrganogramName = CO.OrganogramName
                                        }).ToList().Select(x => new CmnOrganogram
                                        {
                                            OrganogramID = x.OrganogramID,
                                            OrganogramName = x.OrganogramName
                                        }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objAllDepartment;
        }

        public CmnOrganogram GetDepartmentByID(vmCmnParameters objcmnParam)
        {
            CmnOrganogram objDepartmentSingle = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objDepartmentSingle = (from CO in _ctxCmn.CmnOrganograms
                                           where CO.CompanyID == objcmnParam.loggedCompany && CO.IsDeleted == false && CO.IsDepartment == true && CO.OrganogramID == objcmnParam.id
                                           //&& SD.ItemID == objcmnParam.id
                                           //&& objcmnParam.id == 0 ? true : CO.ItemID == objcmnParam.id
                                           //orderby CO.OrganogramName
                                           select new
                                           {
                                               OrganogramID = CO.OrganogramID,
                                               OrganogramName = CO.OrganogramName
                                           }).ToList().Select(x => new CmnOrganogram
                                           {
                                               OrganogramID = x.OrganogramID,
                                               OrganogramName = x.OrganogramName
                                           }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDepartmentSingle;
        }

        public IEnumerable<CmnCombo> GetBWSType(vmCmnParameters objcmnParam)
        {
            GenericFactory_EF_cmnCombo = new CmnCombo_EF();
            IEnumerable<CmnCombo> objBWSType = null;
            try
            {
                objBWSType = GenericFactory_EF_cmnCombo.GetAll()
                    .Select(m => new CmnCombo
                    {
                        ComboID = m.ComboID,
                        ComboName = m.ComboName,
                        ComboType = m.ComboType,
                        IsDefault = m.IsDefault,
                        IsDeleted = m.IsDeleted
                    })
                    .Where(m => m.ComboType == objcmnParam.ParamName && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBWSType;
        }

        public IEnumerable<PrdFinishingProcess> GetFinishingProcessType(vmCmnParameters objcmnParam)
        {
            //GenericFactory_PrdFinishingProcess_EF = new PrdFinishingProcess_EF();
            IEnumerable<PrdFinishingProcess> objFiniProcType = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {

                    objFiniProcType = (from CO in _ctxCmn.PrdFinishingProcesses
                                       where CO.CompanyID == objcmnParam.loggedCompany && CO.IsDeleted == false && CO.IsActive == true
                                       orderby CO.FinishingProcessID
                                       select new
                                       {
                                           FinishingProcessID = CO.FinishingProcessID,
                                           FinishingProcessName = CO.FinishingProcessName
                                       }).ToList().Select(x => new PrdFinishingProcess
                                       {
                                           FinishingProcessID = x.FinishingProcessID,
                                           FinishingProcessName = x.FinishingProcessName
                                       }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFiniProcType;
        }

        public IEnumerable<vmGrade> GetAllGrade(vmCmnParameters objcmnParam)
        {
            //GenericFactory_PrdFinishingProcess_EF = new PrdFinishingProcess_EF();
            IEnumerable<vmGrade> objGrade = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objGrade = (from CIG in _ctxCmn.CmnItemGrades
                                where CIG.CompanyID == objcmnParam.loggedCompany && CIG.IsDeleted == false
                                orderby CIG.GradeName
                                select new
                                {
                                    ItemGradeID = CIG.ItemGradeID,
                                    GradeName = CIG.GradeName
                                }).ToList().Select(x => new vmGrade
                                {
                                    ItemGradeID = x.ItemGradeID,
                                    GradeName = x.GradeName
                                }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objGrade;
        }

        //This Method also have filter with itemGroup picked up by itemType

        public List<vmItemMaster> GetItmeByItemType(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID)
        {
            List<vmItemMaster> items = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    var ItemGroupID = 0;
                    var ItemGroup = _ctxCmn.CmnItemGroups.Where(x => x.ItemTypeID == ItemTypeID).FirstOrDefault();
                    if (ItemGroup != null)
                        ItemGroupID = ItemGroup.ItemGroupID;
                    items = (from olt in _ctxCmn.CmnItemMasters
                             join CUOM in _ctxCmn.CmnUOMs on olt.UOMID equals CUOM.UOMID
                             where
                             olt.ItemTypeID == (ItemTypeID == 0 ? olt.ItemTypeID : (int)ItemTypeID)
                             && olt.ItemGroupID == (ItemGroupID == 0 ? olt.ItemGroupID : (int)ItemGroupID)
                             && olt.CompanyID == companyID && olt.IsDeleted == false
                             select new vmItemMaster
                             {
                                 ItemID = olt.ItemID,
                                 ItemTypeID = olt.ItemTypeID,
                                 ItemGroupID = olt.ItemGroupID,
                                 ItemName = olt.ItemName,
                                 ArticalNo = olt.ArticleNo,                                 
                                 UnitID=(long)olt.UOMID,
                                 UOMName=CUOM.UOMName,
                                 Batch = (from CB in _ctxCmn.CmnBatches
                                          join IM in _ctxCmn.InvStockMasters on CB.BatchID equals IM.BatchID
                                          where CB.ItemID == olt.ItemID
                                          //where B.ItemTypeID != null
                                          select new vmCmnBatch { BatchID = CB.BatchID, BatchNo = CB.BatchNo }).ToList(),
                                 Supplier = (from CU in _ctxCmn.CmnUsers
                                             join IM in _ctxCmn.InvStockMasters on CU.UserID equals IM.SupplierID
                                             where IM.ItemID == olt.ItemID
                                             //where B.ItemTypeID != null
                                             select new vmBallInfo { SupplierID = IM.SupplierID, SupplierName = CU.UserFullName }).ToList()

                             }).ToList();

                    items = items.Select((itema, indexa) => new vmItemMaster
                    {
                        ItemID = itema.ItemID,
                        ItemTypeID = itema.ItemTypeID,
                        ItemGroupID = itema.ItemGroupID,
                        ItemName = itema.ItemName,
                        ArticalNo = itema.ArticalNo,
                        UnitID=itema.UnitID,
                        UOMName=itema.UOMName,
                        Batch=itema.Batch,
                        Supplier=itema.Supplier,
                        RowNum = indexa + 1
                    }).ToList();

                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return items.OrderBy(x => x.ItemID).ToList();//.Take(3).ToList();
        }

        public List<vmCmnBatch> GetBatchByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            List<vmCmnBatch> listBatch = null;
            try
            {
                using (ERP_Entities _ctxCmn = new ERP_Entities())
                {
                    listBatch = (from olt in _ctxCmn.CmnBatches
                                 where
                                 olt.ItemID == (ItemID == 0 ? olt.ItemID : (int)ItemID)
                                 && olt.CompanyID == companyID && olt.IsDeleted == false
                                 select new vmCmnBatch
                                 {
                                     BatchID = olt.BatchID,
                                     BatchNo = olt.BatchNo,
                                     ItemID = (int)(olt.ItemID ?? 0),
                                 }).ToList();


                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listBatch.OrderBy(x => x.BatchID).ToList();
        }

        public IEnumerable<vmCmnBatch> GetAllBatches(vmCmnParameters objcmnParam)
        {
            //GenericFactory_PrdFinishingProcess_EF = new PrdFinishingProcess_EF();
            IEnumerable<vmCmnBatch> objBatch = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objBatch = (from CIG in _ctxCmn.CmnBatches
                                join ISM in _ctxCmn.InvStockMasters on CIG.BatchID equals ISM.BatchID
                                where CIG.CompanyID == objcmnParam.loggedCompany && CIG.IsDeleted == false && ISM.CurrentStock > 0
                                orderby CIG.BatchNo
                                select new
                                {
                                    BatchID = CIG.BatchID,
                                    BatchNo = CIG.BatchNo
                                }).ToList().Select(x => new vmCmnBatch
                                {
                                    BatchID = x.BatchID,
                                    BatchNo = x.BatchNo
                                }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBatch;
        }

        public List<vmPrdFinishingType> GetPrdFinishingType(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmPrdFinishingType> listPFtype = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    listPFtype = (from pft in _ctxCmn.PrdFinishingTypes
                                  where pft.CompanyID == companyID && pft.IsDeleted == false
                                  select new vmPrdFinishingType
                                  {
                                      FInishTypeID = pft.FInishTypeID,
                                      FInishTypeName = pft.FInishTypeName
                                  }).ToList();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listPFtype.OrderBy(x => x.FInishTypeID).ToList();
        }
        public List<vmFinishingInspactionDetail> GetInspactionDetailsByIDandDates(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? ItemId, DateTime? FromDate, DateTime? ToDate)
        {
            List<vmFinishingInspactionDetail> _obj = new List<vmFinishingInspactionDetail>();
            return _obj;
        }

        public List<vmPrdWeavingMachinConfig> GetMachine(vmCmnParameters objcmnParam)
        {
            try
            {
                ERP_Entities _ctxCmn = new ERP_Entities();
                return (from machine in _ctxCmn.PrdWeavingMachinConfigs
                        where machine.CompanyID == objcmnParam.loggedCompany
                        && machine.IsDeleted == false
                        && machine.DepartmentID == objcmnParam.DepartmentID
                        && machine.IsBook == false
                        select new vmPrdWeavingMachinConfig
                        {
                            MachineConfigID = machine.MachineConfigID,
                            MachineConfigNo = machine.MachineConfigNo
                        }).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return new List<vmPrdWeavingMachinConfig>();
        }


        public object GetDyingMachineForChemicalProcess(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            try
            {
                using(GenericFactory_PrdDyingMachinePart_EF = new PrdDyingMachinePart_EF())
                {
                    return GenericFactory_PrdDyingMachinePart_EF.FindBy(x => x.CompanyID == companyID && x.IsDeleted == false).Select(x=>new vmPrdDyingMachinePart {MachineID=x.MachineID ,MachineName=x.CmnItemMaster.ItemName }).GroupBy(i => i.MachineID).Select(i=>i.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public object GetDyingReferenceByItemID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging,int? ItemID)
        {
            try
            {
                using (GenericFactory_PrdDyingMRRSet_EF = new PrdDyingMRRSet_EF())
                {
                    return GenericFactory_PrdDyingMRRSet_EF.FindBy(x => x.CompanyID == companyID && x.IsDeleted == false).Select(x => new vmPrdDyingMRRSet { DyingSetID = x.DyingSetID, DyingSetNo = x.DyingSetNo }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }



        void iProductionDDLMgt.Dispose()
        {

        }
        void IDisposable.Dispose()
        {

        }
    }

}
