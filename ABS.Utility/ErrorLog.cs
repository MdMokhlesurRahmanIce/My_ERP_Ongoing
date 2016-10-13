using ABS.Models.ViewModel.ErrorLog;
using ABS.Utility.ErrorExLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ABS.Utility
{
    public class ErrorLog
    {
        public static void Log(Exception ex)
        {
            vmErrorStack objError = new vmErrorStack();
            StackTrace st = new StackTrace(true);
            StackFrame sf = st.GetFrame(1);

            objError.ErrorMethod = sf.GetMethod().ToString();
            objError.ErrorFile = sf.GetFileName().ToString();
            objError.ErrorLine = sf.GetFileLineNumber().ToString();
            objError.ErrorDate = DateTime.Now.ToString();
            objError.ErrorPath = HttpContext.Current.Request.Url.AbsolutePath;
            objError.ErrorType = ex.GetType().ToString();
            objError.ErrorMessage = ex.Message;
            objError.ErrorSource = ex.Source;
            objError.ErrorStack = ex.StackTrace;

            //objErrorLog.SaveErrorLog(objError);
        }
    }
}
