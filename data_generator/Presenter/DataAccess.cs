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
        OracleConnection con = new OracleConnection
        {
            ConnectionString = "Data Source=(DESCRIPTION =" +
                "(ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.43.105)(PORT = 1521))" +
                "(CONNECT_DATA =(SERVER = DEDICATED)" +
                "(SERVICE_NAME = PBigData))); " +
                "User Id=Generateur_Donnes;Password=azer123*"
        };
        SqlQuery sq = new SqlQuery();

        public static int nbCandy;
        public static int nbOrder;
        public static int nbOD;
        public static int nbCountry;

        void Close()
        {
            con.Close();
            con.Dispose();
        }

        public void GetDataRows()
        {
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            string query = sq.CountCandyData();
            cmd.CommandText = query;
            OracleDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                nbCandy = Convert.ToInt32(reader.GetValue(0));
                nbOrder = Convert.ToInt32(reader.GetValue(1));
                nbCountry = Convert.ToInt32(reader.GetValue(2));
                nbOD = Convert.ToInt32(reader.GetValue(3));
            }
            Close();
        }

        public void InsertOrder(List<Order> orders, List<OrderDetails> orderDetails)
        {
            string query = sq.InsertOrders();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = orders.Count;
                cmd.Parameters.Add(new OracleParameter("order_id", OracleDbType.Int32, orders.Select(c => c.order_id).ToArray(), ParameterDirection.Input)); //x2
                cmd.Parameters.Add(new OracleParameter("customer_id", OracleDbType.Int32, orders.Select(c => c.customer_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("country_id", OracleDbType.Int32, orders.Select(c => c.country_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("total_price", OracleDbType.Int32, orders.Select(c => c.total_price).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("order_date", OracleDbType.Varchar2, orders.Select(c => c.date).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertOrdersDetails();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = orderDetails.Count;

                cmd.Parameters.Add(new OracleParameter("order_details_id", OracleDbType.Int32, orderDetails.Select(c => c.order_details_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("re_order_id", OracleDbType.Int32, orderDetails.Select(c => c.order_id).ToArray(), ParameterDirection.Input)); //x2
                cmd.Parameters.Add(new OracleParameter("candy_ref_id", OracleDbType.Int32, orderDetails.Select(c => c.candy_ref_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("quantity", OracleDbType.Int32, orderDetails.Select(c => c.quantity).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();
        }

        public List<CandySent> GetCandyData()
        {
            List<CandySent> candySent = new List<CandySent>();
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            string query = sq.GetCandySent();
            cmd.CommandText = query;
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                candySent.Add(new CandySent
                {
                    Candy_ref_id = Convert.ToInt32(reader.GetValue(0)),
                    Variant_id = Convert.ToInt32(reader.GetValue(1)),
                    Package_id = Convert.ToInt32(reader.GetValue(2)),
                    Date = reader.GetValue(3).ToString()
                });

            }
            Close();

            return candySent;
        }

        public void InsertData()
        {
            string query = sq.InsertCandyColor();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = PopulateDB.color.Count;
                cmd.Parameters.Add(new OracleParameter("color_id", OracleDbType.Int32, PopulateDB.color.Select(c => c.id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("color_name", OracleDbType.Varchar2, PopulateDB.color.Select(c => c.name).ToArray(), ParameterDirection.Input));
                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertCandyVariant();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = PopulateDB.variant.Count;
                cmd.Parameters.Add(new OracleParameter("variant_id", OracleDbType.Int32, PopulateDB.variant.Select(c => c.id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("variant_name", OracleDbType.Varchar2, PopulateDB.variant.Select(c => c.name).ToArray(), ParameterDirection.Input));
                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertCandyTexture();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = PopulateDB.texture.Count;
                cmd.Parameters.Add(new OracleParameter("texture_id", OracleDbType.Int32, PopulateDB.texture.Select(c => c.id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("texture_name", OracleDbType.Varchar2, PopulateDB.texture.Select(c => c.name).ToArray(), ParameterDirection.Input));
                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertCandyPackaging();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = PopulateDB.packaging.Count;
                cmd.Parameters.Add(new OracleParameter("packaging_id", OracleDbType.Int32, PopulateDB.packaging.Select(c => c.id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("packaging_name", OracleDbType.Varchar2, PopulateDB.packaging.Select(c => c.name).ToArray(), ParameterDirection.Input));
                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertAllCandy();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = PopulateDB.candy.Count;
                cmd.Parameters.Add(new OracleParameter("candy_id", OracleDbType.Int32, PopulateDB.candy.Select(c => c.id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("candy_name", OracleDbType.Varchar2, PopulateDB.candy.Select(c => c.name).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("manufacturing_cost", OracleDbType.Int32, PopulateDB.candy.Select(c => c.manufacturing_cost).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("packaging_cost", OracleDbType.Int32, PopulateDB.candy.Select(c => c.packaging_cost).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("sending_cost", OracleDbType.Int32, PopulateDB.candy.Select(c => c.sending_cost).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("general_cost", OracleDbType.Int32, PopulateDB.candy.Select(c => c.general_cost).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("bag_price", OracleDbType.Decimal, PopulateDB.candy.Select(c => c.bag_price).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("box_price", OracleDbType.Decimal, PopulateDB.candy.Select(c => c.box_price).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("sample_price", OracleDbType.Decimal, PopulateDB.candy.Select(c => c.sample_price).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("additive", OracleDbType.Int32, PopulateDB.candy.Select(c => c.additive).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("coating", OracleDbType.Int32, PopulateDB.candy.Select(c => c.coating).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("aroma", OracleDbType.Int32, PopulateDB.candy.Select(c => c.aroma).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("gelling", OracleDbType.Int32, PopulateDB.candy.Select(c => c.gelling).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("sugar", OracleDbType.Int32, PopulateDB.candy.Select(c => c.sugar).ToArray(), ParameterDirection.Input));
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        public void InsertAllCandyRef(List<CandyReference> bulkData)
        {
            string query = sq.InsertCandyRef();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = bulkData.Count;

                cmd.Parameters.Add(new OracleParameter("candy_ref_id", OracleDbType.Int32, bulkData.Select(c => c.Candy_ref_id).ToArray(), ParameterDirection.Input)); //x2
                cmd.Parameters.Add(new OracleParameter("candy_id", OracleDbType.Int32, bulkData.Select(c => c.Candy_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("color_id", OracleDbType.Int32, bulkData.Select(c => c.Color_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("variant_id", OracleDbType.Int32, bulkData.Select(c => c.Variant_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("texture_id", OracleDbType.Int32, bulkData.Select(c => c.Texture_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("packaging_id", OracleDbType.Int32, bulkData.Select(c => c.Packaging_id).ToArray(), ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("stock_id", OracleDbType.Int32, bulkData.Select(c => c.Candy_ref_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("candy_ref_code", OracleDbType.Int32, bulkData.Select(c => c.Candy_ref_code).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertCandyStock();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = bulkData.Count;

                cmd.Parameters.Add(new OracleParameter("stock_id", OracleDbType.Int32, bulkData.Select(c => c.Candy_ref_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("stock_candy_ref_id", OracleDbType.Int32, bulkData.Select(c => c.Candy_ref_id).ToArray(), ParameterDirection.Input)); //x2
                cmd.Parameters.Add(new OracleParameter("quantity_stock", OracleDbType.Int32, bulkData.Select(c => c.Candy_quantity).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();
        }

        internal void InsertCountryShippingData(List<Country> country, List<Shipping> shipping)
        {
            string query = sq.InsertShipping();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = shipping.Count;

                cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int32, shipping.Select(c => c.Shipping_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("name", OracleDbType.Varchar2, shipping.Select(c => c.Name).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("quantity", OracleDbType.Int32, shipping.Select(c => c.Quantity).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertCountry();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = country.Count;

                cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int32, country.Select(c => c.Country_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("name", OracleDbType.Varchar2, country.Select(c => c.Name).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("shipping_id", OracleDbType.Int32, country.Select(c => c.Shipping_id).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();
        }

        public void InsertCondiMachine(List<MachinePackaging> machinePackagings)
        {
            string query = sq.InsertPackagingMachine();
            con.Open();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = machinePackagings.Count;
                cmd.Parameters.Add(new OracleParameter("machine_id", OracleDbType.Int32, machinePackagings.Select(c => c.Machine_id).ToArray(), ParameterDirection.Input)); //x2
                cmd.Parameters.Add(new OracleParameter("id_packaging", OracleDbType.Int32, machinePackagings.Select(c => c.Packaging_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("cadence", OracleDbType.Int32, machinePackagings.Select(c => c.Cadence).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("change_tools", OracleDbType.Int32, machinePackagings.Select(c => c.Tool_change).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();
        }

        //Au cas où si je dois faire l'etl
        public void GetData()
        {
            OracleCommand cmd = con.CreateCommand();
            string query = ""; //requete select des données à insert dans mongoDB
            cmd.CommandText = query;

            con.Open();
            OracleDataReader reader = cmd.ExecuteReader();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                //List<> des tables à récup
            }

            Close();
        }
    }
}
