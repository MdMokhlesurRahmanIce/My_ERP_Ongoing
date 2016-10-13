using System;

namespace ABS.Reports.Accounting
{
    /// <summary>
    /// Summary description for ChartOfAccount.
    /// </summary>
    public partial class ChartOfAccount : Telerik.Reporting.Report
    {
        public ChartOfAccount()
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

        public string pvparam
        {
            get
            {
                return (string)sqlDataSource1.Parameters[0].Value;
            }
            set
            {
                sqlDataSource1.Parameters[0].Value = value;
            }

        }
    }
}