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

namespace ABS.Service.SystemCommon.Factories 
{
    public class UnitOfMeasurementMgt:iUnitOfMeasurementMgt
    {
        private iGenericFactory_EF<CmnUOM> GenericFactory_EF_UnitOfMeasurement = null;
        private iGenericFactory_EF<CmnUOMGroup> GenericFactory_EF_UOMGroup = null;

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when Get data</para>
        /// </summary>
        public IEnumerable<CmnUOM> GetUnitOfMeasurement(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_UnitOfMeasurement = new CmnUOM_EF();
            IEnumerable<CmnUOM> objUnitOfMeasurement = null;
            try
            {
                objUnitOfMeasurement = GenericFactory_EF_UnitOfMeasurement.GetAll();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objUnitOfMeasurement;
        }

        /// No CompanyID Provided
        /// <summary>
        /// Get Data From Database
        /// <para>Use it when Get data</para>
        /// </summary>
        public IEnumerable<CmnUOMGroup> GetUOMGroup(int? pageNumber, int? pageSize, int? IsPaging) 
        {
            GenericFactory_EF_UOMGroup = new CmnUOMGroup_EF();
            IEnumerable<CmnUOMGroup> objUOMGroup = null;
            try 
            {
                objUOMGroup = GenericFactory_EF_UOMGroup.GetAll();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objUOMGroup;
        }

        /// <summary>
        /// Get Data From Database
        /// <para>Use it when Get data</para>
        /// </summary>
        public IEnumerable<CmnUOM> GetUnitOfMeasurementById(int Id)
        {
            GenericFactory_EF_UnitOfMeasurement = new CmnUOM_EF();
            IEnumerable<CmnUOM> objUnitOfMeasurement = null;
            try
            {
                objUnitOfMeasurement = GenericFactory_EF_UnitOfMeasurement.FindBy(m => m.UOMID == Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUnitOfMeasurement;
        }

        /// No CompanyID Provided
        /// <summary>
        /// Save Data To Database
        /// <para>Use it when save data through ORM</para>
        /// </summary>
        public int SaveUpdateUnitOfMeasurement(CmnUOM model)
        {
            GenericFactory_EF_UnitOfMeasurement = new CmnUOM_EF();
            int result = 0;
            if (model.UOMID > 0)
            {
                try
                {
                    GenericFactory_EF_UnitOfMeasurement.Update(model);
                    GenericFactory_EF_UnitOfMeasurement.Save();
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
                int NextId = GenericFactory_EF_UnitOfMeasurement.getMaxVal_int("UOMID", "CmnUOM");

                try
                {
                    model.UOMID = NextId;
                    GenericFactory_EF_UnitOfMeasurement.Insert(model);
                    GenericFactory_EF_UnitOfMeasurement.Save();
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
        /// Delete Data To Database
        /// <para>Use it when Delete data through ORM</para>
        /// </summary>
        public int DeleteUnitOfMeasurement(int Id)
        {
            GenericFactory_EF_UnitOfMeasurement = new CmnUOM_EF();
            int result = 0;
            try
            {
                GenericFactory_EF_UnitOfMeasurement.Delete(m => m.UOMID == Id);
                GenericFactory_EF_UnitOfMeasurement.Save();
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
