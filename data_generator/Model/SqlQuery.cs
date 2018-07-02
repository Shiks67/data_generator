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

        public string InsertData()
        {
            return "INSERT INTO order VALUES(:order_id,:customer_id,:country_id,:total_price,:date); " +
                "INSERT INTO order_details VALUES(:orderdetails_id,:order_id,:candy_id,:quantity);";
        }
    }
}
