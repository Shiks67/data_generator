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
            {
                Gen_btn.Background = Brushes.DarkRed;
                dg.GenerateOrder(Convert.ToInt32(nb_order.Text), Convert.ToInt32(nb_min.Text), Convert.ToInt32(nb_max.Text), order_date.Text);
                Gen_btn.Background = Brushes.WhiteSmoke;
            }
        }

        private void Populate_btn_Click(object sender, RoutedEventArgs e)
        {
            Populate_btn.Background = Brushes.DarkRed;
            pDB.OpenCandyCsvFile();
            Populate_btn.Background = Brushes.WhiteSmoke;
        }

        private void Add_country_shipping_Click(object sender, RoutedEventArgs e)
        {
            Add_country_shipping.Background = Brushes.DarkRed;
            pDB.OpenCountryShippingCsvFile();
            Add_country_shipping.Background = Brushes.WhiteSmoke;
        }

        private void Add_machine_condi_Click(object sender, RoutedEventArgs e)
        {
            Add_machine_condi.Background = Brushes.DarkRed;
            pDB.OpenMachineCondiCsvFile();
            Add_machine_condi.Background = Brushes.WhiteSmoke;
        }

        private void gen_machine_use_Click(object sender, RoutedEventArgs e)
        {
            gen_machine_use.Background = Brushes.DarkRed;
            dg.GenerateMachineUse();
            gen_machine_use.Background = Brushes.WhiteSmoke;
        }
    }
}
