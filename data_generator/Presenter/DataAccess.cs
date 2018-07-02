using data_generator.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

        public void InsertData(List<Order> bulkData)
        {
            DataAccess da = new DataAccess();

            //OracleCommand cmd = con.CreateCommand();
            string query = sq.InsertData();
            da.Connect();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = bulkData.Count;
                cmd.Parameters.Add(new OracleParameter("order_id", OracleDbType.Int64, bulkData.Select(c => c.order_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("customer_id", OracleDbType.Int64, bulkData.Select(c => c.customer_id).ToArray(), ParameterDirection.Input)));
                cmd.Parameters.Add(new OracleParameter("country_id", OracleDbType.Int64, bulkData.Select(c => c.country_id).ToArray(), ParameterDirection.Input)));
                cmd.Parameters.Add(new OracleParameter("total_price", OracleDbType.Int64, bulkData.Select(c => c.total_price).ToArray(), ParameterDirection.Input)));
                cmd.Parameters.Add(new OracleParameter("date", OracleDbType.Varchar2, bulkData.Select(c => c.date).ToArray(), ParameterDirection.Input)));

                cmd.Parameters.Add(new OracleParameter("order_details_id", OracleDbType.Int64, bulkData.Select(c => c.order_details_id).ToArray(), ParameterDirection.Input)));
                cmd.Parameters.Add(new OracleParameter("candy_id", OracleDbType.Int64, bulkData.Select(c => c.candy_id).ToArray(), ParameterDirection.Input)));
                cmd.Parameters.Add(new OracleParameter("quantity", OracleDbType.Int64, bulkData.Select(c => c.quantity).ToArray(), ParameterDirection.Input)));

                cmd.ExecuteNonQuery();
            }
            da.Close();
        }
    }
}
