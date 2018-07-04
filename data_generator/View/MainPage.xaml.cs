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
        PopulateDB pDB = new PopulateDB();

        private void Gen_btn_Click(object sender, RoutedEventArgs e)
        {
            if(nb_order.Text != "")
            {dg.GenerateOrder(Convert.ToInt32(nb_order.Text), Convert.ToInt32(nb_min.Text), Convert.ToInt32(nb_max.Text));}
        }

        private void Populate_btn_Click(object sender, RoutedEventArgs e)
        {
            pDB.OpenCandyCsvFile();
        }

        private void Add_country_shipping_Click(object sender, RoutedEventArgs e)
        {
            pDB.OpenCountryShippingCsvFile();
        }

        private void Add_machine_condi_Click(object sender, RoutedEventArgs e)
        {
            pDB.OpenMachineCondiCsvFile();
        }

        private void gen_machine_use_Click(object sender, RoutedEventArgs e)
        {
            dg.GenerateMachineUse();
        }
    }
}
