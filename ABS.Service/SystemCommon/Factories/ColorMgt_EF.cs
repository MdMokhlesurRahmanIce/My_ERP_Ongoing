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
using System.Collections;
using ABS.Models.ViewModel.SystemCommon;
using System.Transactions;
using ABS.Utility;

namespace ABS.Service.SystemCommon.Factories
{

    public class ColorMgt_EF : iColorMgt_EF
    {
        private ERP_Entities _ctxCmn = null;
        private iGenericFactory_EF<CmnItemColor> GenericFactory_EF_Color;
        //private iGenericFactory<CmnItemColor> GenericFactory_Color;
        private iGenericFactory<vmColor> GenericFactory_vmColor;

        /// No CompanyID Provided
        public IEnumerable<vmColor> GetColors(vmCmnParameters objcmnParam, out int recordsTotal)
        {
            GenericFactory_vmColor = new CmnItemColor_VM();
            IEnumerable<vmColor> objColors = null;
            recordsTotal = 0;
            string spQuery = string.Empty;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("CompanyID", objcmnParam.loggedCompany);
                    ht.Add("LoggedUser", objcmnParam.loggeduser);
                    ht.Add("PageNo", objcmnParam.pageNumber);
                    ht.Add("RowCountPerPage", objcmnParam.pageSize);
                    ht.Add("IsPaging", objcmnParam.IsPaging);

                    spQuery = "[Get_Color]";
                    objColors = GenericFactory_vmColor.ExecuteQuery(spQuery, ht);
                    recordsTotal = _ctxCmn.CmnItemColors.Where(x => x.CompanyID == objcmnParam.loggedCompany && x.IsDeleted == false).Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objColors;
        }

        public CmnItemColor GetColorsById(vmCmnParameters objcmnParam)
        {
            GenericFactory_EF_Color = new CmnItemColor_EF();
            CmnItemColor objColors = null;
            try
            {
                var TblColor = GenericFactory_EF_Color.FindBy(m => m.ItemColorID == objcmnParam.id);

                objColors = (from c in TblColor
                             where c.ItemColorID == objcmnParam.id
                             orderby c.ItemColorID descending
                             select new
                             {
                                 ItemColorID = c.ItemColorID,
                                 ColorName = c.ColorName,
                                 ColorCode = c.ColorCode,
                                 CustomCode = c.CustomCode
                             }).Select(x => new CmnItemColor
                             {
                                 ItemColorID = x.ItemColorID,
                                 ColorName = x.ColorName,
                                 ColorCode = x.ColorCode,
                                 CustomCode = x.CustomCode

                             }).FirstOrDefault();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objColors;
        }

        /// No CompanyID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public string SaveUpdateColors(vmColor model, vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                GenericFactory_EF_Color = new CmnItemColor_EF();
                string CustomNo = string.Empty, CustomCode = string.Empty; long ItemColorID = 0;
                //string strTblName = model.ToString().Substring(model.ToString().LastIndexOf(".") + 1);
                var Master = new CmnItemColor();
                try
                {
                    if (model.ItemColorID > 0)
                    {
                        Master = GenericFactory_EF_Color.GetAll().Where(x => x.ItemColorID == model.ItemColorID).FirstOrDefault();
                        Master.ColorCode = model.ColorCode;
                        Master.ColorName = model.ColorName;
                        Master.CompanyID = objcmnParam.loggedCompany;
                        Master.UpdateBy = objcmnParam.loggeduser;
                        Master.UpdateOn = DateTime.Now;
                        Master.UpdatePc = HostService.GetIP();
                        CustomCode = Master.CustomCode;
                    }
                    else
                    {
                        ItemColorID = Convert.ToInt16(GenericFactory_EF_Color.getMaxID("CmnItemColor"));
                        CustomNo = GenericFactory_EF_Color.getCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1); // // 1 for user id and 1 for db id --- work later
                        if (CustomNo == null)
                        {
                            CustomCode = ItemColorID.ToString();
                        }
                        else
                        {
                            CustomCode = CustomNo;
                        }

                        Master = new CmnItemColor
                        {
                            ItemColorID = Convert.ToInt32(ItemColorID),
                            ColorName = model.ColorName,
                            ColorCode = model.ColorCode,
                            CustomCode = CustomCode,

                            StatusID = 1,
                            CompanyID = objcmnParam.loggedCompany,
                            CreateBy = objcmnParam.loggeduser,
                            CreateOn = DateTime.Now,
                            CreatePc =  HostService.GetIP()
                        };
                    }

                    if (model.ItemColorID > 0)
                    {
                        GenericFactory_EF_Color.Update(Master);
                        GenericFactory_EF_Color.Save();
                    }
                    else
                    {
                        GenericFactory_EF_Color.Insert(Master);
                        GenericFactory_EF_Color.Save();
                        GenericFactory_EF_Color.updateMaxID("CmnItemColor", Convert.ToInt64(ItemColorID));
                        GenericFactory_EF_Color.updateCustomCode(objcmnParam.menuId, DateTime.Now, objcmnParam.loggedCompany, 1, 1);
                    }
                    transaction.Complete();
                    result = CustomCode;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public string DeleteUpdateColor(vmCmnParameters objcmnParam)
        {
            string result = string.Empty;
            using (var transaction = new TransactionScope())
            {
                GenericFactory_EF_Color = new CmnItemColor_EF();
                try
                {
                    var Master = GenericFactory_EF_Color.GetAll().Where(x => x.ItemColorID == objcmnParam.id).FirstOrDefault();
                    Master.CompanyID = objcmnParam.loggedCompany;
                    Master.DeleteBy = objcmnParam.loggeduser;
                    Master.DeleteOn = DateTime.Now;
                    Master.DeletePc = HostService.GetIP();
                    Master.IsDeleted = true;

                    GenericFactory_EF_Color.Update(Master);
                    GenericFactory_EF_Color.Save();

                    transaction.Complete();
                    result = Master.ColorName;
                }
                catch (Exception e)
                {
                    e.ToString();
                    result = "";
                }
            }
            return result;
        }

        public int DeleteColors(int Id)
        {
            GenericFactory_EF_Color = new CmnItemColor_EF();
            int result = 0;
            try
            {
                GenericFactory_EF_Color.Delete(m => m.ItemColorID == Id);
                GenericFactory_EF_Color.Save();
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
