namespace ABS.Reports.Production
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptRndFabricAnalysisDevelopment.
    /// </summary>
    public partial class rptRndFabricAnalysisDevelopment : Telerik.Reporting.Report
    {
        public rptRndFabricAnalysisDevelopment()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public int CompanyID
        {
            get
            {
                return (int)sqlDataSource1.Parameters[0].Value;
            }
            set
            {
                sqlDataSource1.Parameters[0].Value = value;
            }
        }

        public int item
        {
            get
            {
                return (int)sqlDataSource1.Parameters[1].Value;
            }
            set
            {
                sqlDataSource1.Parameters[1].Value = value;
            }
        }
    }
}