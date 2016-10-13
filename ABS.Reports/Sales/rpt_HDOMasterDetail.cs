namespace ABS.Reports.Sales
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rpt_HDOMasterDetail.
    /// </summary>
    public partial class rpt_HDOMasterDetail : Telerik.Reporting.Report
    {
        public rpt_HDOMasterDetail()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public string pramHDONo
        {
            get
            {
                return (string)rpt_SalHDOMaster.Parameters[0].Value;                
            }
            set
            {
                rpt_SalHDOMaster.Parameters[0].Value = value;
                rpt_SalHDODetail.Parameters[0].Value = value;
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