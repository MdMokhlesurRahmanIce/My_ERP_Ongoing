using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ABS.Service.Production.Factories
{
    public class CountWiseMechineSetupMgt
    {
        private ERP_Entities _ctxCmn = null;

        private iGenericFactory_EF<PrdBallMachineSetup> GFactory_EF_PrdBallMachineSetup = null;

        public IEnumerable<CmnItemCount> GetCount(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            recordsTotal = 0;
            IEnumerable<CmnItemCount> objCount = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    if (objcmnParam.id > 0)
                    {
                        objCount = (from IC in _ctxCmn.CmnItemCounts
                                    where IC.Count == objcmnParam.id && IC.CompanyID == objcmnParam.loggedCompany && IC.IsDeleted == false
                                    orderby IC.Count
                                    select new
                                    {
                                        //CountID = IC.CountID,
                                        Count = IC.Count
                                    }).ToList().Select(x => new CmnItemCount
                                    {
                                        //CountID = x.CountID,
                                        Count = x.Count
                                    }).ToList();
                    }
                    else
                    {
                        objCount = (from IC in _ctxCmn.CmnItemCounts
                                    where IC.CompanyID == objcmnParam.loggedCompany && IC.IsDeleted == false
                                    orderby IC.Count
                                    select new
                                    {
                                        //CountID = IC.CountID,
                                        Count = IC.Count
                                    }).ToList().Select(x => new CmnItemCount
                                    {
                                        //CountID = x.CountID,
                                        Count = x.Count
                                    }).ToList();
                    }

                    recordsTotal = objCount.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCount;
        }

        public IEnumerable<PrdBallMachineSetup> GetMachinSetup(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            recordsTotal = 0;
            IEnumerable<PrdBallMachineSetup> ListMachineSet = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    ListMachineSet = (from PBMS in _ctxCmn.PrdBallMachineSetups.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false
                                      && objcmnParam.id == 0 ? true : x.BallMachineSetupID == objcmnParam.id)
                                      select new
                                      {
                                          BallMachineSetupID = PBMS.BallMachineSetupID,
                                          Count = PBMS.Count,
                                          Jog = PBMS.Jog,
                                          RFront = PBMS.RFront,
                                          RRear = PBMS.RRear,
                                          Agm = PBMS.Agm,
                                          Empty = PBMS.Empty,
                                          Speed = PBMS.Speed

                                      }).ToList().Select(x => new PrdBallMachineSetup
                                      {
                                          BallMachineSetupID = x.BallMachineSetupID,
                                          Count = x.Count,
                                          Jog = x.Jog,
                                          RFront = x.RFront,
                                          RRear = x.RRear,
                                          Agm = x.Agm,
                                          Empty = x.Empty,
                                          Speed = x.Speed
                                      }).ToList();

                    if (objcmnParam.id == 0)
                    {
                        recordsTotal = ListMachineSet.Count();
                        ListMachineSet = ListMachineSet.OrderByDescending(x => x.BallMachineSetupID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return ListMachineSet;
        }

        public IEnumerable<PrdBallMachineSetup> GetCountToCheck(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            recordsTotal = 0;
            IEnumerable<PrdBallMachineSetup> objCountCheck = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objCountCheck = (from MS in _ctxCmn.PrdBallMachineSetups
                                     where MS.Count == objcmnParam.id //MS.CompanyID == objcmnParam.loggedCompany && MS.IsDeleted == false
                                     orderby MS.Count descending
                                     select new
                                     {
                                         Count = MS.Count
                                     }).ToList().Select(x => new PrdBallMachineSetup
                                     {
                                         Count = x.Count
                                     }).ToList();
                }

                recordsTotal = objCountCheck.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objCountCheck;
        }

        public string SaveUpdateCountWiseMachineSetup(vmPrdBallMachineSetup model)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GFactory_EF_PrdBallMachineSetup = new PrdBallMachineSetup_EF();
                int BallMachineSetupID = 0;
                
                var BallMachineSetup = new PrdBallMachineSetup();
                var AllMachineSetup = GFactory_EF_PrdBallMachineSetup.GetAll();
                try
                {
                    //using (_ctxCmn = new ERP_Entities())
                    //{
                    if (model.BallMachineSetupID > 0)
                    {
                        BallMachineSetup = AllMachineSetup.FirstOrDefault(x => x.BallMachineSetupID == model.BallMachineSetupID);
                        if (model.IsDeleted != false)
                        {
                            BallMachineSetup.CompanyID = (int)model.LCompanyID;
                            BallMachineSetup.DeleteBy = (int)model.LUserID;
                            BallMachineSetup.IsDeleted = model.IsDeleted;
                            BallMachineSetup.DeleteOn = DateTime.Now;
                            BallMachineSetup.DeletePc =  HostService.GetIP();
                        }
                        else
                        {
                            BallMachineSetup.Count = (int)model.Count;
                            BallMachineSetup.Jog = model.Jog;
                            BallMachineSetup.RFront = model.RFront;
                            BallMachineSetup.RRear = model.RRear;
                            BallMachineSetup.Agm = model.Agm;
                            BallMachineSetup.Empty = model.Empty;
                            BallMachineSetup.Speed = model.Speed;
                            BallMachineSetup.CompanyID = (int)model.LCompanyID;
                            BallMachineSetup.UpdateBy = (int)model.LUserID;
                            BallMachineSetup.UpdateOn = DateTime.Now;
                            BallMachineSetup.UpdatePc =  HostService.GetIP();
                            BallMachineSetup.IsDeleted = false;
                        }

                        GFactory_EF_PrdBallMachineSetup.Update(BallMachineSetup);
                        GFactory_EF_PrdBallMachineSetup.Save();
                    }
                    else
                    {
                        BallMachineSetupID = Convert.ToInt16(GFactory_EF_PrdBallMachineSetup.getMaxID("PrdBallMachineSetup"));
                        BallMachineSetup = new PrdBallMachineSetup
                        {
                            BallMachineSetupID = BallMachineSetupID,
                            Count = (int)model.Count,
                            Jog = model.Jog,
                            RFront = model.RFront,
                            RRear = model.RRear,
                            Agm = model.Agm,
                            Empty = model.Empty,
                            Speed = model.Speed,
                            CompanyID = (int)model.LCompanyID,
                            CreateBy = (int)model.LUserID,
                            CreateOn = DateTime.Now,
                            CreatePc =  HostService.GetIP(),
                            IsDeleted = false
                        };

                        GFactory_EF_PrdBallMachineSetup.Insert(BallMachineSetup);
                        GFactory_EF_PrdBallMachineSetup.Save();
                        GFactory_EF_PrdBallMachineSetup.updateMaxID("PrdBallMachineSetup", Convert.ToInt64(BallMachineSetupID));
                    }

                    transaction.Complete();
                    result = BallMachineSetup.Count.ToString();
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }
    }
}
