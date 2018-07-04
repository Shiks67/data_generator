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
        DataAccess da = new DataAccess();

        private List<Order> oList = new List<Order>();
        private List<OrderDetails> odList = new List<OrderDetails>();

        private List<CandyReference> allCandys = new List<CandyReference>();

        public void GenerateOrder(int nbOrder, int nbMin, int nbMax)
        {
            da.GetDataRows();
            Random rnd = new Random();
            int odID = DataAccess.nbOD;

            for (int i = 0; i < nbOrder; i++)
            {
                int ordId = DataAccess.nbOrder + i + 1;
                oList.Add(new Order
                {
                    order_id = ordId,
                    customer_id = rnd.Next(1, (nbOrder)),
                    country_id = rnd.Next(1, DataAccess.nbCountry),
                    total_price = rnd.Next(1, 50),
                    date = rnd.Next(01, 28) + "/" + rnd.Next(01, 12) + "/" + rnd.Next(2015, 2018),
                    order_details_id = i + 1,
                    candy_ref_id = rnd.Next(1, DataAccess.nbCandy),
                    quantity = rnd.Next(10, 30)
                });
                int lines = rnd.Next(nbMin, nbMax);
                for (int y = 0; y < lines; y++)
                {
                    odID++;
                    odList.Add(new OrderDetails
                    {
                        order_details_id = odID,
                        candy_ref_id = rnd.Next(1, DataAccess.nbCandy),
                        order_id = ordId,
                        quantity = rnd.Next(1, 5)
                    });
                }
            }
            da.InsertOrder(oList, odList);
            oList.Clear();
        }

        public void GenerateCandyReference()
        {
            Random rnd = new Random();

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
                                    Packaging_id = PopulateDB.packaging[e].id,
                                    Candy_quantity = rnd.Next(20, 60),
                                    Candy_ref_code = rnd.Next(100,999999999)
                                });
                            }
                        }
                    }
                }
            }
            da.InsertAllCandyRef(allCandys);
        }

        public void GenerateMachineUse()
        {
            List<CandySent> candySent = new List<CandySent>();
            candySent = da.GetCandyData();
        }
    }
}
