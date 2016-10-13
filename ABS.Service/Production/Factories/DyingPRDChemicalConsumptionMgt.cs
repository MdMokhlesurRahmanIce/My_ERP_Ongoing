using ABS.Service.Production.Interfaces;
using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Models;
using System.Collections;
using ABS.Data.BaseInterfaces;
using ABS.Data.BaseFactories;
using ABS.Models.ViewModel.Production;
using ABS.Service.AllServiceClasses;
using ABS.Utility;
using System.Web;
using System.Transactions;
using ABS.Models.ViewModel.SystemCommon;

namespace ABS.Service.Production.Factories
{
    public class DyingPRDChemicalConsumptionMgt : iDyingPRDChemicalConsumptionMgt, IDisposable
    {
        bool disposed = false;
        
        private ERP_Entities _ctxObj = null;
        private iGenericFactory_EF<PrdDyingConsumptionMaster> PrdDyingConsumptionMaster_IEF = null;
        private iGenericFactory_EF<PrdDyingConsumptionDetail> PrdDyingConsumptionDetail_IEF = null;
        private iGenericFactory_EF<PrdDyingConsumptionChemicalM> PrdDyingConsumptionChemicalM_IEF = null;
        
        private iGenericFactory_EF<PrdDyingConsumptionChemical> PrdDyingConsumptionChemical_IEF = null;
        private iGenericFactory_EF<PrdDyingConsumptionChemical> PrdDyingConsumptionChemical_IEFI = null;
        private iGenericFactory_EF<PrdDyingConsumptionChemical> PrdDyingConsumptionChemical_IEFU = null;
        #region Save 
        //public PrdDyingConsumptionMaster BakeMaster(PrdDyingConsumptionMaster master, List<PrdDyingConsumptionDetail> detailsList, UserCommonEntity commonEntity)
        //{
        //    PrdDyingConsumptionMaster consumption = new PrdDyingConsumptionMaster();
        //    PrdDyingConsumptionChemicalM chemical = new PrdDyingConsumptionChemicalM();
        //    PrdDyingConsumptionChemical chemicalDetails = new PrdDyingConsumptionChemical();
        //    PrdDyingConsumptionDetail consumptionDetails = new PrdDyingConsumptionDetail();
        //    try
        //    {
        //        //using (ERP_Entities contex = new ERP_Entities())
        //        //{
        //        ERP_Entities contex = new ERP_Entities();
        //            PrdDyingConsumptionMaster_IEF = new PrdDyingConsumptionMaster_EF();
        //            PrdDyingConsumptionDetail_IEF = new PrdDyingConsumptionDetail_EF();
        //            PrdDyingConsumptionChemicalM_IEF = new PrdDyingConsumptionChemicalM_EF();
        //            PrdDyingConsumptionChemical_IEF = new PrdDyingConsumptionChemical_EF();
        //            #region Master Begin
        //            Int64 ID = 0;
        //            consumption.DyingConsumptionNo = master.DyingConsumptionNo;
        //            consumption.SetID = master.SetID;
        //            consumption.ItemID = master.ItemID;
        //            consumption.Date = master.Date;
        //            consumption.IndigoStart = master.IndigoStart;
        //            consumption.IndigoStop = master.IndigoStop;
        //            consumption.BlackStart = master.BlackStart;
        //            consumption.BlackStop = master.BlackStop;
        //            consumption.OperationID = master.OperationID;
        //            if (master.EntityState == "Insert")
        //            {
        //                ID = Convert.ToInt64(PrdDyingConsumptionMaster_IEF.getMaxID("PrdDyingConsumptionMaster"));
        //                PrdDyingConsumptionMaster_IEF.updateMaxID("PrdDyingConsumptionMaster", ID);
        //                consumption.DyingConsumptionID = ID;

        //                contex.Entry(consumption).State = System.Data.Entity.EntityState.Added;
        //                consumption.CreateBy = commonEntity.loggedUserID;
        //                consumption.CreateOn = serverDate;
        //                consumption.CreatePc = HostService.GetIP();

        //            }
        //            else if (master.EntityState == "Update")
        //            {
        //                contex.Entry(consumption).State = System.Data.Entity.EntityState.Modified;
        //                consumption.DyingConsumptionID = master.DyingConsumptionID;
        //                if (master.IsDeleted == true)
        //                {
        //                    consumption.DeleteBy = commonEntity.loggedUserID;
        //                    consumption.DeleteOn = serverDate;
        //                    consumption.DeletePc = HostService.GetIP();
        //                }
        //                else
        //                {
        //                    consumption.UpdateBy = commonEntity.loggedUserID;
        //                    consumption.UpdateOn = serverDate;
        //                    consumption.UpdatePc = HostService.GetIP();
        //                }
        //            }
        //            consumption.CompanyID = commonEntity.loggedCompnyID ?? 0;
        //            consumption.IsDeleted = master.IsDeleted;
        //            #endregion Master Begin End
        //            #region Details Begin
        //            foreach (PrdDyingConsumptionDetail details in detailsList)
        //            {
        //                consumptionDetails = new PrdDyingConsumptionDetail();
        //                if (details.EntityState == "Insert")
        //                {
        //                    ID = Convert.ToInt64(PrdDyingConsumptionDetail_IEF.getMaxID("PrdDyingConsumptionDetail"));
        //                    PrdDyingConsumptionDetail_IEF.updateMaxID("PrdDyingConsumptionDetail", ID);
        //                }
        //                else
        //                    ID = details.DyingConsumptionDetailID;
        //                consumptionDetails.DyingConsumptionDetailID = ID;
        //                consumptionDetails.DyingConsumptionID = consumption.DyingConsumptionID;
        //                consumptionDetails.MachinePartID = details.MachinePartID;
        //                consumptionDetails.OperationID = details.OperationID;
        //                consumptionDetails.Temp = details.Temp;
        //                #region Chemical Master
        //                ID = 0;
        //                if (details.ConsumptionChemicalMID > 0)
        //                {
                           
        //                    chemical = new PrdDyingConsumptionChemicalM();
                            
        //                    chemical.TotalChemical = details.PrdDyingConsumptionChemicalM.TotalChemical;
        //                    if (details.PrdDyingConsumptionChemicalM.EntityState == "Insert")
        //                    {
        //                        ID = Convert.ToInt64(PrdDyingConsumptionChemicalM_IEF.getMaxID("PrdDyingConsumptionChemicalM"));
        //                        PrdDyingConsumptionChemicalM_IEF.updateMaxID("PrdDyingConsumptionChemicalM", ID);
        //                        chemical.ConsumptionChemicaMlID = ID;

        //                        contex.Entry(chemical).State = System.Data.Entity.EntityState.Added;
        //                        chemical.CreateBy = commonEntity.loggedUserID;
        //                        chemical.CreateOn = serverDate;
        //                        chemical.CreatePc = HostService.GetIP();
        //                    }
        //                    else if (details.PrdDyingConsumptionChemicalM.EntityState == "Update")
        //                    {
        //                        chemical.ConsumptionChemicaMlID =(long) details.ConsumptionChemicalMID;// details.PrdDyingConsumptionChemicalM.ConsumptionChemicaMlID;
        //                        ID = chemical.ConsumptionChemicaMlID;
        //                        //contex.Entry(chemical).State = System.Data.Entity.EntityState.Modified;
        //                        if (chemical.IsDeleted == true)
        //                        {
        //                            chemical.DeleteBy = commonEntity.loggedUserID;
        //                            chemical.DeleteOn = serverDate;
        //                            chemical.DeletePc = HostService.GetIP();
        //                        }
        //                        else
        //                        {
        //                            chemical.UpdateBy = commonEntity.loggedUserID;
        //                            chemical.UpdateOn = serverDate;
        //                            chemical.UpdatePc = HostService.GetIP();
        //                        }
        //                    }
        //                    chemical.CompanyID = commonEntity.loggedCompnyID ?? 0;
        //                    chemical.IsDeleted = details.PrdDyingConsumptionChemicalM.IsDeleted;

        //                }
        //                #endregion Checmical Master
        //                if (ID == 0)
        //                {
        //                    consumptionDetails.ConsumptionChemicalMID = null;// details.ConsumptionChemicalMID;
        //                }
        //                else
        //                {
        //                    consumptionDetails.ConsumptionChemicalMID = ID;
        //                    #region Chemical Details
        //                    var masterWatch = System.Diagnostics.Stopwatch.StartNew();
                           
                          
        //                    foreach (PrdDyingConsumptionChemical item in details.PrdDyingConsumptionChemicalM.PrdDyingConsumptionChemicals)
        //                    {
        //                        var item1 =details.PrdDyingConsumptionChemicalM.PrdDyingConsumptionChemicals.Count();
        //                        chemicalDetails = new PrdDyingConsumptionChemical();
        //                        chemicalDetails.ConsumptionChemicaMlID = item.ConsumptionChemicaMlID;
                              
        //                        chemicalDetails.ConsumptionChemicaMlID = consumptionDetails.ConsumptionChemicalMID ?? 0;
        //                        chemicalDetails.ChemicalID = item.ChemicalID;
        //                        chemicalDetails.DepartmentID = item.DepartmentID;
        //                        chemicalDetails.BatchID = item.BatchID;
        //                        chemicalDetails.Qty = item.Qty;
        //                        chemicalDetails.UnitID = item.UnitID;

        //                        if (item.EntityState == "Insert")
        //                        {
        //                            ID = Convert.ToInt64(PrdDyingConsumptionChemical_IEF.getMaxID("PrdDyingConsumptionChemical"));
        //                            PrdDyingConsumptionChemical_IEF.updateMaxID("PrdDyingConsumptionChemical", ID);
        //                            chemicalDetails.ConsumptionChemicalID = ID;

        //                            contex.Entry(chemicalDetails).State = System.Data.Entity.EntityState.Added;
        //                            chemicalDetails.CreateBy = commonEntity.loggedUserID;
        //                            chemicalDetails.CreateOn = serverDate;
        //                            chemicalDetails.CreatePc = HostService.GetIP();
        //                        }
        //                        else if (item.EntityState == "Update")
        //                        {
                                   
        //                            chemicalDetails.ConsumptionChemicalID = item.ConsumptionChemicalID;
        //                            if (chemicalDetails.IsDeleted == true)
        //                            {
        //                                chemicalDetails.DeleteBy = commonEntity.loggedUserID;
        //                                chemicalDetails.DeleteOn = serverDate;
        //                                chemicalDetails.DeletePc = HostService.GetIP();
        //                            }
        //                            else
        //                            {
        //                                chemicalDetails.UpdateBy = commonEntity.loggedUserID;
        //                                chemicalDetails.UpdateOn = serverDate;
        //                                chemicalDetails.UpdatePc = HostService.GetIP();
        //                            }
        //                        }
        //                        chemicalDetails.IsDeleted = item.IsDeleted;
        //                        chemical.PrdDyingConsumptionChemicals.Add(chemicalDetails);
        //                    }
        //                    masterWatch.Stop();
        //                    var masterWatchMs = masterWatch.ElapsedMilliseconds;
        //                    #endregion Checmical Details
        //                }

        //                consumptionDetails.Volume = details.Volume;
        //                consumptionDetails.UnitID = details.UnitID;
        //                consumptionDetails.CompanyID = commonEntity.loggedCompnyID ?? 0;
        //                if (details.EntityState == "Insert")
        //                {
        //                    contex.Entry(consumptionDetails).State = System.Data.Entity.EntityState.Added;
        //                    consumptionDetails.CreateBy = commonEntity.loggedUserID;
        //                    consumptionDetails.CreateOn = serverDate;
        //                    consumptionDetails.CreatePc = HostService.GetIP();
        //                }
        //                else if (details.EntityState == "Update")
        //                {
        //                    //contex.Entry(consumption).State = System.Data.Entity.EntityState.Modified;
        //                    if (master.IsDeleted == true)
        //                    {
        //                        consumptionDetails.DeleteBy = commonEntity.loggedUserID;
        //                        consumptionDetails.DeleteOn = serverDate;
        //                        consumptionDetails.DeletePc = HostService.GetIP();
        //                    }
        //                    else
        //                    {
        //                        consumptionDetails.UpdateBy = commonEntity.loggedUserID;
        //                        consumptionDetails.UpdateOn = serverDate;
        //                        consumptionDetails.UpdatePc = HostService.GetIP();
        //                    }
        //                }
        //                consumptionDetails.CompanyID = commonEntity.loggedCompnyID ?? 0;
        //                consumptionDetails.IsDeleted = details.IsDeleted;
        //                consumption.PrdDyingConsumptionDetails.Add(consumptionDetails);

        //            }

        //            #endregion Details End
        //            contex.SaveChanges();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //    return consumption;
        //}
        public PrdDyingConsumptionMaster BakeMaster(PrdDyingConsumptionMaster master, List<PrdDyingConsumptionDetail> detailsList, UserCommonEntity commonEntity)
        {
            PrdDyingConsumptionMaster consumption = new PrdDyingConsumptionMaster();
            PrdDyingConsumptionChemicalM chemical = new PrdDyingConsumptionChemicalM();
            PrdDyingConsumptionChemical chemicalDetails = new PrdDyingConsumptionChemical();
            PrdDyingConsumptionDetail consumptionDetails = new PrdDyingConsumptionDetail();
            DateTime serverDate = DateTime.Now;
            String HostID = HostService.GetIP();
            try
            {
                //using (ERP_Entities contex = new ERP_Entities())
                //{
                ERP_Entities contex = new ERP_Entities();
                PrdDyingConsumptionMaster_IEF = new PrdDyingConsumptionMaster_EF();
                PrdDyingConsumptionDetail_IEF = new PrdDyingConsumptionDetail_EF();
                PrdDyingConsumptionChemicalM_IEF = new PrdDyingConsumptionChemicalM_EF();
                PrdDyingConsumptionChemical_IEF = new PrdDyingConsumptionChemical_EF();

                List<PrdDyingConsumptionChemicalM> InsertionChemicaMlList = new List<PrdDyingConsumptionChemicalM>();
                List<PrdDyingConsumptionChemical> InsertionChemicalList = new List<PrdDyingConsumptionChemical>();
                List<PrdDyingConsumptionDetail> InsertionConsumptionDetailsList = new List<PrdDyingConsumptionDetail>();
                #region Master Begin
                Int64 ID = 0;
                consumption.DyingConsumptionNo = master.DyingConsumptionNo;
                consumption.SetID = master.SetID;
                consumption.ItemID = master.ItemID;
                consumption.Date = master.Date;
                consumption.IndigoStart = master.IndigoStart;
                consumption.IndigoStop = master.IndigoStop;
                consumption.BlackStart = master.BlackStart;
                consumption.BlackStop = master.BlackStop;
                consumption.OperationID = master.OperationID;
                Int64 masterID = 0;
                Int64 detailsID = 0;
                Int64 ChecmicalID = 0;
                Int64 ChecmicalDetailID = 0;
                masterID= Convert.ToInt64(PrdDyingConsumptionMaster_IEF.getMaxID("PrdDyingConsumptionMaster"));
                detailsID = Convert.ToInt64(PrdDyingConsumptionDetail_IEF.getMaxID("PrdDyingConsumptionDetail"));
                ChecmicalID = Convert.ToInt64(PrdDyingConsumptionChemicalM_IEF.getMaxID("PrdDyingConsumptionChemicalM"));
                ChecmicalDetailID = Convert.ToInt64(PrdDyingConsumptionChemical_IEF.getMaxID("PrdDyingConsumptionChemical"));

               
                if (master.EntityState == "Insert")
                {
                    ID = masterID;
                    consumption.DyingConsumptionID = ID;

                 //   contex.Entry(consumption).State = System.Data.Entity.EntityState.Added;
                    consumption.CreateBy = commonEntity.loggedUserID;
                    consumption.CreateOn = serverDate;
                    consumption.CreatePc = HostID;

                }
                else if (master.EntityState == "Update")
                {
                  //  contex.Entry(consumption).State = System.Data.Entity.EntityState.Modified;
                    consumption.DyingConsumptionID = master.DyingConsumptionID;
                    if (master.IsDeleted == true)
                    {
                        consumption.DeleteBy = commonEntity.loggedUserID;
                        consumption.DeleteOn = serverDate;
                        consumption.DeletePc = HostID;
                    }
                    else
                    {
                        consumption.UpdateBy = commonEntity.loggedUserID;
                        consumption.UpdateOn = serverDate;
                        consumption.UpdatePc = HostID;
                    }
                }
                consumption.CompanyID = commonEntity.loggedCompnyID ?? 0;
                consumption.IsDeleted = master.IsDeleted;
                #endregion Master Begin End
                #region Details Begin
                foreach (PrdDyingConsumptionDetail details in detailsList)
                {
                    consumptionDetails = new PrdDyingConsumptionDetail();
                    
                    if (details.EntityState == "Insert")
                    {
                        ID = ++detailsID; 
                       
                    }
                    else
                        ID = details.DyingConsumptionDetailID;
                    consumptionDetails.DyingConsumptionDetailID = ID;
                    consumptionDetails.DyingConsumptionID = consumption.DyingConsumptionID;
                    consumptionDetails.MachinePartID = details.MachinePartID;
                    consumptionDetails.OperationID = details.OperationID;
                    consumptionDetails.Temp = details.Temp;
                    #region Chemical Master
                    ID = 0;
                    if (details.ConsumptionChemicalMID > 0)
                    {

                        chemical = new PrdDyingConsumptionChemicalM();
                        chemical.TotalChemical = details.PrdDyingConsumptionChemicalM.TotalChemical;
                        if (details.PrdDyingConsumptionChemicalM.EntityState == "Insert")
                        {
                            ID = ++ChecmicalID;
                            chemical.ConsumptionChemicaMlID = ID;

                           // contex.Entry(chemical).State = System.Data.Entity.EntityState.Added;
                            chemical.CreateBy = commonEntity.loggedUserID;
                            chemical.CreateOn = serverDate;
                            chemical.CreatePc = HostID;
                        }
                        else if (details.PrdDyingConsumptionChemicalM.EntityState == "Update")
                        {
                            chemical.ConsumptionChemicaMlID = (long)details.ConsumptionChemicalMID;// details.PrdDyingConsumptionChemicalM.ConsumptionChemicaMlID;
                            ID = chemical.ConsumptionChemicaMlID;
                            //contex.Entry(chemical).State = System.Data.Entity.EntityState.Modified;
                            if (chemical.IsDeleted == true)
                            {
                                chemical.DeleteBy = commonEntity.loggedUserID;
                                chemical.DeleteOn = serverDate;
                                chemical.DeletePc = HostID;
                            }
                            else
                            {
                                chemical.UpdateBy = commonEntity.loggedUserID;
                                chemical.UpdateOn = serverDate;
                                chemical.UpdatePc = HostID;
                            }
                        }
                        chemical.CompanyID = commonEntity.loggedCompnyID ?? 0;
                        chemical.IsDeleted = details.PrdDyingConsumptionChemicalM.IsDeleted;
                        //New
                        InsertionChemicaMlList.Add(chemical);
                    }
                    #endregion Checmical Master
                    if (ID == 0)
                    {
                        consumptionDetails.ConsumptionChemicalMID = null;// details.ConsumptionChemicalMID;
                    }
                    else
                    {
                        consumptionDetails.ConsumptionChemicalMID = ID;
                        #region Chemical Details
                        


                        foreach (PrdDyingConsumptionChemical item in details.PrdDyingConsumptionChemicalM.PrdDyingConsumptionChemicals)
                        {
                            if(item.Qty>0)
                            { 
                            var item1 = details.PrdDyingConsumptionChemicalM.PrdDyingConsumptionChemicals.Count();
                            chemicalDetails = new PrdDyingConsumptionChemical();
                            chemicalDetails.ConsumptionChemicaMlID = item.ConsumptionChemicaMlID;

                            chemicalDetails.ConsumptionChemicaMlID = consumptionDetails.ConsumptionChemicalMID ?? 0;
                            chemicalDetails.ChemicalID = item.ChemicalID;
                            chemicalDetails.DepartmentID = commonEntity.loggedUserDepartmentID;
                            chemicalDetails.BatchID = item.BatchID;
                            chemicalDetails.SupplierID = item.SupplierID;
                            chemicalDetails.TransactionTypeID = commonEntity.currentTransactionTypeID;
                            chemicalDetails.Qty = item.Qty;
                            chemicalDetails.UnitID = item.UnitID;
                            chemicalDetails.UnitPrice = item.UnitPrice??0;
                            chemicalDetails.Amount = item.UnitPrice??0 * item.Qty??0;                           
                            chemicalDetails.ConsumptionDate = master.Date;
                            chemicalDetails.CompanyID = (int)commonEntity.loggedCompnyID;

                            if (item.EntityState == "Insert")
                            {
                                ID = ++ChecmicalDetailID;
                                chemicalDetails.ConsumptionChemicalID = ID;
                                //    contex.Entry(chemicalDetails).State = System.Data.Entity.EntityState.Added;
                                chemicalDetails.CreateBy = commonEntity.loggedUserID;
                                chemicalDetails.CreateOn = serverDate;
                                chemicalDetails.CreatePc = HostID;
                            }
                            else if (item.EntityState == "Update")
                            {
                                chemicalDetails.ConsumptionChemicalID = item.ConsumptionChemicalID;
                                if (chemicalDetails.IsDeleted == true)
                                {
                                    chemicalDetails.DeleteBy = commonEntity.loggedUserID;
                                    chemicalDetails.DeleteOn = serverDate;
                                    chemicalDetails.DeletePc = HostID;
                                }
                                else
                                {
                                    chemicalDetails.UpdateBy = commonEntity.loggedUserID;
                                    chemicalDetails.UpdateOn = serverDate;
                                    chemicalDetails.UpdatePc = HostID;
                                }
                            }
                            chemicalDetails.IsDeleted = item.IsDeleted;
                            //   chemical.PrdDyingConsumptionChemicals.Add(chemicalDetails);
                            //New
                            InsertionChemicalList.Add(chemicalDetails);
                        }
                        }
                        #endregion Checmical Details
                    }

                    consumptionDetails.Volume = details.Volume;
                    consumptionDetails.UnitID = details.UnitID;
                    consumptionDetails.CompanyID = commonEntity.loggedCompnyID ?? 0;
                    if (details.EntityState == "Insert")
                    {
                      //  contex.Entry(consumptionDetails).State = System.Data.Entity.EntityState.Added;
                        consumptionDetails.CreateBy = commonEntity.loggedUserID;
                        consumptionDetails.CreateOn = serverDate;
                        consumptionDetails.CreatePc = HostID;
                    }
                    else if (details.EntityState == "Update")
                    {
                        //contex.Entry(consumption).State = System.Data.Entity.EntityState.Modified;
                        if (master.IsDeleted == true)
                        {
                            consumptionDetails.DeleteBy = commonEntity.loggedUserID;
                            consumptionDetails.DeleteOn = serverDate;
                            consumptionDetails.DeletePc = HostID;
                        }
                        else
                        {
                            consumptionDetails.UpdateBy = commonEntity.loggedUserID;
                            consumptionDetails.UpdateOn = serverDate;
                            consumptionDetails.UpdatePc = HostID;
                        }
                    }
                    consumptionDetails.CompanyID = commonEntity.loggedCompnyID ?? 0;
                    consumptionDetails.IsDeleted = details.IsDeleted;
                    // consumption.PrdDyingConsumptionDetails.Add(consumptionDetails);
                    InsertionConsumptionDetailsList.Add(consumptionDetails);

                }

                #endregion Details End
                //   contex.SaveChanges();
                PrdDyingConsumptionMaster_IEF.Insert(consumption);
                PrdDyingConsumptionMaster_IEF.Save();
                PrdDyingConsumptionChemicalM_IEF.InsertList(InsertionChemicaMlList);
                PrdDyingConsumptionChemicalM_IEF.Save();
                PrdDyingConsumptionDetail_IEF.InsertList(InsertionConsumptionDetailsList);
                PrdDyingConsumptionDetail_IEF.Save();
                PrdDyingConsumptionChemical_IEF.InsertList(InsertionChemicalList);
                PrdDyingConsumptionChemical_IEF.Save();

                PrdDyingConsumptionMaster_IEF.updateMaxID("PrdDyingConsumptionMaster", masterID);
                PrdDyingConsumptionDetail_IEF.updateMaxID("PrdDyingConsumptionDetail", detailsID);
                PrdDyingConsumptionChemicalM_IEF.updateMaxID("PrdDyingConsumptionChemicalM", ChecmicalID);
                PrdDyingConsumptionChemical_IEF.updateMaxID("PrdDyingConsumptionChemical", ChecmicalDetailID);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return consumption;
        }
        public PrdDyingConsumptionMaster BakeMasterUpdate(PrdDyingConsumptionMaster master, List<PrdDyingConsumptionDetail> detailsList, UserCommonEntity commonEntity)
        {

            PrdDyingConsumptionMaster_IEF = new PrdDyingConsumptionMaster_EF();
            PrdDyingConsumptionDetail_IEF = new PrdDyingConsumptionDetail_EF();
            PrdDyingConsumptionChemicalM_IEF = new PrdDyingConsumptionChemicalM_EF();

            PrdDyingConsumptionChemical_IEF = new PrdDyingConsumptionChemical_EF();
            PrdDyingConsumptionChemical_IEFI = new PrdDyingConsumptionChemical_EF();
            PrdDyingConsumptionChemical_IEFU = new PrdDyingConsumptionChemical_EF();

            List<PrdDyingConsumptionDetail> UpdateConsumptionDetailsList = new List<PrdDyingConsumptionDetail>();
            List<PrdDyingConsumptionDetail> InsertionConsumptionDetailsList = new List<PrdDyingConsumptionDetail>();
            List<PrdDyingConsumptionChemicalM> UpdateChemicalMList = new List<PrdDyingConsumptionChemicalM>();
            List<PrdDyingConsumptionChemicalM> InsertionChemicaMlList = new List<PrdDyingConsumptionChemicalM>();
            List<PrdDyingConsumptionChemical> UpdateChemicalList = new List<PrdDyingConsumptionChemical>();
            List<PrdDyingConsumptionChemical> InsertionChemicalList = new List<PrdDyingConsumptionChemical>();

            DateTime serverDate = DateTime.Now;
            String HostID = HostService.GetIP();
            ERP_Entities context = new ERP_Entities();
            serverDate = context.Database.SqlQuery<DateTime>("SELECT getdate()").AsEnumerable().First();
            Int64 UmasterID = 0;
            Int64 UdetailsID = 0;
            Int64 UChecmicalID = 0;
            Int64 UChecmicalDetailID = 0;

            UmasterID = Convert.ToInt64(PrdDyingConsumptionMaster_IEF.getMaxID("PrdDyingConsumptionMaster"));
            UdetailsID = Convert.ToInt64(PrdDyingConsumptionDetail_IEF.getMaxID("PrdDyingConsumptionDetail"));
            UChecmicalID = Convert.ToInt64(PrdDyingConsumptionChemicalM_IEF.getMaxID("PrdDyingConsumptionChemicalM"));
            UChecmicalDetailID = Convert.ToInt64(PrdDyingConsumptionChemical_IEF.getMaxID("PrdDyingConsumptionChemical"));

            PrdDyingConsumptionMaster consumption = new PrdDyingConsumptionMaster();
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    consumption = PrdDyingConsumptionMaster_IEF.GetAll().Where(x => x.DyingConsumptionID == master.DyingConsumptionID).FirstOrDefault();
                    #region Master
                    consumption.DyingConsumptionID = master.DyingConsumptionID;
                    consumption.DyingConsumptionNo = master.DyingConsumptionNo;
                    consumption.SetID = master.SetID;
                    consumption.ItemID = master.ItemID;
                    consumption.Date = master.Date;
                    consumption.IndigoStart = master.IndigoStart;
                    consumption.IndigoStop = master.IndigoStop;
                    consumption.BlackStart = master.BlackStart;
                    consumption.BlackStop = master.BlackStop;
                    consumption.OperationID = master.OperationID;
                    
                    if (master.IsDeleted == true)
                    {
                        consumption.DeleteBy = commonEntity.loggedUserID;
                        consumption.DeleteOn = serverDate;
                        consumption.DeletePc = HostID;
                       
                    }
                    else
                    {
                        consumption.UpdateBy = commonEntity.loggedUserID;
                        consumption.UpdateOn = serverDate;
                        consumption.UpdatePc = HostID;
                    }
                    consumption.CompanyID = commonEntity.loggedCompnyID ?? 0;
                    consumption.IsDeleted = master.IsDeleted;
                    #endregion Master End
                    Int64 ID = 0;
                    bool DeleteAllChild = false;
                    #region Details Begin
                    foreach (PrdDyingConsumptionDetail details in detailsList)
                    {
                        var consumptionDetails = new PrdDyingConsumptionDetail();
                       
                        if (details.EntityState == "Insert")
                        {
                            ID = ++UdetailsID;
                            consumptionDetails.ConsumptionChemicalMID = null;
                            consumptionDetails.CreateBy = commonEntity.loggedUserID;
                            consumptionDetails.CreateOn = serverDate;
                            consumptionDetails.CreatePc = HostID;
                           

                        }
                        else
                        {
                            consumptionDetails = PrdDyingConsumptionDetail_IEF.GetAll().Where(x => x.DyingConsumptionDetailID == details.DyingConsumptionDetailID).FirstOrDefault();
                            ID = details.DyingConsumptionDetailID;
                            consumptionDetails.ConsumptionChemicalMID = details.ConsumptionChemicalMID;
                        }


                        consumptionDetails.DyingConsumptionDetailID = ID;
                        consumptionDetails.DyingConsumptionID = consumption.DyingConsumptionID;
                        consumptionDetails.MachinePartID = details.MachinePartID;
                        consumptionDetails.OperationID = details.OperationID;
                        consumptionDetails.Temp = details.Temp;

                        consumptionDetails.Volume = details.Volume;
                        consumptionDetails.UnitID = details.UnitID;
                        consumptionDetails.CompanyID = commonEntity.loggedCompnyID ?? 0;
                        consumptionDetails.IsDeleted = details.IsDeleted;
                        if (details.IsDeleted == true)
                        {
                            consumptionDetails.DeleteBy = commonEntity.loggedUserID;
                            consumptionDetails.DeleteOn = serverDate;
                            consumptionDetails.DeletePc = HostID;
                            consumptionDetails.IsDeleted = true;
                            DeleteAllChild = true;
                        }
                        else
                        {
                            consumptionDetails.UpdateBy = commonEntity.loggedUserID;
                            consumptionDetails.UpdateOn = serverDate;
                            consumptionDetails.UpdatePc = HostID;
                        }

                        #region Chemical Master
                        ID = 0;
                        var chemical = new PrdDyingConsumptionChemicalM();
                        chemical.TotalChemical = details.PrdDyingConsumptionChemicalM.TotalChemical;
                        if (details.PrdDyingConsumptionChemicalM.EntityState == "Insert")
                        {
                            ID = ++UChecmicalID;  
                            chemical.ConsumptionChemicaMlID = ID;


                            chemical.CreateBy = commonEntity.loggedUserID;
                            chemical.CreateOn = serverDate;
                            chemical.CreatePc = HostID;
                            chemical.CompanyID = commonEntity.loggedCompnyID ?? 0;
                        }
                        else if (details.PrdDyingConsumptionChemicalM.EntityState == "Update")
                        {
                            chemical.ConsumptionChemicaMlID = (long)details.ConsumptionChemicalMID;
                            chemical = PrdDyingConsumptionChemicalM_IEF.GetAll().Where(x => x.ConsumptionChemicaMlID == details.ConsumptionChemicalMID).FirstOrDefault();
                            
                            ID = chemical.ConsumptionChemicaMlID;
                            if (chemical.IsDeleted == true)
                            {
                                chemical.DeleteBy = commonEntity.loggedUserID;
                                chemical.DeleteOn = serverDate;
                                chemical.DeletePc = HostID;
                            }
                            else
                            {
                                chemical.UpdateBy = commonEntity.loggedUserID;
                                chemical.UpdateOn = serverDate;
                                chemical.UpdatePc = HostID;
                            }

                            chemical.CompanyID = commonEntity.loggedCompnyID ?? 0;
                            chemical.IsDeleted = details.PrdDyingConsumptionChemicalM.IsDeleted;
                            chemical.TotalChemical = 0;
                            if (DeleteAllChild)
                            {
                                chemical.DeleteBy = commonEntity.loggedUserID;
                                chemical.DeleteOn = serverDate;
                                chemical.DeletePc = HostID;
                                chemical.IsDeleted = true;
                            }

                        }
                        consumptionDetails.ConsumptionChemicalMID = chemical.ConsumptionChemicaMlID;
                    
                        #endregion Checmical Master
                        #region Chemical Details
                        foreach (PrdDyingConsumptionChemical item in details.PrdDyingConsumptionChemicalM.PrdDyingConsumptionChemicals)
                        {
                            var modifiedInsertion = true;
                            var chemicalDetails = new PrdDyingConsumptionChemical();
                            try
                            {
                              var   chemicalDetailTemp = PrdDyingConsumptionChemical_IEF.GetAll().Where(x => x.ConsumptionChemicaMlID == details.ConsumptionChemicalMID && x.ChemicalID == item.ChemicalID).FirstOrDefault();
                                //  chemicalDetails.ConsumptionChemicalID = new ERP_Entities().PrdDyingConsumptionChemicals.Where(x => x.ConsumptionChemicaMlID == chemicalDetails.ConsumptionChemicaMlID && x.ChemicalID == chemicalDetails.ChemicalID && x.IsDeleted == false).FirstOrDefault().ConsumptionChemicalID;// item.ConsumptionChemicalID;
                               if(chemicalDetailTemp==null)
                                {
                                    chemicalDetails.ConsumptionChemicaMlID = item.ConsumptionChemicaMlID;
                                    chemicalDetails.ConsumptionChemicaMlID = consumptionDetails.ConsumptionChemicalMID ?? 0;
                                    chemicalDetails.ChemicalID = item.ChemicalID;
                                    chemicalDetails.DepartmentID = commonEntity.loggedUserDepartmentID;
                                    chemicalDetails.BatchID = item.BatchID;
                                    chemicalDetails.SupplierID = item.SupplierID;
                                    chemicalDetails.Qty = item.Qty??0;
                                    chemicalDetails.UnitPrice = item.UnitPrice??0;
                                    chemicalDetails.Amount = item.UnitPrice??0 * item.Qty??0;
                                    chemicalDetails.ConsumptionDate = master.Date;

                                    chemical.TotalChemical = Convert.ToInt32( chemical.TotalChemical) + Convert.ToInt32(chemicalDetails.Qty);
                                    chemicalDetails.UnitID = item.UnitID;
                                    ID = ++UChecmicalDetailID;
                                    chemicalDetails.ConsumptionChemicalID = ID;
                                    chemicalDetails.TransactionTypeID = commonEntity.currentTransactionTypeID;
                                    chemicalDetails.CreateBy = commonEntity.loggedUserID;
                                    chemicalDetails.CreateOn = serverDate;
                                    chemicalDetails.CreatePc = HostID;
                                    modifiedInsertion = false;
                                    item.EntityState = "Insert";
                                }
                               else
                                {
                                    chemicalDetails.ConsumptionChemicaMlID = chemicalDetailTemp.ConsumptionChemicaMlID;
                                    chemicalDetails.ChemicalID = chemicalDetailTemp.ChemicalID;
                                    chemicalDetails.ConsumptionChemicalID = chemicalDetailTemp.ConsumptionChemicalID;
                                    chemicalDetails.ChemicalID = item.ChemicalID;
                                    chemicalDetails.DepartmentID = commonEntity.loggedUserDepartmentID;
                                    chemicalDetails.BatchID = item.BatchID;
                                    chemicalDetails.SupplierID = item.SupplierID;
                                    chemicalDetails.Qty = item.Qty??0;
                                   chemicalDetails.UnitPrice = item.UnitPrice??0;
                                    chemicalDetails.Amount = item.UnitPrice??0 * item.Qty??0;
                                    chemicalDetails.ConsumptionDate = master.Date;
                                    chemical.TotalChemical = Convert.ToInt32(chemical.TotalChemical) + Convert.ToInt32(chemicalDetails.Qty);
                                    chemicalDetails.UnitID = item.UnitID;
                                    chemicalDetails.TransactionTypeID = commonEntity.currentTransactionTypeID;
                                    chemicalDetails.CreateBy = chemicalDetailTemp.CreateBy;
                                    chemicalDetails.CreateOn = chemicalDetailTemp.CreateOn;
                                    chemicalDetails.CreatePc = chemicalDetailTemp.CreatePc;

                                }
                            }
                            catch (Exception)
                            {
                               
                            }
                            if (item.EntityState == "Insert")
                            {
                                ID = ++UChecmicalDetailID; 
                                chemicalDetails.ConsumptionChemicalID = ID;
                                chemicalDetails.CreateBy = commonEntity.loggedUserID;
                                chemicalDetails.CreateOn = serverDate;
                                chemicalDetails.CreatePc = HostID;
                            }
                            else if (item.EntityState == "Update")
                            {
                                //  var chemicalID = new ERP_Entities().PrdDyingConsumptionChemicals.Where(x => x.ConsumptionChemicaMlID == chemicalDetails.ConsumptionChemicaMlID && x.ChemicalID == chemicalDetails.ChemicalID && x.IsDeleted == false).FirstOrDefault().ConsumptionChemicalID;
                                
                                if (item.IsDeleted == true)
                                {
                                    chemicalDetails.DeleteBy = commonEntity.loggedUserID;
                                    chemicalDetails.DeleteOn = serverDate;
                                    chemicalDetails.DeletePc = HostID;
                                }
                                else
                                {
                                    if(modifiedInsertion)
                                    {
                                        chemicalDetails.UpdateBy = commonEntity.loggedUserID;
                                        chemicalDetails.UpdateOn = serverDate;
                                        chemicalDetails.UpdatePc = HostID;
                                    }
                                   
                                }
                            }
                            chemicalDetails.IsDeleted = item.IsDeleted;
                            if(item.EntityState == "Update" && item.Qty<=0)
                            {
                                chemicalDetails.IsDeleted = true;
                                chemicalDetails.DeleteBy = commonEntity.loggedUserID;
                                chemicalDetails.DeleteOn = serverDate;
                                chemicalDetails.DeletePc = HostID;
                            }

                            if (DeleteAllChild)
                            {
                                chemicalDetails.DeleteBy = commonEntity.loggedUserID;
                                chemicalDetails.DeleteOn = serverDate;
                                chemicalDetails.DeletePc = HostID;
                                chemicalDetails.IsDeleted = true;
                            }
                            chemicalDetails.CompanyID = commonEntity.loggedCompnyID??0;

                            chemicalDetails.DepartmentID = null;
                            if (item.EntityState == "Update") UpdateChemicalList.Add(chemicalDetails);
                            else if(item.EntityState=="Insert" && item.Qty>0)
                                InsertionChemicalList.Add(chemicalDetails);
                        }
                        #endregion Checmical Details


                        if (details.PrdDyingConsumptionChemicalM.EntityState == "Update") UpdateChemicalMList.Add(chemical);
                        else InsertionChemicaMlList.Add(chemical);

                        if (details.EntityState == "Update") UpdateConsumptionDetailsList.Add(consumptionDetails);
                        else InsertionConsumptionDetailsList.Add(consumptionDetails);

                        DeleteAllChild = false;
                    }

                    #endregion Details End
                    PrdDyingConsumptionMaster_IEF.Update(consumption);
                    PrdDyingConsumptionMaster_IEF.Save();

                    PrdDyingConsumptionChemicalM_IEF.UpdateList(UpdateChemicalMList);
                    PrdDyingConsumptionChemicalM_IEF.Save();
                    PrdDyingConsumptionChemicalM_IEF.InsertList(InsertionChemicaMlList);
                    PrdDyingConsumptionChemicalM_IEF.Save();

                    PrdDyingConsumptionDetail_IEF.UpdateList(UpdateConsumptionDetailsList);
                    PrdDyingConsumptionDetail_IEF.Save();
                    PrdDyingConsumptionDetail_IEF.InsertList(InsertionConsumptionDetailsList);
                    PrdDyingConsumptionDetail_IEF.Save();

                    PrdDyingConsumptionChemical_IEFU.UpdateList(UpdateChemicalList);
                    PrdDyingConsumptionChemical_IEFU.Save();
                    PrdDyingConsumptionChemical_IEFI.InsertList(InsertionChemicalList);
                    PrdDyingConsumptionChemical_IEFI.Save();

                    PrdDyingConsumptionDetail_IEF.updateMaxID("PrdDyingConsumptionDetail", UdetailsID);
                    PrdDyingConsumptionChemicalM_IEF.updateMaxID("PrdDyingConsumptionChemicalM", UChecmicalID);
                    PrdDyingConsumptionChemical_IEF.updateMaxID("PrdDyingConsumptionChemical", UChecmicalDetailID);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }

            }
            return consumption;
        }
        public int SaveChemicalConsumption(PrdDyingConsumptionMaster master, List<PrdDyingConsumptionDetail> detailsList, UserCommonEntity commonEntity)
        {
            int result = 0;
            try
            {
                //var watch = System.Diagnostics.Stopwatch.StartNew();
                //watch.Stop();
                //var elapsedMs = watch.ElapsedMilliseconds;
                PrdDyingConsumptionMaster consumption = null;
                if(master.EntityState == "Insert")
                {
                    consumption = this.BakeMaster(master, detailsList, commonEntity);
                }
                else
                {
                    consumption = this.BakeMasterUpdate(master, detailsList, commonEntity); 
                }
               
             
                result = (int)consumption.DyingConsumptionID;
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
        #endregion  Save
        #region Get Consumption
        public List<vmPrdDyingConsumptionMaster> GetAllConsumption(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            List<vmPrdDyingConsumptionMaster> returnList = new List<vmPrdDyingConsumptionMaster>();
            recordsTotal = 0;
            try
            {
                using (ERP_Entities context = new ERP_Entities())
                {
                    returnList = (from consumption in context.PrdDyingConsumptionMasters
                                  join set in context.PrdDyingMRRSets on consumption.SetID equals set.DyingSetID into setGroup
                                  from sg in setGroup.DefaultIfEmpty()
                                  join art in context.CmnItemMasters on consumption.ItemID equals art.ItemID into artGroup
                                  from ag in artGroup.DefaultIfEmpty()
                                  join operation in context.PrdDyingOperations on consumption.OperationID equals operation.OperationID into operationGroup
                                  from og in operationGroup.DefaultIfEmpty()
                                  where consumption.CompanyID == objcmnParam.loggedCompany
                                  select new vmPrdDyingConsumptionMaster
                                  {
                                      DyingConsumptionID = consumption.DyingConsumptionID,
                                      DyingConsumptionNo = consumption.DyingConsumptionNo,
                                      SetID = consumption.SetID,
                                      SetNo = sg.DyingSetNo,
                                      ItemID = consumption.ItemID,
                                      ArticleNo = ag.ArticleNo,
                                      Date = consumption.Date,
                                      IndigoStart = consumption.IndigoStart,
                                      IndigoStop = consumption.IndigoStop,
                                      BlackStart = consumption.BlackStart,
                                      BlackStop = consumption.BlackStop,
                                      OperationID = consumption.OperationID,
                                      OperationName = og.OperationName,
                                      CompanyID = consumption.CompanyID,
                                      CreateBy = consumption.CreateBy,
                                      CreateOn = consumption.CreateOn,
                                      CreatePc = consumption.CreatePc,
                                      UpdateBy = consumption.UpdateBy,
                                      UpdateOn = consumption.UpdateOn,
                                      UpdatePc = consumption.UpdatePc,
                                      IsDeleted = consumption.IsDeleted,
                                      DeleteBy = consumption.DeleteBy,
                                      DeleteOn = consumption.DeleteOn,
                                      DeletePc = consumption.DeletePc
                                  }).ToList();
                    recordsTotal = returnList.Count();
                    returnList = returnList.OrderByDescending(x => x.DyingConsumptionID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnList;
        }

        public vmPrdDyingConsumptionMaster GetConsumptionByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging,int? ConsumptionID)
        {
            vmPrdDyingConsumptionMaster returnList = new vmPrdDyingConsumptionMaster();
            try
            {
                using (ERP_Entities context = new ERP_Entities())
                {
                    returnList = (from consumption in context.PrdDyingConsumptionMasters
                                  join set in context.PrdDyingMRRSets on consumption.SetID equals set.DyingSetID into setGroup
                                  from sg in setGroup.DefaultIfEmpty()
                                  join art in context.CmnItemMasters on consumption.ItemID equals art.ItemID into artGroup
                                  from ag in artGroup.DefaultIfEmpty()
                                  join operation in context.PrdDyingOperations on consumption.OperationID equals operation.OperationID into operationGroup
                                  from og in operationGroup.DefaultIfEmpty()
                                  where consumption.CompanyID == companyID && consumption.DyingConsumptionID==ConsumptionID
                                  select new vmPrdDyingConsumptionMaster
                                  {
                                      DyingConsumptionID = consumption.DyingConsumptionID,
                                      DyingConsumptionNo = consumption.DyingConsumptionNo,
                                      SetID = consumption.SetID,
                                      SetNo = sg.DyingSetNo,
                                      ItemID = consumption.ItemID,
                                      ArticleNo = ag.ArticleNo,
                                      //Date = consumption.Date,
                                      IndigoStart = consumption.IndigoStart,
                                      IndigoStop = consumption.IndigoStop,
                                      BlackStart = consumption.BlackStart,
                                      BlackStop = consumption.BlackStop,
                                      OperationID = consumption.OperationID,
                                      OperationName = og.OperationName,
                                      Date=consumption.Date,
                                      CompanyID = consumption.CompanyID,
                                      //CreateBy = consumption.CreateBy,
                                      //CreateOn = consumption.CreateOn,
                                      //CreatePc = consumption.CreatePc,
                                      //UpdateBy = consumption.UpdateBy,
                                      //UpdateOn = consumption.UpdateOn,
                                      //UpdatePc = consumption.UpdatePc,
                                      IsDeleted = consumption.IsDeleted,
                                      //DeleteBy = consumption.DeleteBy,
                                      //DeleteOn = consumption.DeleteOn,
                                      //DeletePc = consumption.DeletePc
                                  }).ToList().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnList;
        }
        public List<vmPrdDyingConsumptionDetail> GetChemicalConsumptionByID(int? companyID, int? loggedUser, int? ConsumptionID)
        {
            List<vmPrdDyingConsumptionDetail> list = new List<vmPrdDyingConsumptionDetail>();
            try
            {
                using (ERP_Entities contex = new ERP_Entities())
                {
                    list = (from details in contex.PrdDyingConsumptionDetails
                            where details.DyingConsumptionID == ConsumptionID

                            select new vmPrdDyingConsumptionDetail {
                                DyingConsumptionDetailID = details.DyingConsumptionDetailID,
                            } ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString()); 
            }
            return list;
        }
        
        public IEnumerable<object> GetChemicalConsumptionDetailsByID(int? companyID, int? loggedUser, int? ConsumptionID)
        {
            ERP_Entities contex = new ERP_Entities();
            var list = (from details in contex.PrdDyingConsumptionDetails
                        where details.DyingConsumptionID == ConsumptionID
                        select new
                        {
                            DyingConsumptionDetailID = details.DyingConsumptionDetailID,
                            ConsumptionChemicalMID = details.ConsumptionChemicalMID,
                            ConsumptionChemicalSum = details.PrdDyingConsumptionChemicalM.TotalChemical,
                            DyingConsumptionID = details.DyingConsumptionID,
                            EntityState = "Update",
                            IsDeleted=details.IsDeleted,
                            MachinePartID=details.MachinePartID,
                            OperationID=details.OperationID,

                            Temp=details.Temp,
                            UOMID=details.UnitID,
                            Volume=details.Volume,

                        PrdDyingConsumptionChemicalM =
                        (
                            from dt in contex.PrdDyingConsumptionDetails
                            where dt.DyingConsumptionDetailID == details.DyingConsumptionDetailID
                            select new
                            {
                                DyingConsumptionChemicalMID= details.ConsumptionChemicalMID,
                                ConsumptionChemicalMID= dt.PrdDyingConsumptionChemicalM.ConsumptionChemicaMlID,
                                EntityState="Update",
                                IsDeleted=dt.PrdDyingConsumptionChemicalM.IsDeleted,
                                TotalChemical=dt.PrdDyingConsumptionChemicalM.TotalChemical,
                                PrdDyingConsumptionChemicals =
                                (
                                     from chemical in contex.PrdDyingConsumptionChemicals
                                     join CUOM in contex.CmnUOMs on chemical.UnitID equals CUOM.UOMID
                                     where chemical.ConsumptionChemicaMlID == details.ConsumptionChemicalMID
                                     select new 
                                     {
                                         SupplierID = chemical.SupplierID,
                                         BatchID=chemical.BatchID,
                                         ChemicalID=chemical.ChemicalID,
                                         ConsumptionChemicalID=chemical.ConsumptionChemicalID,
                                         ConsumptionChemicalMID=chemical.ConsumptionChemicaMlID,
                                         EntityState = "Update",
                                         Qty= chemical.Qty,
                                         UnitID= chemical.UnitID,
                                         UnitPrice=chemical.UnitPrice,
                                         Amount=chemical.Amount,                                         
                                         UOMName = CUOM.UOMName,                                         
                                         CurrentStock = contex.InvStockMasters.Where(x => x.ItemID == chemical.ChemicalID && x.BatchID == chemical.BatchID && x.DepartmentID == chemical.DepartmentID && x.CompanyID == chemical.CompanyID && x.UOMID == chemical.UnitID && (chemical.SupplierID == null || chemical.SupplierID == 0 ? true : x.SupplierID == chemical.SupplierID)).Select(x => x.CurrentStock).FirstOrDefault(),
                                         Batch = (from CB in contex.CmnBatches
                                                  where CB.ItemID == chemical.ChemicalID
                                                  //where B.ItemTypeID != null
                                                  select new vmCmnBatch { BatchID = CB.BatchID, BatchNo = CB.BatchNo }).Distinct().ToList(),
                                         Supplier = (from CU in contex.CmnUsers
                                                     join IM in contex.InvStockMasters on CU.UserID equals IM.SupplierID
                                                     where IM.ItemID == chemical.ChemicalID
                                                     //where B.ItemTypeID != null
                                                     select new vmBallInfo { SupplierID = IM.SupplierID, SupplierName = CU.UserFullName }).Distinct().ToList()

                                     }
                                )
                            }
                        ).FirstOrDefault(),
                    }).ToList();

            return list;
        }

        #endregion Get Consumption
        #region Delete 
        public Int64 DeleteConsumption(vmCmnParameters commonEntity)
        {
            Int64  result = 0;
            try
            {
                using (var transaction = new TransactionScope())
                {

                    PrdDyingConsumptionMaster_IEF = new PrdDyingConsumptionMaster_EF();
                    PrdDyingConsumptionDetail_IEF = new PrdDyingConsumptionDetail_EF();
                    PrdDyingConsumptionChemicalM_IEF = new PrdDyingConsumptionChemicalM_EF();
                    PrdDyingConsumptionChemical_IEF = new PrdDyingConsumptionChemical_EF();

                    PrdDyingConsumptionMaster item = new PrdDyingConsumptionMaster();
                    List<PrdDyingConsumptionChemicalM> InsertionChemicaMlList = new List<PrdDyingConsumptionChemicalM>();
                    List<PrdDyingConsumptionChemical> InsertionChemicalList = new List<PrdDyingConsumptionChemical>();
                    List<PrdDyingConsumptionDetail> InsertionConsumptionDetailsList = new List<PrdDyingConsumptionDetail>();

                    DateTime serverDate = DateTime.Now;
                    String HostID = HostService.GetIP();
                    ERP_Entities context = new ERP_Entities();
                    serverDate = context.Database.SqlQuery<DateTime>("SELECT getdate()").AsEnumerable().First();
                    item = PrdDyingConsumptionMaster_IEF.GetAll().Where(x => x.DyingConsumptionID == commonEntity.id && x.IsDeleted == false).FirstOrDefault();

                    item.IsDeleted = true;
                    item.DeleteOn = serverDate;
                    item.DeletePc = HostID;
                    item.DeleteBy = commonEntity.loggedCompany;

                    InsertionConsumptionDetailsList = PrdDyingConsumptionDetail_IEF.GetAll().Where(x => x.DyingConsumptionID == commonEntity.id && x.IsDeleted == false).ToList();
                    for (int i = 0; i < InsertionConsumptionDetailsList.Count(); i++)
                    {
                        InsertionConsumptionDetailsList[i].IsDeleted = true;
                        InsertionConsumptionDetailsList[i].DeleteOn = serverDate;
                        InsertionConsumptionDetailsList[i].DeletePc = HostID;
                        InsertionConsumptionDetailsList[i].DeleteBy = commonEntity.loggeduser;
                        var chemical = PrdDyingConsumptionChemicalM_IEF.GetAll().Where(x => x.ConsumptionChemicaMlID == InsertionConsumptionDetailsList[i].ConsumptionChemicalMID && x.IsDeleted==false).FirstOrDefault();
                        var chemicalDetails = PrdDyingConsumptionChemical_IEF.GetAll().Where(x => x.ConsumptionChemicaMlID == InsertionConsumptionDetailsList[i].ConsumptionChemicalMID && x.IsDeleted == false ).ToList();

                        for (int a = 0; a < chemicalDetails.Count(); a++)
                        {
                            chemicalDetails[a].IsDeleted = true;
                            chemicalDetails[a].DeleteOn = serverDate;
                            chemicalDetails[a].DeletePc = HostID;
                            chemicalDetails[a].DeleteBy = commonEntity.loggeduser;
                        }
                        chemical.IsDeleted = true;
                        chemical.DeleteOn = serverDate;
                        chemical.DeletePc = HostID;
                        chemical.DeleteBy = commonEntity.loggeduser;

                        InsertionChemicaMlList.Add(chemical);
                        InsertionChemicalList.AddRange(chemicalDetails);
                    }

                    //PrdDyingConsumptionChemical_IEF.UpdateList(InsertionChemicalList);
                    //PrdDyingConsumptionChemical_IEF.Save();

                    //PrdDyingConsumptionDetail_IEF.UpdateList(InsertionConsumptionDetailsList);
                    //PrdDyingConsumptionDetail_IEF.Save();

                    //PrdDyingConsumptionChemicalM_IEF.UpdateList(InsertionChemicaMlList);
                    //PrdDyingConsumptionChemicalM_IEF.Save();

                    //PrdDyingConsumptionMaster_IEF.Update(item);
                    //PrdDyingConsumptionMaster_IEF.Save();


                    PrdDyingConsumptionChemical_IEF.UpdateList(InsertionChemicalList);
                    PrdDyingConsumptionChemical_IEF.Save();

                    PrdDyingConsumptionDetail_IEF.UpdateList(InsertionConsumptionDetailsList);
                    PrdDyingConsumptionDetail_IEF.Save();

                    PrdDyingConsumptionChemicalM_IEF.UpdateList(InsertionChemicaMlList);
                    PrdDyingConsumptionChemicalM_IEF.Save();

                    PrdDyingConsumptionMaster_IEF.Update(item);
                    PrdDyingConsumptionMaster_IEF.Save();

                    result = item.DyingConsumptionID;
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return result;
        }
        #endregion Delete
        #region Dispose
        ~DyingPRDChemicalConsumptionMgt()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
            //Dispose Method
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        #endregion Dispose
    }

}

