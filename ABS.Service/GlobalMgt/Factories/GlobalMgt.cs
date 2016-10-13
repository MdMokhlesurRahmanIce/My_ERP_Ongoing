using ABS.Models;
using ABS.Service.GlobalMgt.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABS.Service.GlobalMgt.Factories
{
    public class GlobalMgt : iGlobalMgt
    {
        private ERP_Entities _ctxCmn = null;
        public int GetUserTotal()
        {
            int result = 0;
            try
            {
                using (_ctxCmn = new ERP_Entities())
                {
                    result = _ctxCmn.CmnUsers.Count();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return result;
        }
    }
}
