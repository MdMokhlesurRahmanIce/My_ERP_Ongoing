namespace ABS.Reports.Inventory
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for rptGRR.
    /// </summary> 
    public partial class rptMRRDetail : Telerik.Reporting.Report
    {
        public rptMRRDetail()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent(); 

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public DateTime startDate
        {
            get { return (DateTime)sqlDataSource1.Parameters[0].Value; }
            set { sqlDataSource1.Parameters[0].Value = value; }

        }
        public DateTime endDate 
        {
            get { return (DateTime)sqlDataSource1.Parameters[1].Value; }
            set { sqlDataSource1.Parameters[1].Value = value; }

        }
       
        public int createdBy
        {
            get
            {
                return (int)sqlDataSource1.Parameters[2].Value;
            }
            set
            {
                sqlDataSource1.Parameters[2].Value = value;
            }

        }

        public int companyId
        {
            get
            {
                return (int)sqlDataSource1.Parameters[3].Value;
            }
            set
            {
                sqlDataSource1.Parameters[3].Value = value;
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