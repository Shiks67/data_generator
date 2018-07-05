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
        private List<CandyRefPrice> crp = new List<CandyRefPrice>();

        public void GenerateOrder(int nbOrder, int nbMin, int nbMax)
        {
            crp = da.GetDataRows();
            Random rnd = new Random();
            int odID = 0;

            for (int i = 0; i < nbOrder; i++)
            {
                int ordId = DataAccess.nbOrder + i + 1;
                int lines = rnd.Next(nbMin, nbMax);
                double totalorderPrice = 0;

                for (int y = 0; y < lines; y++)
                {
                    odID++;
                    int refId = rnd.Next(1, DataAccess.nbCandy);
                    int refq = rnd.Next(1, 5);
                    odList.Add(new OrderDetails
                    {
                        order_line = odID,
                        candy_ref_id = refId,
                        order_id = ordId,
                        quantity = refq
                    });
                    totalorderPrice += Convert.ToDouble(crp.Single(x => x.Candy_ref_id == refId).Price) * refq;
                }
                odID = 0;

                oList.Add(new Order
                {
                    order_id = ordId,
                    customer_id = rnd.Next(1, (nbOrder)),
                    country_id = rnd.Next(1, DataAccess.nbCountry),
                    total_price = totalorderPrice,
                    date = rnd.Next(01, 28) + "/" + rnd.Next(01, 12) + "/" + rnd.Next(2015, 2018),
                });
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
                                    Candy_id = PopulateDB.candy[a].candy_id,
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

            List<MachineManufacture> machineManufacture = new List<MachineManufacture>();
            machineManufacture = da.GetMachineManufactureData();

            List<MachinePackaging> machinePackaging = new List<MachinePackaging>();
            machinePackaging = da.GetMachinePackagingData();

            List<MachineWork> mmWork = new List<MachineWork>();
            List<MachineWork> mpWork = new List<MachineWork>();

            int id = 0;
            Random rnd = new Random();

            foreach (var cs in candySent)
            {
                id++;

                string d = rnd.Next(Convert.ToInt32(cs.Date.Split('/')[0])).ToString();
                string m = cs.Date.Split('/')[1];
                string y = cs.Date.Split('/')[2];

                string date = d + "/" + m + "/" + y + " " + rnd.Next(0, 23) + ":" + rnd.Next(0, 59) + ":" + rnd.Next(0, 59);

                var machinesm = machineManufacture.Where(mm => mm.candy_variant_id == cs.Variant_id).ToList();
                int machineToUse = rnd.Next(machinesm.Count);
                mmWork.Add(new MachineWork
                {
                    Id = id,
                    Machine_id = (machinesm)[machineToUse].Machine_id,
                    Candy_ref_id = cs.Candy_ref_id,
                    Date = date
                });
                var machinesp = machinePackaging.Where(mm => mm.Packaging_id == cs.Variant_id).ToList();
                machineToUse = rnd.Next(machinesp.Count);
                mpWork.Add(new MachineWork
                {
                    Id = id,
                    Machine_id = (machinesp)[machineToUse].Machine_id,
                    Candy_ref_id = cs.Candy_ref_id,
                    Date = date
                });
            }
            da.InsertMachineWork(mpWork,mmWork);
        }
    }
}
