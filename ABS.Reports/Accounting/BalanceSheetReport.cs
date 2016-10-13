using System;

namespace ABS.Reports.Accounting
{
    public partial class BalanceSheetReport : Telerik.Reporting.Report
    {
        public BalanceSheetReport()
        {
            
            InitializeComponent();

           
        }

        public static object ValueCheck(object value1)
        {

            string str;

            float damount;

            float abc = float.Parse(value1.ToString());
           // string strval = string.Format("{0:0.00}", d1);
           // float abc = float.Parse(strval.ToString());



            if (abc < 0)
            {
                damount = abc * (-1);

                str = "(" + string.Format("{0:0,0.00}", damount) + ")";
            }

            else
            {
                damount = abc;
                str = string.Format("{0:0,0.00}", damount);
            }





            return (object)str ;


        }




        public DateTime pvparam1
        {
            get { return (DateTime)sqlDataSourceBalanceSheet.Parameters[0].Value; }
            set { sqlDataSourceBalanceSheet.Parameters[0].Value = value; }

        }
        public DateTime pvparam2
        {
            get { return (DateTime)sqlDataSourceBalanceSheet.Parameters[1].Value; }
            set { sqlDataSourceBalanceSheet.Parameters[1].Value = value; }

        }
        public int pvparam
        {
            get { return (int)sqlDataSourceBalanceSheet.Parameters[2].Value; }
            set { sqlDataSourceBalanceSheet.Parameters[2].Value = value; }

        }
    }
}