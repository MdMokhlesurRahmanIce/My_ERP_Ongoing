using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.Production.Interfaces
{
    public interface iChemiclePreparationMgt
    {
        object[] GetChemicalPreparation(vmCmnParameters objcmnParam, out int recordsTotal);
    }
}
