using ABS.Models.ViewModel.ErrorLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Utility.ErrorExLog
{
    public interface iErrorMgt
    {
        IEnumerable<vmErrorStack> GetErrorLog(int? pageNumber, int? pageSize, int? IsPaging);
        int SaveErrorLog(vmErrorStack model);
    }
}
