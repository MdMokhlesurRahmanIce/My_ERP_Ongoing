using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.SystemCommon.Interfaces
{
    public interface iColorMgt_EF
    {
        IEnumerable<vmColor> GetColors(vmCmnParameters objcmnParam, out int recordsTotal);
        string SaveUpdateColors(vmColor model, vmCmnParameters objcmnParam);
        CmnItemColor GetColorsById(vmCmnParameters objcmnParam);
        string DeleteUpdateColor(vmCmnParameters objcmnParam);
        int DeleteColors(int Id);
    }
}
