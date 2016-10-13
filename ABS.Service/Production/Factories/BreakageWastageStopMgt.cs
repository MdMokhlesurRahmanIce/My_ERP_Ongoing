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
    public class BreakageWastageStopMgt : iBreakageWastageStopMgt
    {
        private iGenericFactory_EF<PrdBWSlist> GenericFactory_EF_PrdBWSlist = null;

        /// No CompanyID Provided
        public IEnumerable<PrdBWSlist> GetBWSInfo(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_EF_PrdBWSlist = new PrdBWSlist_EF();
            IEnumerable<PrdBWSlist> objBWSMaster = null;
            recordsTotal = 0;
            try
            {
                objBWSMaster = GenericFactory_EF_PrdBWSlist.GetAll()
                    .Select(m => new PrdBWSlist
                    {
                        BWSID = m.BWSID,
                        BWSNo = m.BWSNo,
                        BWSName = m.BWSName,
                        BWSType = m.BWSType,
                        DepartmentID = m.DepartmentID,
                        Description = m.Description,
                        CompanyID = m.CompanyID,
                        IsDeleted=m.IsDeleted
                    }).Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList();
                recordsTotal = objBWSMaster.Count();
                objBWSMaster = objBWSMaster.OrderByDescending(x => x.BWSID).Skip(objcmnParam.pageNumber).Take(objcmnParam.pageSize).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBWSMaster;
        }

        public string SaveUpdateBWS(PrdBWSlist BWSInfo, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdBWSlist = new PrdBWSlist_EF();
                long MainId = 0; string CustomNo = string.Empty, BWSNos = string.Empty;
                var UBWSInfo = new PrdBWSlist();
                try
                {
                    if (BWSInfo.BWSID > 0)
                    {
                        UBWSInfo = GenericFactory_EF_PrdBWSlist.GetAll().Where(x => x.BWSID == BWSInfo.BWSID).FirstOrDefault();
                        UBWSInfo.BWSName = BWSInfo.BWSName;
                        UBWSInfo.BWSType = BWSInfo.BWSType;
                        UBWSInfo.DepartmentID = BWSInfo.DepartmentID;
                        UBWSInfo.Description = BWSInfo.Description;

                        UBWSInfo.CompanyID = objcmnParam.loggedCompany;
                        UBWSInfo.UpdateBy = objcmnParam.loggeduser;
                        UBWSInfo.UpdateOn = DateTime.Now;
                        UBWSInfo.UpdatePc = HostService.GetIP();
                        BWSNos = UBWSInfo.BWSNo;
                    }
                    else
                    {
                        MainId = Convert.ToInt16(GenericFactory_EF_PrdBWSlist.getMaxID("PrdBWSlist"));
                        CustomNo = GenericFactory_EF_PrdBWSlist.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        if (CustomNo == null || CustomNo == "")
                        {
                            BWSNos = MainId.ToString();
                        }
                        else
                        {
                            BWSNos = CustomNo;
                        }

                        UBWSInfo = new PrdBWSlist()
                        {
                            BWSID = (int)MainId,
                            BWSNo = BWSNos.ToString(),
                            BWSName = BWSInfo.BWSName,
                            BWSType = BWSInfo.BWSType,
                            DepartmentID = BWSInfo.DepartmentID,
                            Description = BWSInfo.Description,
                            IsDeleted = false,

                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP()
                        };
                    }

                    if (BWSInfo.BWSID > 0)
                    {
                        GenericFactory_EF_PrdBWSlist.Update(UBWSInfo);
                        GenericFactory_EF_PrdBWSlist.Save();
                    }
                    else
                    {
                        GenericFactory_EF_PrdBWSlist.Insert(UBWSInfo);
                        GenericFactory_EF_PrdBWSlist.Save();
                        GenericFactory_EF_PrdBWSlist.updateMaxID("PrdBWSlist", Convert.ToInt64(MainId));
                        GenericFactory_EF_PrdBWSlist.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                    }
                    transaction.Complete();
                    result = BWSNos;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public string DeleteUpdatePrdBWSlist(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdBWSlist = new PrdBWSlist_EF();
                string BWSNos = string.Empty;
                var UBWSInfo = new PrdBWSlist();
                try
                {
                    UBWSInfo = GenericFactory_EF_PrdBWSlist.GetAll().Where(x => x.BWSID == objcmnParam.id).FirstOrDefault();
                    UBWSInfo.IsDeleted = true;
                    UBWSInfo.CompanyID = objcmnParam.loggedCompany;
                    UBWSInfo.DeleteBy = objcmnParam.loggeduser;
                    UBWSInfo.DeleteOn = DateTime.Now;
                    UBWSInfo.DeletePc = HostService.GetIP();
                    BWSNos = UBWSInfo.BWSNo;

                    GenericFactory_EF_PrdBWSlist.Update(UBWSInfo);
                    GenericFactory_EF_PrdBWSlist.Save();

                    transaction.Complete();
                    result = UBWSInfo.BWSNo;
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
