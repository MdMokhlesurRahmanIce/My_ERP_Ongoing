using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.Production.Interfaces;
using ABS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ABS.Service.Production.Factories
{

    public partial class ConsumptionFactory_EF : GenericFactory_EF<ERP_Entities, RndConsumptionType> { }

    public class ConsumptionMgt : iConsumptionMgt
    {
        private iGenericFactory_EF<RndConsumptionType> GenericFactory_EF_Consumption = null;

        public ConsumptionMgt()
        {
            
        }

        /// No CompanyID Provided
        public IEnumerable<RndConsumptionType> GetConsumptions(int? pageNumber, int? pageSize, int? IsPaging)
        {
            GenericFactory_EF_Consumption = new ConsumptionFactory_EF();
            IEnumerable<RndConsumptionType> objConsumptionTypes = null;
            try
            {
                objConsumptionTypes = GenericFactory_EF_Consumption.GetAll().Where(x => x.IsDeleted == false);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objConsumptionTypes;
        }


        public RndConsumptionType GetConsumptionByID(int? id)
        {
            GenericFactory_EF_Consumption = new ConsumptionFactory_EF();
            RndConsumptionType objConsumption = null;
            try
            {
                objConsumption = GenericFactory_EF_Consumption.FindBy(m => m.ConsumptionTypeID == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objConsumption;
        }

        public int SaveConsumption(RndConsumptionType model)
        {
            GenericFactory_EF_Consumption = new ConsumptionFactory_EF();
            int result = 0;
            try
            {
                int NextId = GenericFactory_EF_Consumption.getMaxVal_int("ConsumptionTypeID", "RndConsumptionType");
                model.ConsumptionTypeID = NextId;
                model.CreateOn = DateTime.Today;
                model.CreatePc =  HostService.GetIP();
                GenericFactory_EF_Consumption.Insert(model);
                GenericFactory_EF_Consumption.Save();
                result = 1;

            }
            catch (Exception ex)
            {

                ex.ToString();
                result = 0;
            }
            return result;
        }

        public int UpdateConsumption(RndConsumptionType model)
        {
            GenericFactory_EF_Consumption = new ConsumptionFactory_EF();
            int result = 0;

            try
            {
                model.UpdateOn = DateTime.Today;
                model.UpdatePc =  HostService.GetIP();
                GenericFactory_EF_Consumption.Update(model);
                GenericFactory_EF_Consumption.Save();

                result = 1;
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return result;
        }

        public int DeleteConsumption(RndConsumptionType model)
        {
            GenericFactory_EF_Consumption = new ConsumptionFactory_EF();
            int result = 0;

            try
            {

                model.DeleteOn = DateTime.Today;
                model.DeleteBy = model.DeleteBy;
                model.IsDeleted = true;
                GenericFactory_EF_Consumption.Update(model);
                GenericFactory_EF_Consumption.Save();

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
