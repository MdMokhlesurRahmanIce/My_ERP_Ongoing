using System;
using Telerik.Reporting;

namespace ABS.Reports.Accounting
{
    /// <summary>
    /// Summary description for CashPaymentVoucher.
    /// </summary>
    public partial class CashPaymentVoucher : Report
    {
        public CashPaymentVoucher()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }


        public static object EtoWDO(object value1)
        {
            double d1 = Convert.ToDouble(value1);
            ABSClass ns1 = new ABSClass();
            return ns1.changeNumericToWords(d1);


        }
    }
}