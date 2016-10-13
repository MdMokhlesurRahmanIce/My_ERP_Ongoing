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
    public class DefectTypeMgt : iDefectTypeMgt
    {
        private iGenericFactory_EF<PrdDefectType> GenericFactory_EF_PrdDefectType = null;
        private iGenericFactory_EF<PrdDefectList> GenericFactory_EF_PrdDefectList = null;

        /// No CompanyID Provided
        public IEnumerable<PrdDefectType> GetDefectType(vmCmnParameters objcmnParam)
        {
            GenericFactory_EF_PrdDefectType = new PrdDefectType_EF();
            IEnumerable<PrdDefectType> objDefectType = null;
            try
            {
                objDefectType = GenericFactory_EF_PrdDefectType.GetAll().Select(m => new PrdDefectType
                {
                    DefectTypeID = m.DefectTypeID,
                    DefectTypeName = m.DefectTypeName,
                    IsDeleted = m.IsDeleted,
                    CompanyID = m.CompanyID
                }).
                    Where(m => m.CompanyID == objcmnParam.loggedCompany && m.IsDeleted == false).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objDefectType;
        }

        public IEnumerable<PrdDefectList> GetDefectTypeInfo(vmCmnParameters cmnParam, out int recordsTotal)
        {
            GenericFactory_EF_PrdDefectList = new PrdDefectList_EF();
            IEnumerable<PrdDefectList> objDefectTypeMaster = null;
            recordsTotal = 0;
            try
            {
                objDefectTypeMaster = GenericFactory_EF_PrdDefectList.GetAll()
                    .Select(m => new PrdDefectList
                    {
                        DefectID = m.DefectID,
                        DefectTypeID = m.DefectTypeID,
                        DefectName = m.DefectName,
                        DefectNo = m.DefectNo,
                        Description = m.Description,
                        CompanyID = m.CompanyID
                    }).Where(m => m.CompanyID == cmnParam.loggedCompany).ToList();
                recordsTotal = objDefectTypeMaster.Count();
                objDefectTypeMaster = objDefectTypeMaster.OrderBy(x => x.DefectTypeID).Skip(cmnParam.pageNumber).Take(cmnParam.pageSize).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDefectTypeMaster;
        }

        public string SaveUpdateDefectType(PrdDefectList DefectTypeInfo, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdDefectList = new PrdDefectList_EF();
                long MainId = 0; string CustomNo = string.Empty, DefectNo = string.Empty;
                var UDefectTypeInfo = new PrdDefectList();

                try
                {
                    if (DefectTypeInfo.DefectID > 0)
                    {
                        UDefectTypeInfo = GenericFactory_EF_PrdDefectList.GetAll().Where(x => x.DefectID == DefectTypeInfo.DefectID).FirstOrDefault();
                        UDefectTypeInfo.DefectName = DefectTypeInfo.DefectName;
                        UDefectTypeInfo.DefectTypeID = DefectTypeInfo.DefectTypeID;
                        UDefectTypeInfo.DefectNo = DefectTypeInfo.DefectNo;
                        UDefectTypeInfo.Description = DefectTypeInfo.Description;

                        UDefectTypeInfo.CompanyID = objcmnParam.loggedCompany;
                        UDefectTypeInfo.UpdateBy = objcmnParam.loggeduser;
                        UDefectTypeInfo.UpdateOn = DateTime.Now;
                        UDefectTypeInfo.UpdatePc = HostService.GetIP();
                    }
                    else
                    {
                        MainId = Convert.ToInt16(GenericFactory_EF_PrdDefectList.getMaxID("PrdDefectList"));
                        //CustomNo = GenericFactory_EF_PrdDefectList.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                        //if (CustomNo == null || CustomNo == "")
                        //{
                        //    BWSNos = MainId.ToString();
                        //}
                        //else
                        //{
                        //    BWSNos = CustomNo;
                        //}

                        UDefectTypeInfo = new PrdDefectList()
                        {
                            DefectID = (int)MainId,
                            DefectName = DefectTypeInfo.DefectName,
                            DefectTypeID = DefectTypeInfo.DefectTypeID,
                            DefectNo = DefectTypeInfo.DefectNo,
                            Description = DefectTypeInfo.Description,
                            IsDeleted = false,

                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc = HostService.GetIP()
                        };
                    }

                    if (DefectTypeInfo.DefectID > 0)
                    {
                        GenericFactory_EF_PrdDefectList.Update(UDefectTypeInfo);
                        GenericFactory_EF_PrdDefectList.Save();
                    }
                    else
                    {
                        GenericFactory_EF_PrdDefectList.Insert(UDefectTypeInfo);
                        GenericFactory_EF_PrdDefectList.Save();
                        GenericFactory_EF_PrdDefectList.updateMaxID("PrdDefectList", Convert.ToInt64(MainId));
                    }
                    transaction.Complete();
                    result = UDefectTypeInfo.DefectNo;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public string DeleteUpdateDefectList(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (TransactionScope transaction = new TransactionScope())
            {
                GenericFactory_EF_PrdDefectList = new PrdDefectList_EF();
                var PrdDefectLists = new PrdDefectList();
                try
                {
                    PrdDefectLists = GenericFactory_EF_PrdDefectList.GetAll().Where(x => x.DefectID == objcmnParam.id).FirstOrDefault();
                    PrdDefectLists.IsDeleted = true;
                    PrdDefectLists.CompanyID = objcmnParam.loggedCompany;
                    PrdDefectLists.DeleteBy = objcmnParam.loggeduser;
                    PrdDefectLists.DeleteOn = DateTime.Now;
                    PrdDefectLists.DeletePc = HostService.GetIP();

                    GenericFactory_EF_PrdDefectList.Update(PrdDefectLists);
                    GenericFactory_EF_PrdDefectList.Save();

                    transaction.Complete();
                    result = PrdDefectLists.DefectNo;
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
