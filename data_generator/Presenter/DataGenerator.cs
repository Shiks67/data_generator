using data_generator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Presenter
{
    class DataGenerator
    {
        Order o;
        DataAccess da;

        public static List<Order> oList;

        public DataGenerator(int nbOrder)
        {
            Random rnd = new Random();

            for (int i = 0; i < nbOrder; i++)
            {
                oList.Add(new Order
                {
                    order_id = DataAccess.nbOrder + i + 1,
                    customer_id = rnd.Next(1, (nbOrder / 4)),
                    country_id = rnd.Next(1, DataAccess.nbCountry),
                    total_price = 45,
                    date = "" + rnd.Next(1, 28) + "" + rnd.Next(1, 12) + "" + rnd.Next(2015, 218),
                    order_details_id = i + 1,
                    candy_id = rnd.Next(1, DataAccess.nbCandy),
                    quantity = rnd.Next(1, 50)
                });
            }

            da = new DataAccess();
            da.InsertData(oList);
            oList.Clear();
        }
    }
}
