using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABS.Data.BaseFactories;
using ABS.Data.BaseInterfaces;
using ABS.Models;
using ABS.Service.Production.Interfaces;
using ABS.Service.AllServiceClasses;

namespace ABS.Service.Production.Factories
{
    //public class PrdLineSetupMgt : iPrdLineSetupMgt
    //{
    //    private iGenericFactory_EF<PrdCmnLineSetup> GenericFactory_EF_PrdlineSetup = null;

    //    public PrdLineSetupMgt()
    //    {
    //        GenericFactory_EF_PrdlineSetup = new PrdLineSetupFactory_EF();
    //    }

    //    public IEnumerable<PrdCmnLineSetup> GetLines(int? pageNumber, int? pageSize, int? IsPaging)
    //    {
    //        IEnumerable<PrdCmnLineSetup> objPrdLineSetup = null;
    //        try
    //        {
    //            objPrdLineSetup = GenericFactory_EF_PrdlineSetup.GetAll().Where(x => x.IsDeleted == false);
    //        }
    //        catch (Exception e)
    //        {
    //            e.ToString();
    //        }

    //        return objPrdLineSetup;
    //    }


    //    public PrdCmnLineSetup GetLineByID(int? id)
    //    {

    //        PrdCmnLineSetup objPrdLineSetup = null;
    //        try
    //        {
    //            objPrdLineSetup = GenericFactory_EF_PrdlineSetup.FindBy(m => m.PrdLineID == id).FirstOrDefault();
    //        }
    //        catch (Exception e)
    //        {
    //            e.ToString();
    //        }
    //        return objPrdLineSetup;
    //    }


    //    public int SaveLine(PrdCmnLineSetup model)
    //    {
    //        int result = 0;
    //        try
    //        {

    //            int NextId = GenericFactory_EF_PrdlineSetup.getMaxVal_int("PrdLineID", "PrdCmnLineSetup");
    //            model.PrdLineID = NextId;
    //            model.CreateOn = DateTime.Today;
    //            model.UpdatePc =  HostService.GetIP();
    //            GenericFactory_EF_PrdlineSetup.Insert(model);
    //            GenericFactory_EF_PrdlineSetup.Save();
    //            result = 1;

    //        }
    //        catch (Exception ex)
    //        {
    //            ex.ToString();
    //            result = 0;
    //        }
    //        return result;
    //    }





    //}
}
