using data_generator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace data_generator.Presenter
{
    class DataGenerator
    {
        DataAccess da = new DataAccess();

        private List<Order> oList = new List<Order>();
        private List<OrderDetails> odList = new List<OrderDetails>();
        private List<CandyReference> allCandys = new List<CandyReference>();
        private List<CandyRefPrice> crp = new List<CandyRefPrice>();

        public void GenerateOrder(int nbOrder, int nbMin, int nbMax, string odate)
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
                    date = odate
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

        List<CandySent> candySent = new List<CandySent>();
        List<Container> nb_package = new List<Container>();
        List<MachineManufacture> machineManufacture = new List<MachineManufacture>();
        List<MachinePackaging> machinePackaging = new List<MachinePackaging>();
        List<MachineWork> mmWork = new List<MachineWork>();
        List<MachineWork> mpWork = new List<MachineWork>();

        public void GenerateMachineUse()
        {
            candySent = da.GetCandyData();
            nb_package = da.GetPackageData();
            machineManufacture = da.GetMachineManufactureData();
            machinePackaging = da.GetMachinePackagingData();

            int id = 0;
            Random rnd = new Random();

            foreach (var cs in candySent)
            {
                id++;

                string d = rnd.Next(Convert.ToInt32(cs.Date.Split('/')[0])).ToString();
                string m = cs.Date.Split('/')[1];
                string y = cs.Date.Split('/')[2];

                string date = (Convert.ToInt32(d) + 1).ToString() + "/" + m + "/" + y + " 7:00:00";

                var machinesm = machineManufacture.Where(mm => mm.candy_variant_id == cs.Variant_id).ToList();
                int machineToUse = rnd.Next(machinesm.Count);
                int nb_candy = nb_package.Single(nbp => nbp.Packaging_id == cs.Package_id).Quantity * cs.Quantity;

                var machinesp = machinePackaging.Where(mm => mm.Packaging_id == cs.Variant_id).ToList();
                int machinepToUse = rnd.Next(machinesp.Count);
                string datef = "";

                while (date != datef)
                {
                    datef = date;
                    date = Verify_mmCadence(date, (machinesm)[machineToUse].Machine_id, (machinesm)[machineToUse].candy_variant_id);
                    date = Verify_mpCadence(date, (machinesp)[machinepToUse].Machine_id);
                }

                mmWork.Add(new MachineWork
                {
                    Id = id,
                    Machine_id = (machinesm)[machineToUse].Machine_id,
                    Candy_ref_id = cs.Candy_ref_id,
                    Quantity = nb_candy,
                    Date = date
                });
                
                mpWork.Add(new MachineWork
                {
                    Id = id,
                    Machine_id = (machinesp)[machinepToUse].Machine_id,
                    Candy_ref_id = cs.Candy_ref_id,
                    Quantity = cs.Quantity,
                    Date = date
                });
            }
            da.InsertMachineWork(mpWork,mmWork);
        }

        public string Verify_mmCadence(string date, int machineToUse, int variant_id)
        {
            var rnd = new Random();
            var cadenceDone = mmWork.Where(mm => mm.Date == date).Where(mm => mm.Machine_id == machineToUse).ToList();
            int total = cadenceDone.Sum(smm => smm.Quantity);

            var mfirst = machineManufacture.Where(cm => cm.Machine_id == machineToUse).ToList();
            string mfirstdisp = mfirst[rnd.Next(mfirst.Count)].Cadence;

            if(mfirstdisp.Contains('/'))
            {
                if(variant_id == 2)
                {
                    mfirstdisp = mfirstdisp.Split('/')[0];
                }
                else
                {
                    mfirstdisp = mfirstdisp.Split('/')[1];
                }
            }

            if (Convert.ToInt32(mfirstdisp) < total)
            {
                string day = date.Split(' ')[0];
                string time = date.Split(' ')[1];
                string m, d, y;
                int hour = Convert.ToInt32(time.Split(':')[0]);
                int minute = Convert.ToInt32(time.Split(':')[1]);

                if(minute == 59)
                {
                    hour++;
                    minute = 0;
                }
                else
                {
                    minute++;
                }

                if(hour == 20)
                {
                    d = day.Split('/')[0];
                    m = day.Split('/')[1];
                    y = day.Split('/')[2];

                    if(Convert.ToUInt32(d) == 30)
                    {
                        if (Convert.ToUInt32(m) == 12)
                        {
                            return day = "1/1/" + (Convert.ToInt32(y) + 1).ToString() + " 7:00:00";
                        }
                        return day = "1/" + (Convert.ToInt32(m) + 1).ToString() + "/" + y + " 7:00:00";
                    }
                    else
                    {
                        return day = (Convert.ToInt32(d) + 1).ToString() + "/" + m + "/" + y + " 7:00:00";
                    }
                }
                return day + " " + hour + ":" + minute + ":00";
            }
            return date;
        }

        public string Verify_mpCadence(string date, int machineToUse)
        {
            var cadenceDone = mpWork.Where(mm => mm.Date == date).Where(mm => mm.Machine_id == machineToUse).ToList();
            int total = cadenceDone.Sum(smm => smm.Quantity);

            if (Convert.ToInt32(machinePackaging.Single(cm => cm.Machine_id == machineToUse).Cadence) < total)
            {
                string day = date.Split(' ')[0];
                string time = date.Split(' ')[1];
                string m, d, y;
                int hour = Convert.ToInt32(time.Split(':')[0]);
                int minute = Convert.ToInt32(time.Split(':')[1]);

                if (minute == 59)
                {
                    hour++;
                    minute = 0;
                }
                else
                {
                    minute++;
                }

                if (hour == 20)
                {
                    d = day.Split('/')[0];
                    m = day.Split('/')[1];
                    y = day.Split('/')[2];

                    if (Convert.ToUInt32(d) == 30)
                    {
                        if(Convert.ToUInt32(m) == 12)
                        {
                            return day = "1/1/" + (Convert.ToInt32(y) + 1).ToString() + " 7:00:00";
                        }
                        return day = "1/" + (Convert.ToInt32(m) + 1).ToString() + "/" + y + " 7:00:00";
                    }
                    else
                    {
                        return day = (Convert.ToInt32(d) + 1).ToString() + "/" + m + "/" + y + " 7:00:00";
                    }
                }
                return day + " " + hour + ":" + minute + ":00";
            }
            return date;
        }
    }
}
