using data_generator.Presenter;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace data_generator.View
{
    /// <summary>
    /// Logique d'interaction pour MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        DataGenerator dg = new DataGenerator();

        private void Gen_btn_Click(object sender, RoutedEventArgs e)
        {
            //if(nb_order.Text != "")
            //{dg.GenerateOrder(Convert.ToInt32(nb_order.Text));}
            string connectionString = "Data Source=(DESCRIPTION=(CID=GTU_APP)(ADDRESS_LIST=(ADDRESS=" +
                "(PROTOCOL=TCP)(HOST=192.168.43.105)(PORT=1521)))" +
                "(CONNECT_DATA=(SID=PBigData) (SERVER=DEDICATED)));" +
                "User Id=Generateur_Donnes;Password=azer123*";
            //string cnxstring = "User Id=Generateur_Donnes@192.168.43.105;Password=azer123";
            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("ça marche");

                }
                catch
                {
                    MessageBox.Show("ça marche pas");
                    throw;

                }
            }
        }

        private void Populate_btn_Click(object sender, RoutedEventArgs e)
        {
            PopulateDB pDB = new PopulateDB();
            pDB.OpenCsvFile();
        }
    }
}
