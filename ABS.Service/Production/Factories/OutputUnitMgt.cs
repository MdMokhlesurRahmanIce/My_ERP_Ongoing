using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
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
    public class OutputUnitMgt : iOutputUnitMgt
    {       
        private iGenericFactory_EF<CmnOrganogram> GenericFactoryFor_CmnOrganogram_EF = null;
        private iGenericFactory_EF<PrdOutputUnit> GenericFactory_EF_OutputUnit = null;

        public IEnumerable<CmnOrganogram> GetOutputName(vmCmnParameters objcmnParam)
        {
            GenericFactoryFor_CmnOrganogram_EF = new CmnOrganogram_EF();
            IEnumerable<CmnOrganogram> objOutputUnit = null;
            try
            {
                objOutputUnit = GenericFactoryFor_CmnOrganogram_EF.GetAll()
                    .Select(m => new CmnOrganogram
                    {
                        OrganogramID = m.OrganogramID,
                        ProcessOutput = m.ProcessOutput,
                        IsDeleted = m.IsDeleted
                    }).Where(m => m.IsDeleted == false && m.ProcessOutput != null).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objOutputUnit;
        }

        public IEnumerable<PrdOutputUnit> GetOutputUnitInfo(vmCmnParameters cmnParam, out int recordsTotal)
        {
            GenericFactory_EF_OutputUnit = new PrdOutputUnit_EF();
            IEnumerable<PrdOutputUnit> objOutputUnit = null;
            recordsTotal = 0;
            try
            {
                objOutputUnit = GenericFactory_EF_OutputUnit.GetAll().Select(m => new PrdOutputUnit
                {
                    OutputID = m.OutputID,
                    OutputNo = m.OutputNo,
                    OutputName = m.OutputName,
                    ProcessID = m.ProcessID,
                    Description = m.Description,
                    CompanyID = m.CompanyID
                }).Where(m => m.CompanyID == cmnParam.loggedCompany).ToList();
                recordsTotal = objOutputUnit.Count();
                objOutputUnit = objOutputUnit.OrderBy(x => x.ProcessID).Skip(cmnParam.pageNumber).Take(cmnParam.pageSize).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objOutputUnit;
        }

        public string SaveUpdateOutputUnit(PrdOutputUnit OutputUnitInfo, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_OutputUnit = new PrdOutputUnit_EF();
                long MainId = 0; string CustomNo = string.Empty, OutputNos = string.Empty;
                var UOutputUnit = new PrdOutputUnit();
                try
                {
                    if (OutputUnitInfo.OutputID > 0)
                    {
                        UOutputUnit = GenericFactory_EF_OutputUnit.GetAll().Where(x => x.OutputID == OutputUnitInfo.OutputID).FirstOrDefault();
                        UOutputUnit.OutputNo = OutputUnitInfo.OutputNo;
                        UOutputUnit.OutputName = OutputUnitInfo.OutputName;
                        UOutputUnit.ProcessID = OutputUnitInfo.ProcessID;
                        UOutputUnit.Description = OutputUnitInfo.Description;

                        UOutputUnit.CompanyID = objcmnParam.loggedCompany;
                        UOutputUnit.UpdateBy = objcmnParam.loggeduser;
                        UOutputUnit.UpdateOn = DateTime.Now;
                        UOutputUnit.UpdatePc = HostService.GetIP();
                        //OutputNos = UOutputUnit.OutputNo;
                    }
                    else
                    {
                        MainId = Convert.ToInt16(GenericFactory_EF_OutputUnit.getMaxID("PrdOutputUnit"));
                        //CustomNo = GenericFactory_EF_OutputUnit.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        //if (CustomNo == null || CustomNo == "")
                        //{
                        //    OutputNos = MainId.ToString();
                        //}
                        //else
                        //{
                        //    OutputNos = CustomNo;
                        //}

                        UOutputUnit = new PrdOutputUnit()
                        {
                            OutputID = (int)MainId,
                            OutputNo = OutputUnitInfo.OutputNo,//OutputNos.ToString(),
                            OutputName = OutputUnitInfo.OutputName,
                            ProcessID = OutputUnitInfo.ProcessID,
                            Description = OutputUnitInfo.Description,
                            IsDeleted = false,

                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP()
                        };
                    }


                    if (OutputUnitInfo.OutputID > 0)
                    {
                        GenericFactory_EF_OutputUnit.Update(UOutputUnit);
                        GenericFactory_EF_OutputUnit.Save();
                    }
                    else
                    {
                        GenericFactory_EF_OutputUnit.Insert(UOutputUnit);
                        GenericFactory_EF_OutputUnit.Save();
                        GenericFactory_EF_OutputUnit.updateMaxID("PrdOutputUnit", Convert.ToInt64(MainId));
                        GenericFactory_EF_OutputUnit.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                    }

                    transaction.Complete();
                    result = OutputUnitInfo.OutputNo;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public string DeleteUpdateOutPutList(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_OutputUnit = new PrdOutputUnit_EF();
                var UOutputUnit = new PrdOutputUnit();
                try
                {
                    UOutputUnit = GenericFactory_EF_OutputUnit.GetAll().Where(x => x.OutputID == objcmnParam.id).FirstOrDefault();
                    UOutputUnit.IsDeleted = true;
                    UOutputUnit.CompanyID = objcmnParam.loggedCompany;
                    UOutputUnit.DeleteBy = objcmnParam.loggeduser;
                    UOutputUnit.DeleteOn = DateTime.Now;
                    UOutputUnit.DeletePc = HostService.GetIP();

                    GenericFactory_EF_OutputUnit.Update(UOutputUnit);
                    GenericFactory_EF_OutputUnit.Save();

                    transaction.Complete();
                    result = UOutputUnit.OutputNo;
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
