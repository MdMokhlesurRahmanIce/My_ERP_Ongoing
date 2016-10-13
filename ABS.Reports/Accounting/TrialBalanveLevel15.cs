using System;
using Telerik.Reporting;

namespace ABS.Reports.Accounting
{
    /// <summary>
    /// Summary description for TrialBalanveLevel15.
    /// </summary>
    public partial class TrialBalanveLevel15 : Report
    {
        public TrialBalanveLevel15()
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


        public DateTime pvparam1
        {
            get
            {
                return (DateTime)sqlDataSource1.Parameters[1].Value;
            }
            set
            {
                sqlDataSource1.Parameters[1].Value = value;
            }

        }

        public DateTime pvparam2
        {
            get
            {
                return (DateTime)sqlDataSource1.Parameters[2].Value;
            }
            set
            {
                sqlDataSource1.Parameters[2].Value = value;
            }

        }


        public string pvparam3
        {
            get
            {
                return (string)sqlDataSource1.Parameters[3].Value;
            }
            set
            {
                sqlDataSource1.Parameters[3].Value = value;
            }

        }

    }
}