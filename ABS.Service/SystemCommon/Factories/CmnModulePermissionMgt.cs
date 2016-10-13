using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ABS.Service.AllServiceClasses;

namespace ABS.Service.SystemCommon.Factories
{
    public class CmnModulePermissionMgt : iCmnModulePermissionMgt
    {
        private iGenericFactory_EF<CmnModulePermission> GenericFactory_EF_ModulePermission;
        private iGenericFactory_EF<CmnModule> GenericFactory_EF_ModuleP;

        /// No CompanyID Provided
        public int SaveModulePermission(CmnModulePermission model)
        {
            GenericFactory_EF_ModulePermission = new CmnModulePermission_EF();
            int result = 0;
            int NextID = GenericFactory_EF_ModulePermission.getMaxVal_int("ModulePermissionID", "CmnModulePermission");
            try
            {
                model.ModulePermissionID = NextID;
                GenericFactory_EF_ModulePermission.Insert(model);
                GenericFactory_EF_ModulePermission.Save();
                result = 1;
            }
            catch (Exception ex)
            {
                ex.ToString();
                result = 0;
            }

            return result;
        }

        public List<vmModulePermission> GetAllModulePermission(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_ModulePermission = new CmnModulePermission_EF();
            List<vmModulePermission> list = null;
            try
            {
                list = GenericFactory_EF_ModulePermission.GetAll()
                    .Select(x => new vmModulePermission
                    {
                        ModulePermissionID = x.ModulePermissionID,
                        ModuleID = x.ModuleID,
                        CustomCode = x.CustomCode,
                        CompanyID = x.CompanyID,
                        CompanyName = x.CmnCompany.CompanyName,
                        ModuleName = x.CmnModule.ModuleName
                    }).ToList();

            }
            catch (Exception)
            {

            }
            return list;
        }

        public int DeletePermission(int? Id)
        {
            GenericFactory_EF_ModulePermission = new CmnModulePermission_EF();
            int result = 0;
            try
            {
                GenericFactory_EF_ModulePermission.Delete(m => m.ModulePermissionID == Id);
                GenericFactory_EF_ModulePermission.Save();
                result = 1;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }

            return result;
        }
    }
}

