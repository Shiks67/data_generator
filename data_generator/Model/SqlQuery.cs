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
            return "SELECT (SELECT COUNT(*) FROM candy) AS candy, " +
                "(SELECT COUNT(*) FROM order) AS order" +
                "(SELECT COUNT(*) FROM country) AS country;";
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

        public string InsertCandyRefAndStock()
        {
            return "BEGIN" +
                "INSERT INTO candy_reference VALUES(:candy_ref_id,:candy_id,:color_id,:variant_id,:texture_id,:packaging_id);" +
                "INSERT INTO stock VALUES(:stock_id,:candy_ref_id,:quantity_stock);" +
                "END;";
        }

        public string InsertShipping()
        {
            return "INSERT INTO Shipping VALUES(:id,:name,:quantity)";
        }

        public string InsertCountry()
        {
            return "INSERT INTO Country VALUES(:id,:name,:shipping_id)";
        }

        public string InsertData()
        {
            return "BEGIN" +
                "INSERT INTO order VALUES(:order_id,:customer_id,:country_id,:total_price,:date); " +
                "INSERT INTO order_details VALUES(:order_details_id,:order_id,:candy_id,:quantity);" +
                "END;";
        }
    }
}
