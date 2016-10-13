namespace ABS.Reports.Commercial
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptBeneficiarysCertificate.
    /// </summary>
    public partial class rptBeneficiarysCertificate : Telerik.Reporting.Report
    {
        public rptBeneficiarysCertificate()
        {
            InitializeComponent();
        }


        public int pramDCID
        {
            get
            {
                return (int)rpt_ComBeneficiaryAndInspectionCertificate.Parameters[0].Value;
            }
            set
            {
                rpt_ComBeneficiaryAndInspectionCertificate.Parameters[0].Value = value;
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