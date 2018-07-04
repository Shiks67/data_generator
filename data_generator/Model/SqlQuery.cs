using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class SqlQuery
    {
        public string CountCandyData()
        {
            return "SELECT (SELECT COUNT(*) FROM candy_reference) AS candy, " +
                "(SELECT COUNT(*) FROM orders) AS orders," +
                "(SELECT COUNT(*) FROM country) AS country, " +
                "(SELECT COUNT(*) FROM order_details) AS od " +
                "FROM country";
        }

        public string InsertAllCandy()
        {
            return "INSERT INTO candy VALUES(:candy_id,:candy_name,:additive,:coating,:aroma,:gelling,:sugar," +
                ":manufacturing_cost,:packaging_cost,:sending_cost,:general_cost," +
                ":bag_price,:box_price,:sample_price)";
        }

        public string InsertCandyColor()
        {
            return "INSERT INTO candy_color VALUES(:color_id,:color_name)";
        }

        public string InsertCandyVariant()
        {
            return "INSERT INTO candy_variant VALUES(:variant_id,:variant_name)";
        }

        public string InsertCandyTexture()
        {
            return "INSERT INTO candy_texture VALUES(:texture_id,:texture_name)";
        }

        public string InsertCandyPackaging()
        {
            return "INSERT INTO candy_packaging VALUES(:packaging_id,:packaging_name)";
        }

        public string InsertCandyRef()
        {
            return "INSERT INTO candy_reference VALUES(:candy_ref_id,:variant_id,:candy_id,:texture_id,'0',:packaging_id,:color_id,:candy_ref_code)";
        }

        public string InsertCandyStock()
        {
            return "INSERT INTO stock VALUES(:stock_id,:stock_candy_ref_id,:quantity_stock)";

        }

        public string InsertShipping()
        {
            return "INSERT INTO delivery_type VALUES(:id,:name,:quantity)";
        }

        public string InsertCountry()
        {
            return "INSERT INTO Country VALUES(:id,:shipping_id,:name)";
        }

        public string InsertOrders()
        {
            return "INSERT INTO orders VALUES(:order_id,:country_id,:customer_id,:total_price,:order_date)";
        }

        public string InsertOrdersDetails()
        {
            return "INSERT INTO order_details VALUES(:order_line,:candy_ref_id,:order_id,:quantity)";
        }

        public string InsertPackagingMachine()
        {
            return "INSERT INTO machine_condi VALUES(:machine_id,:id_packaging,:cadence,:change_tools)";
        }

        public string InsertmmWork()
        {
            return "INSERT INTO Machine_fab_work VALUES (:id,:machine_id,:candy_ref_id,:mw_date)";
        }

        public string InsertmpWork()
        {
            return "INSERT INTO Machine_condi_work VALUES (:id,:machine_id,:candy_ref_id,:mw_date)";
        }

        public string GetCandySent()
        {
            return "SELECT id_candy_reference, (SELECT id_variant from candy_reference WHERE id_candy_reference = order_details.id_candy_reference)," +
                "(SELECT id_packaging from candy_reference WHERE id_candy_reference = order_details.id_candy_reference), " +
                "(SELECT order_date from orders where order_id = order_details.order_id) " +
                "FROM order_details";
        }

        public string GetMachineManufacture()
        {
            return "SELECT id_variant, id_machine_fab FROM associe";
        }

        public string GetMachinePackaging()
        {
            return "SELECT id_machine_condi, id_packaging FROM machine_condi";
        }
    }
}
