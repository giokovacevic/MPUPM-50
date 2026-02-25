using FTN.Common;
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
using TelventDMS.Services.NetworkModelService.TestClient.Tests;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestGda testGda;

        public MainWindow()
        {
            InitializeComponent();

            testGda = new TestGda();
        }

        private void Button_ClickTest(object sender, RoutedEventArgs e)
        {
            try
            {
                // Pozivamo metodu iz onog fajla koji si mi poslao
                // Uzimamo sve GID-ove za PowerTransformer-e
                List<long> rezultati = testGda.GetExtentValues(ModelCode.POWERTR);

                // Ovde možeš da ih ispišeš negde da proveriš da li radi
                MessageBox.Show($"Pronađeno {rezultati.Count} transformatora!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Greška pri povezivanju: " + ex.Message);
            }
        }
    }
}
