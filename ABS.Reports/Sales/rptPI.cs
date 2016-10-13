namespace ABS.Reports.Sales
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptPI.
    /// </summary>
    public partial class rptPI : Telerik.Reporting.Report
    {
        public rptPI()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }



        public string pramPINo
        {
            get
            {
                return (string)rpt_SalPI.Parameters[0].Value;
            }
            set
            {
                rpt_SalPI.Parameters[0].Value = value;
            }

        }

        public static object EtoWDO(object value1)
        {
            double d1 = Convert.ToDouble(value1);

            ABSConversion ns1 = new ABSConversion();
            return ns1.changeNumericToWords(d1);
        }

    }
}