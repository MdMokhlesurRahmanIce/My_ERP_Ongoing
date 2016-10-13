using ABS.Utility.Attributes;
using System.Web.Mvc;

namespace ABS.Web.Areas.Accounting.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Accounting/Reports/
        [SessionExpire]
        public RedirectResult CashPaymentVoucher()
        {

            return Redirect("~/Areas/Accounting/Reports/CashPVReportByVoucherNo.aspx");  
        }

        [SessionExpire]
        public RedirectResult BankPaymentVoucher()
        {
            return Redirect("~/Areas/Accounting/Reports/Bankpayment.aspx");
        }

        [SessionExpire]
        public RedirectResult ContraVoucher()
        {
            return Redirect("~/Areas/Accounting/Reports/ContraVoucher.aspx");
        }
        [SessionExpire]
        public RedirectResult JournalVoucher()
        {
            return Redirect("~/Areas/Accounting/Reports/JournalVoucher.aspx");
        }
        [SessionExpire]
        public RedirectResult ReceiptVoucher()
        {
            return Redirect("~/Areas/Accounting/Reports/ReceiptVoucher.aspx");
        }

        [SessionExpire]
        public RedirectResult BankReceiptVoucher()
        {
            return Redirect("~/Areas/Accounting/Reports/BankReceiptVoucher.aspx");
        }
        [SessionExpire]
        public RedirectResult CashStatement()
        {
            return Redirect("~/Areas/Accounting/Reports/CashPaymentStatement.aspx");
        }
        [SessionExpire]
        public RedirectResult BankStatement()
        {
            return Redirect("~/Areas/Accounting/Reports/BankPaymentStatement.aspx");
        }
        [SessionExpire]
        public RedirectResult ReceiptStatement()
        {
            return Redirect("~/Areas/Accounting/Reports/ReceiptStatement.aspx");
        }
        [SessionExpire]
        public RedirectResult ContraStatement()
        {
            return Redirect("~/Areas/Accounting/Reports/ContraStatement.aspx");
        }
        [SessionExpire]
        public RedirectResult JournalStatement()
        {
            return Redirect("~/Areas/Accounting/Reports/JournalStatement.aspx");
        }
        [SessionExpire]
        public RedirectResult TrialBalanceVoucher()
        {
            return Redirect("~/Areas/Accounting/Reports/TrailBalanceReportNew.aspx");
        }

        [SessionExpire]
        public RedirectResult LedgerBookVoucher()
        {
            return Redirect("~/Areas/Accounting/Reports/LedgerBookReport.aspx");
        }

        [SessionExpire]
        public RedirectResult BalanceSheet()
        {
            return Redirect("~/Areas/Accounting/Reports/BalanceSheet.aspx");
        }
        [SessionExpire]
        public RedirectResult IncomeStatement()
        {
            return Redirect("~/Areas/Accounting/Reports/IncomeStatement.aspx");
        }
        [SessionExpire]
        public RedirectResult ChartOfAccounts()
        {
            return Redirect("~/Areas/Accounting/Reports/ChartOfAccountReport.aspx");
        }
        [SessionExpire]
        public RedirectResult LedgerBookAllIndex()
        {
            return Redirect("~/Areas/Accounting/Reports/LedgerBookAll.aspx");
        }
        [SessionExpire]
        public RedirectResult TrialBalanceAllLevel()
        {
            return Redirect("~/Areas/Accounting/Reports/TrailBalanceAllLevel.aspx");
        }
     

	}
}