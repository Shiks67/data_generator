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
        DataAccess da;

        public static List<Order> oList;
        private List<CandyReference> allCandys = new List<CandyReference>();

        public void GenerateOrder(int nbOrder)
        {
            oList = new List<Order>();
            da.GetDataRows();
            Random rnd = new Random();

            for (int i = 0; i < nbOrder; i++)
            {
                oList.Add(new Order
                {
                    order_id = DataAccess.nbOrder + i + 1,
                    customer_id = rnd.Next(1, (nbOrder / 4)),
                    country_id = rnd.Next(1, DataAccess.nbCountry),
                    total_price = rnd.Next(1, 50),
                    date = "" + rnd.Next(1, 28) + "" + rnd.Next(1, 12) + "" + rnd.Next(2015, 218),
                    order_details_id = i + 1,
                    candy_ref_id = rnd.Next(1, DataAccess.nbCandy),
                    quantity = rnd.Next(1, 50)
                });
            }

            da = new DataAccess();
            da.InsertOrder(oList);
            oList.Clear();
        }

        public void GenerateCandyReference()
        {
            int id = 0;
            for (int a = 0; a < PopulateDB.candy.Count(); a++)
            {
                for (int b = 0; b < PopulateDB.color.Count(); b++)
                {
                    for (int c = 0; c < PopulateDB.variant.Count(); c++)
                    {
                        for (int d = 0; d < PopulateDB.texture.Count(); d++)
                        {
                            for (int e = 0; e < PopulateDB.packaging.Count(); e++)
                            {
                                id++;
                                allCandys.Add(new CandyReference
                                {
                                    Candy_ref_id = id,
                                    Candy_id = PopulateDB.candy[a].id,
                                    Color_id = PopulateDB.color[b].id,
                                    Variant_id = PopulateDB.variant[c].id,
                                    Texture_id = PopulateDB.texture[d].id,
                                    Packaging_id = PopulateDB.packaging[e].id
                                });
                            }
                        }
                    }
                }
            }
            da.InsertAllCandyRef(allCandys);
        }
    }
}
