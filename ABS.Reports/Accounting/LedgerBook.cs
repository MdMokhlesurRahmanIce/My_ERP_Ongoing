using System;

namespace ABS.Reports.Accounting
{
    /// <summary>
    /// Summary description for PettyCashBook.
    /// </summary>
    public partial class LedgerBook : Telerik.Reporting.Report
    {
        public LedgerBook()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public DateTime pvparam
        {
            get
            {
                return (DateTime)sqlDataSource1.Parameters[0].Value;
            }
            set
            {
                sqlDataSource1.Parameters[0].Value = value;
                rptPettyCashData.Parameters[0].Value = value;
                sqlDataSource2.Parameters[0].Value = value;
            }

        }
        public DateTime pvparam1
        {
            get
            {
                return (DateTime)sqlDataSource1.Parameters[1].Value;
            }
            set
            {
                sqlDataSource1.Parameters[1].Value = value;
                rptPettyCashData.Parameters[1].Value = value;
                sqlDataSource2.Parameters[1].Value = value;

            }

        }
        public string pvparam2
        {
            get
            {
                return (string)sqlDataSource1.Parameters[2].Value;
            }
            set
            {
                sqlDataSource1.Parameters[2].Value = value;
                rptPettyCashData.Parameters[2].Value = value;
                sqlDataSource2.Parameters[2].Value = value;

            }

        }

        public int companyId
        {
            get
            {
                return (int)sqlDataSource2.Parameters[3].Value;
            }
            set
            {
                //sqlDataSource1.Parameters[3].Value = value;
                rptPettyCashData.Parameters[3].Value = value;
                sqlDataSource2.Parameters[3].Value = value;

            }

        }
    }
}