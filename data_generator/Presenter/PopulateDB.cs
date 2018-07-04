using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using data_generator.Model;

namespace data_generator.Presenter
{
    class PopulateDB
    {
        public static List<Candy> candy = new List<Candy>();
        public static List<CandyColor> color = new List<CandyColor>();
        public static List<CandyVariant> variant = new List<CandyVariant>();
        public static List<CandyTexture> texture = new List<CandyTexture>();
        public static List<CandyPackaging> packaging = new List<CandyPackaging>();

        DataAccess da = new DataAccess();
        DataGenerator dg = new DataGenerator();

        //.CSV paramètres du fichier csv
        private string delimiter = ";";
        private string commentToken = "#";
        private bool isFieldInQuotes = false;
        private bool skipHeader = false;
        private TextFieldParser csvParser;

        public void OpenCandyCsvFile()
        {
            //ouvre une fenêtre de recherche de fichier avec le filtre .csv
            OpenFileDialog xlsxPath = new OpenFileDialog
            {
                Filter = "fichier .csv | *.csv"
            };

            //Si un fichier est sélectionné
            if (xlsxPath.ShowDialog() == DialogResult.OK)
            {
                Task insertCSV;
                using (csvParser = new TextFieldParser(xlsxPath.FileName))
                {
                    //Paramètres du parser csv
                    csvParser.CommentTokens = new string[] { commentToken };
                    csvParser.SetDelimiters(new string[] { delimiter });
                    csvParser.HasFieldsEnclosedInQuotes = isFieldInQuotes;
                    if (skipHeader)
                    {
                        csvParser.ReadLine();
                    }

                    //lance la tâche
                    //pour parser et inserer les données
                    insertCSV = Task.Run(() => { PrepareCandyData(); });
                    insertCSV.Wait();
                }
            }
        }

        public void OpenCountryShippingCsvFile()
        {
            //ouvre une fenêtre de recherche de fichier avec le filtre .csv
            OpenFileDialog xlsxPath = new OpenFileDialog
            {
                Filter = "fichier .csv | *.csv"
            };

            //Si un fichier est sélectionné
            if (xlsxPath.ShowDialog() == DialogResult.OK)
            {
                Task insertCSV;
                using (csvParser = new TextFieldParser(xlsxPath.FileName))
                {
                    //Paramètres du parser csv
                    csvParser.CommentTokens = new string[] { commentToken };
                    csvParser.SetDelimiters(new string[] { delimiter });
                    csvParser.HasFieldsEnclosedInQuotes = isFieldInQuotes;
                    if (skipHeader)
                    {
                        csvParser.ReadLine();
                    }

                    //lance la tâche
                    //pour parser et inserer les données
                    insertCSV = Task.Run(() => { PrepareCountryShippingData(); });
                    insertCSV.Wait();
                }
            }
        }

        public void OpenMachineCondiCsvFile()
        {
            //ouvre une fenêtre de recherche de fichier avec le filtre .csv
            OpenFileDialog xlsxPath = new OpenFileDialog
            {
                Filter = "fichier .csv | *.csv"
            };

            //Si un fichier est sélectionné
            if (xlsxPath.ShowDialog() == DialogResult.OK)
            {
                Task insertCSV;
                using (csvParser = new TextFieldParser(xlsxPath.FileName))
                {
                    //Paramètres du parser csv
                    csvParser.CommentTokens = new string[] { commentToken };
                    csvParser.SetDelimiters(new string[] { delimiter });
                    csvParser.HasFieldsEnclosedInQuotes = isFieldInQuotes;
                    if (skipHeader)
                    {
                        csvParser.ReadLine();
                    }

                    //lance la tâche
                    //pour parser et inserer les données
                    insertCSV = Task.Run(() => { PrepareMachineCondi(); });
                    insertCSV.Wait();
                }
            }
        }

        private void PrepareCandyData()
        {
            int id = 1;
            while (!csvParser.EndOfData)
            {
                try
                {
                    string[] fields = csvParser.ReadFields();
                    for (int i = 0; i <= fields.Count() - 1; i++)
                    {
                        if (i == 0 && fields[i] != "")
                        {
                            candy.Add(new Candy
                            {
                                id = id,
                                name = fields[0],
                                manufacturing_cost = Convert.ToInt32(fields[1]),
                                packaging_cost = Convert.ToInt32(fields[2]),
                                sending_cost = Convert.ToInt32(fields[3]),
                                general_cost = Convert.ToInt32(fields[4]),
                                bag_price = fields[5],
                                box_price = fields[6],
                                sample_price = fields[7],
                                additive = fields[8],
                                coating = fields[9],
                                aroma = fields[10],
                                gelling = fields[11],
                                sugar = fields[12]
                            });
                        }
                        else if (i == 13 && fields[i] != "")
                        {
                            color.Add(new CandyColor
                            {
                                id = id,
                                name = fields[13]
                            });
                        }
                        else if (i == 14 && fields[i] != "")
                        {
                            variant.Add(new CandyVariant
                            {
                                id = id,
                                name = fields[14]
                            });
                        }
                        else if (i == 15 && fields[i] != "")
                        {
                            texture.Add(new CandyTexture
                            {
                                id = id,
                                name = fields[15]
                            });
                        }
                        else if (i == 16 && fields[i] != "")
                        {
                            packaging.Add(new CandyPackaging
                            {
                                id = id,
                                name = fields[16]
                            }); ;
                        }
                    }
                    id++;
                }
                catch (IndexOutOfRangeException)
                {
                    throw;
                }
            }
            //da.InsertData();
            dg.GenerateCandyReference();
        }

        private void PrepareMachineCondi()
        {
            List<MachinePackaging> machineCondi = new List<MachinePackaging>();
            

            int id = 1;
            while (!csvParser.EndOfData)
            {
                try
                {
                    string[] fields = csvParser.ReadFields();
                    for (int i = 0; i <= fields.Count() - 1; i++)
                    {
                        if (i == 0 && fields[i] != "")
                        {
                            machineCondi.Add(new MachinePackaging
                            {
                                Machine_id = id,
                                Packaging_id = Convert.ToInt32(fields[1]),
                                Cadence = Convert.ToInt32(fields[2]),
                                Tool_change = Convert.ToInt32(fields[3])
                            });
                        }
                    }
                    id++;
                }
                catch (IndexOutOfRangeException)
                {
                    throw;
                }
            }
            da.InsertCondiMachine(machineCondi);
        }

        private void PrepareCountryShippingData()
        {
            List<Country> country = new List<Country>();
            List<Shipping> shipping = new List<Shipping>();

            /*List<Container> container = new List<Container>();
            List<Carton> carton = new List<Carton>();
            List<Pallets> pallets = new List<Pallets>();
            List<CandyPackaging> candyPackaging = new List<CandyPackaging>();

            List<MachinePackaging> machinePackaging = new List<MachinePackaging>();
            List<MachineManufacture> machineManufacture = new List<MachineManufacture>();*/

            int id = 1;
            while (!csvParser.EndOfData)
            {
                try
                {
                    string[] fields = csvParser.ReadFields();
                    for (int i = 0; i <= fields.Count() - 1; i++)
                    {
                        if (i == 0 && fields[i] != "")
                        {
                            shipping.Add(new Shipping
                            {
                                Shipping_id = id,
                                Name = fields[0],
                                Quantity = Convert.ToInt32(fields[1])
                            });
                        }
                        else if (i == 2 && fields[i] != "")
                        {
                            country.Add(new Country
                            {
                                Country_id = id,
                                Name = fields[2],
                                Shipping_id = Convert.ToInt32(fields[3])
                            });
                        }
                        /*else if (i == 0 && fields[i] != "")
                        {
                            candyPackaging.Add(new CandyPackaging
                            {
                                id = id,
                                name = fields[0]
                            });
                        }
                        else if (i == 13 && fields[i] != "")
                        {
                            container.Add(new Container
                            {
                                Container_id = id,
                                Packaging_id = Convert.ToInt32(from lid in candyPackaging where lid.name == fields[9] select lid.id),
                                Quantity = Convert.ToInt32(fields[5])
                            });
                        }
                        else if (i == 13 && fields[i] != "")
                        {
                            pallets.Add(new Pallets
                            {
                                Pallets_id = id,
                                Shipping_id = Convert.ToInt32(from lid in shipping where lid.Name == fields[9] select lid.Shipping_id),
                                Quantity = Convert.ToInt32(fields[9])

                            });
                        }
                        else if (i == 13 && fields[i] != "")
                        {
                            machinePackaging.Add(new MachinePackaging
                            {
                                Machine_id = id,
                                Cadence = Convert.ToInt32(fields[9]),
                                Tool_change = Convert.ToInt32(fields[9]),
                                Packaging_id = Convert.ToInt32(from lid in candyPackaging where lid.name == fields[9] select lid.id)
                                //Convert.ToInt32(candyPackaging.Select(c => c.id).Where( = fields[10])
                            });
                        }*/
                    }
                    id++;
                }
                catch (IndexOutOfRangeException)
                {
                    throw;
                }
            }
            da.InsertCountryShippingData(country, shipping);
        }
    }
}