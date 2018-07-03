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

        public void OpenCsvFile()
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
                    insertCSV = Task.Run(() => { PrepareData(); });
                    insertCSV.Wait();
                }
            }
        }

        private void PrepareData()
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
                                name = fields[0]
                            });
                        }
                        else if (i == 14 && fields[i] != "")
                        {
                            variant.Add(new CandyVariant
                            {
                                id = id,
                                name = fields[0]
                            });
                        }
                        else if (i == 15 && fields[i] != "")
                        {
                            texture.Add(new CandyTexture
                            {
                                id = id,
                                name = fields[0]
                            });
                        }
                        else if (i == 16 && fields[i] != "")
                        {
                            packaging.Add(new CandyPackaging
                            {
                                id = id,
                                name = fields[0]
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
            da.InsertData();
            dg.GenerateCandyReference();
        }
    }
}