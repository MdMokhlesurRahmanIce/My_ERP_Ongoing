using ABS.Utility.Attributes;
using System.Web.Mvc;

namespace ABS.Web.Areas.Accounting.Controllers
{
    public class AccountHomeController : Controller
    {
        // GET: /Accounts/AccountHome/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}