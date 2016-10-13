using System;

namespace ABS.Reports.Accounting
{
    /// <summary>
    /// Summary description for PettyCashBook.
    /// </summary>
    public partial class LedgerBookAllLevel : Telerik.Reporting.Report
    {
        public LedgerBookAllLevel()
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
                return (DateTime)rptPettyCashData.Parameters[0].Value;
            }
            set
            {
                rptPettyCashData.Parameters[0].Value = value;
                sqlDataSource2.Parameters[0].Value = value;
            }

        }
        public DateTime pvparam1
        {
            get
            {
                return (DateTime)rptPettyCashData.Parameters[1].Value;
            }
            set
            {
                rptPettyCashData.Parameters[1].Value = value;
                sqlDataSource2.Parameters[1].Value = value;

            }

        }
        public string pvparam2
        {
            get
            {
                return (string)rptPettyCashData.Parameters[2].Value;
            }
            set
            {
                rptPettyCashData.Parameters[2].Value = value;
                sqlDataSource2.Parameters[2].Value = value;

            }

        }
        public string pvparam3
        {
            get
            {
                return (string)rptPettyCashData.Parameters[3].Value;
            }
            set
            {
                rptPettyCashData.Parameters[3].Value = value;
                sqlDataSource2.Parameters[3].Value = value;

            }

        }

        public int CompanyId
        {
            get
            {
                return (int)rptPettyCashData.Parameters[4].Value;


            }
            set
            {
                rptPettyCashData.Parameters[4].Value = value;
                sqlDataSource2.Parameters[4].Value = value;

            }

        }
    }
}