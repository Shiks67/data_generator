﻿using data_generator.Model;
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
        SqlQuery sq = new SqlQuery();

        public static int nbCandy;
        public static int nbOrder;
        public static int nbOD;
        public static int nbCountry;
        public static List<CandyRefPrice> crp;

        void Connect()
        {
            con = new OracleConnection
            {
                ConnectionString = "Data Source=(DESCRIPTION =" +
                "(ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.43.105)(PORT = 1521))" +
                "(CONNECT_DATA =(SERVER = DEDICATED)" +
                "(SERVICE_NAME = PBigData))); " +
                "User Id=Generateur_Donnes;Password=azer123*"
            };
            con.Open();
        }

        void Close()
        {
            con.Close();
            con.Dispose();
        }

        public List<CandyRefPrice> GetDataRows()
        {
            crp = new List<CandyRefPrice>();
            List<Candy> candyp = new List<Candy>();

            Connect();
            OracleCommand cmd = con.CreateCommand();
            string query = sq.CountCandyData();
            cmd.CommandText = query;
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                nbCandy = Convert.ToInt32(reader.GetValue(0));
                nbOrder = Convert.ToInt32(reader.GetValue(1));
                nbCountry = Convert.ToInt32(reader.GetValue(2));
                nbOD = Convert.ToInt32(reader.GetValue(3));
            }
            Close();

            Connect();
            cmd = con.CreateCommand();
            query = sq.GetCandyRef();
            cmd.CommandText = query;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                crp.Add(new CandyRefPrice
                {
                    Candy_ref_id = Convert.ToInt32(reader.GetValue(0)),
                    Package_id = Convert.ToInt32(reader.GetValue(1)),
                    Candy_id = Convert.ToInt32(reader.GetValue(2)),
                    Price = "0"
                });
            }
            Close();

            Connect();
            cmd = con.CreateCommand();
            query = sq.GetCandyPrices();
            cmd.CommandText = query;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                candyp.Add(new Candy
                {
                    candy_id = Convert.ToInt32(reader.GetValue(0)),
                    bag_price = reader.GetValue(1).ToString(),
                    box_price = reader.GetValue(2).ToString(),
                    sample_price = reader.GetValue(3).ToString()
                });
            }
            Close();

            for (int i = 0; i < crp.Count; i++)
            {
                if(crp[i].Package_id == 1)
                    crp[i].Price = candyp.Single(x => x.candy_id == crp[i].Candy_id).bag_price.ToString();
                if (crp[i].Package_id == 2)
                    crp[i].Price = candyp.Single(x => x.candy_id == crp[i].Candy_id).box_price.ToString();
                if (crp[i].Package_id == 3)
                    crp[i].Price = candyp.Single(x => x.candy_id == crp[i].Candy_id).sample_price.ToString();
            }
            return crp;
        }

        public void InsertOrder(List<Order> orders, List<OrderDetails> orderDetails)
        {
            string query = sq.InsertOrders();
            Connect();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = orders.Count;
                cmd.Parameters.Add(new OracleParameter("order_id", OracleDbType.Int32, orders.Select(c => c.order_id).ToArray(), ParameterDirection.Input)); //x2
                cmd.Parameters.Add(new OracleParameter("customer_id", OracleDbType.Int32, orders.Select(c => c.customer_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("country_id", OracleDbType.Int32, orders.Select(c => c.country_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("total_price", OracleDbType.Decimal, orders.Select(c => c.total_price).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("order_date", OracleDbType.Varchar2, orders.Select(c => c.date).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertOrdersDetails();
            Connect();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = orderDetails.Count;

                cmd.Parameters.Add(new OracleParameter("order_line", OracleDbType.Int32, orderDetails.Select(c => c.order_line).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("order_id", OracleDbType.Int32, orderDetails.Select(c => c.order_id).ToArray(), ParameterDirection.Input)); //x2
                cmd.Parameters.Add(new OracleParameter("candy_ref_id", OracleDbType.Int32, orderDetails.Select(c => c.candy_ref_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("quantity", OracleDbType.Int32, orderDetails.Select(c => c.quantity).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();
        }

        public List<Container> GetPackageData()
        {
            List<Container> nb_package = new List<Container>();
            Connect();
            OracleCommand cmd = con.CreateCommand();
            string query = sq.GetPackageData();
            cmd.CommandText = query;
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                nb_package.Add(new Container
                {
                    Packaging_id = Convert.ToInt32(reader.GetValue(0)),
                    Quantity = Convert.ToInt32(reader.GetValue(1))
                });
            }
            Close();
            return nb_package;
        }

        public List<CandySent> GetCandyData()
        {
            List<CandySent> candySent = new List<CandySent>();
            Connect();
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
                    Date = reader.GetValue(3).ToString(),
                    Quantity = Convert.ToInt32(reader.GetValue(4))
                });

            }
            Close();
            return candySent;
        }

        public List<MachineManufacture> GetMachineManufactureData()
        {
            List<MachineManufacture> machineManufacture = new List<MachineManufacture>();
            Connect();
            OracleCommand cmd = con.CreateCommand();
            string query = sq.GetMachineManufacture();
            cmd.CommandText = query;
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                machineManufacture.Add(new MachineManufacture
                {
                    Machine_id = Convert.ToInt32(reader.GetValue(0)),
                    candy_variant_id = Convert.ToInt32(reader.GetValue(1)),
                    Cadence = reader.GetValue(2).ToString()
                });

            }
            Close();
            return machineManufacture;
        }

        public List<MachinePackaging> GetMachinePackagingData()
        {
            List<MachinePackaging> machinePackaging = new List<MachinePackaging>();
            Connect();
            OracleCommand cmd = con.CreateCommand();
            string query = sq.GetMachinePackaging();
            cmd.CommandText = query;
            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                machinePackaging.Add(new MachinePackaging
                {
                    Machine_id = Convert.ToInt32(reader.GetValue(0)),
                    Packaging_id = Convert.ToInt32(reader.GetValue(1)),
                    Cadence = Convert.ToInt32(reader.GetValue(2))
                });

            }
            Close();

            return machinePackaging;
        }

        public void InsertData()
        {
            string query = sq.InsertCandyColor();
            Connect();
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
            Connect();
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
            Connect();
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
            Connect();
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
            Connect();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = PopulateDB.candy.Count;
                cmd.Parameters.Add(new OracleParameter("candy_id", OracleDbType.Int32, PopulateDB.candy.Select(c => c.candy_id).ToArray(), ParameterDirection.Input));
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
            Connect();
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
            Connect();
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
            Connect();
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
            Connect();
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
            Connect();
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

        public void InsertMachineWork(List<MachineWork> mp, List<MachineWork> mm)
        {
            string query = sq.InsertmpWork();
            Connect();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = mp.Count;

                cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int32, mp.Select(c => c.Id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("machine_id", OracleDbType.Int32, mp.Select(c => c.Machine_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("candy_ref_id", OracleDbType.Int32, mp.Select(c => c.Candy_ref_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("quantity", OracleDbType.Int32, mp.Select(c => c.Quantity).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("mw_date", OracleDbType.Varchar2, mp.Select(c => c.Date).ToArray(), ParameterDirection.Input));

                cmd.ExecuteNonQuery();
            }
            Close();

            query = sq.InsertmmWork();
            Connect();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.ArrayBindCount = mm.Count;

                cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Int32, mm.Select(c => c.Id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("machine_id", OracleDbType.Int32, mm.Select(c => c.Machine_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("candy_ref_id", OracleDbType.Int32, mm.Select(c => c.Candy_ref_id).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("quantity", OracleDbType.Int32, mm.Select(c => c.Quantity).ToArray(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("mw_date", OracleDbType.Varchar2, mm.Select(c => c.Date).ToArray(), ParameterDirection.Input));

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

            Connect();
            OracleDataReader reader = cmd.ExecuteReader();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                //List<> des tables à récup
            }

            Close();
        }
    }
}
