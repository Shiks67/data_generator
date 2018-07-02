using data_generator.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Presenter
{
    class DataAccess
    {
        OracleConnection con;
        SqlQuery sq;

        public static int nbCandy;
        public static int nbOrder;
        public static int nbCountry;

        void Connect()
        {
            con = new OracleConnection
            {
                ConnectionString = "User Id=<username>;Password=<password>;Data Source=<datasource>"
            };
            con.Open();
        }

        void Close()
        {
            con.Close();
            con.Dispose();
        }

        static void Main()
        {
            DataAccess da = new DataAccess();
            da.Connect();
            da.Close();
        }
        public void GetDataRows()
        {
            DataAccess da = new DataAccess();
            OracleCommand cmd = con.CreateCommand();
            string query = sq.CountCandyData();
            cmd.CommandText = query;

            da.Connect();
            OracleDataReader reader = cmd.ExecuteReader();
            da.Close();
        }

        public void InsertData()
        {
            DataAccess da = new DataAccess();

            OracleCommand cmd = con.CreateCommand();
            string query = sq.InsertData();
            cmd.CommandText = query;

            cmd.Parameters.Add(new OracleParameter("order_id", 1));
            cmd.Parameters.Add(new OracleParameter("customer_id", 1));
            cmd.Parameters.Add(new OracleParameter("country_id", 1));
            cmd.Parameters.Add(new OracleParameter("total_price", 1));
            cmd.Parameters.Add(new OracleParameter("date", 1));

            cmd.Parameters.Add(new OracleParameter("orderdetails_id", 1));
            cmd.Parameters.Add(new OracleParameter("candy_id", 1));
            cmd.Parameters.Add(new OracleParameter("quantity", 1));

            da.Connect();
            cmd.ExecuteNonQuery();
            da.Close();
        }
    }
}
