using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Service.AllServiceClasses;
using ABS.Utility;

namespace ABS.Service.SystemCommon.Factories
{
    public class ItemSizeMgt : iItemSizeMgt
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnItemSize> GenericFactory_EF_ItemSize;

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public IEnumerable<CmnItemSize> GetItemSize(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnItemSize> objItemSize = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objItemSize = _ctxCmn.CmnItemSizes.ToList();// GenericFactory_EF_ItemSize.GetAll();
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemSize;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when to retive data through Entity</para>
        /// </summary>
        public IEnumerable<CmnItemSize> GetItemSizeById(int Id)
        {
            IEnumerable<CmnItemSize> objItemSize = null;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    objItemSize = _ctxCmn.CmnItemSizes.Where(m => m.SizeID == Id).ToList(); // GenericFactory_EF_ItemSize.FindBy(m => m.SizeID == Id);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemSize;
        }

        /// Static CompanyID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public int SaveUpdateItemSize(CmnItemSize model)
        {
            GenericFactory_EF_ItemSize = new CmnItemSize_EF();
            int result = 0;
            if (model.SizeID > 0)
            {
                try
                {
                    if (model.IsDeleted == false)
                    {
                        model.StatusID = 1;
                        model.CompanyID = 1;
                        model.UpdateBy = 1;
                        model.UpdateOn = DateTime.Today;
                        model.UpdatePc =  HostService.GetIP();

                        GenericFactory_EF_ItemSize.Update(model);
                        GenericFactory_EF_ItemSize.Save();
                    }
                    else
                    {
                        model.StatusID = 1;
                        model.CompanyID = 1;
                        model.DeleteBy = 1;
                        model.DeleteOn = DateTime.Today;
                        //model.DeletePc =  HostService.GetIP();                        

                        GenericFactory_EF_ItemSize.Update(model);
                        GenericFactory_EF_ItemSize.Save();
                    }
                    result = 1;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = 0;
                }
            }
            else
            {
                //long NextId = GenericFactory_EF_ItemSize.getMaxVal_int("SizeID", "CmnItemSize");
                string strTblName = model.ToString().Substring(model.ToString().LastIndexOf(".") + 1);
                object[] parameters = { strTblName };
                string spName = "[SPMaxRowID]";

                //long NextId = GenericFactory_EF_Color.getMaxValBySp_string("SPMaxRowID", strTblName);                
                long NextId = GenericFactory_EF_ItemSize.getMaxValBySp(spName, parameters);

                try
                {
                    model.SizeID = Convert.ToInt32(NextId);
                    model.StatusID = 1;
                    model.CompanyID = 1;
                    model.CreateBy = 1;
                    model.CreateOn = DateTime.Today;
                    model.CreatePc =  HostService.GetIP();

                    GenericFactory_EF_ItemSize.Insert(model);
                    GenericFactory_EF_ItemSize.Save();
                    result = 1;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// Delete Data From Database
        /// <para>Use it when delete data through ORM</para>
        /// </summary>
        public int DeleteItemSize(int Id)
        {
            GenericFactory_EF_ItemSize = new CmnItemSize_EF();
            int result = 0;
            try
            {
                GenericFactory_EF_ItemSize.Delete(m => m.SizeID == Id);
                GenericFactory_EF_ItemSize.Save();
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
