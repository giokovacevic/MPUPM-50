using FTN.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel;
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
using WPFClient.ClientConnection;

namespace WPFClient
{
    public class ElementModel
    {
        public long GID { get; set; }
        public string GidHex { get; set; }
        public string Name { get; set;  }
    }

    public class ObjectItem
    {
        public long GID { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private DMSType selectedElementType;
        private List<DMSType> comboBoxElementTypes;

        public List<ObjectItem> ComboBoxRelElements { get; set; } = new List<ObjectItem>();
        //public List<DMSType> ComboBoxRelAttributes { get; set; } = new List<DMSType>();
        public List<ModelCode> ComboBoxRels { get; set; } = new List<ModelCode>();
        
        private string selectedRel;

        private GDAClient gdaClient;
        private ModelResourcesDesc modelDesc = new ModelResourcesDesc();

        public MainWindow()
        {
            InitializeComponent();

            this.gdaClient = new GDAClient();
            
            ComboBoxElementTypes = new List<DMSType>() { DMSType.TERMINAL, DMSType.POWERTREND, DMSType.POWERTR, DMSType.TAPCHANGER, DMSType.TAPCHANGERCTRL };

            AllUsedElements();

            //ComboBoxRelAttributes = new List<DMSType>();

            this.DataContext = this;

        }

        public DMSType SelectedElementType { get => selectedElementType; set => selectedElementType = value; }
        public List<DMSType> ComboBoxElementTypes { get => comboBoxElementTypes; set => comboBoxElementTypes = value; }
        
        public string SelectedRel { get => selectedRel; set => selectedRel = value; }

        private void cb_element_types_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_element_types.SelectedItem == null) return;

            try
            {
                // 1. Dobijamo tip iz ComboBox-a
                DMSType selectedType = (DMSType)cb_element_types.SelectedItem;

                // 2. Tražimo samo osnovno za tabelu (GID i Name)
                List<ModelCode> propsForTable = new List<ModelCode> { ModelCode.IDOBJ_GID, ModelCode.IDOBJ_NAME };

                // 3. POZIV NOVE METODE
                List<ResourceDescription> rds = gdaClient.GetExtentValuesObjects(selectedType, propsForTable);

                // 4. Pretvaramo u model koji DataGrid razume
                List<ElementModel> listaZaTabelu = new List<ElementModel>();
                foreach (var rd in rds)
                {
                    // Izvlacimo IDOBJ_NAME iz Properija
                    var nameProp = rd.Properties.FirstOrDefault(p => p.Id == ModelCode.IDOBJ_NAME);
                    string nameVal = (nameProp != null) ? nameProp.PropertyValue.StringValue : "N/A";

                    listaZaTabelu.Add(new ElementModel
                    {
                        GID = rd.Id,
                        GidHex = String.Format("0x{0:x16}", rd.Id),
                        Name = nameVal
                    });
                }

                // 5. Punimo tabelu
                dg_elements.ItemsSource = listaZaTabelu;
                richTextBox_2.Text = "Kliknite na element u tabeli za detalje...";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška: " + ex.Message);
            }
        }

        private void dg_elements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 1. Proveravamo da li je korisnik zaista kliknuo na red
            if (dg_elements.SelectedItem is ElementModel selectedElement)
            {
                try
                {
                    long gid = selectedElement.GID;

                    // 1. Dobijamo DMSType iz GID-a
                    DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid);

                    // 2. Korišćenje tvoje MOĆNE metode koja vraća APSOLUTNO SVE (uključujući nasleđeno)
                    // Pravimo instancu jer metoda nije statička (ili koristi tvoju postojeću instancu ako je imaš)
               
                    List<ModelCode> allProperties = modelDesc.GetAllPropertyIds(type);

                    // 3. Poziv klijenta sa kompletnom listom
                    string detaljanIspis = gdaClient.GetValues(gid, allProperties);

                    // 4. Ispis u belo polje
                    richTextBox_2.Text = detaljanIspis;
                }
                catch (Exception ex)
                {
                    richTextBox_2.Text = "Greška pri učitavanju: " + ex.Message;
                }
            }
        }
        //cbRelElement_SelectionChanged

        private void cbRelElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Proveravamo SelectedValue jer smo u XAML-u stavili SelectedValuePath="GID"
            if (cbRelElements.SelectedValue is long gid)
            {
                try
                {
                    DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid);
                    List<ModelCode> allProps = modelDesc.GetAllPropertyIds(type);

                    List<ModelCode> rels = new List<ModelCode>();
                    foreach (ModelCode p in allProps)
                    {
                        PropertyType propType = Property.GetPropertyType(p);
                        if (propType == PropertyType.Reference || propType == PropertyType.ReferenceVector)
                        {
                            rels.Add(p);
                        }
                    }

                    cbRel.ItemsSource = rels; // Ovde punimo drugi combo
                    if (rels.Count > 0) cbRel.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška: " + ex.Message);
                }
            }
        }

        private void cbRel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRel.SelectedItem is ModelCode relationCode)
            {
                try
                {
                    // Tvoja staticka metoda nalazi tip na drugoj strani veze
                    DMSType targetType = ModelResourcesDesc.GetTypeFromModelCode(relationCode);
                    List<ModelCode> targetAttributes = modelDesc.GetAllPropertyIds(targetType);

                    //cbRelAttributes.ItemsSource = targetAttributes;
                    //if (targetAttributes.Count > 0) cbRelAttributes.SelectedIndex = 0;
                }
                catch
                {
                    //cbRelAttributes.ItemsSource = new List<ModelCode> { ModelCode.IDOBJ_NAME, ModelCode.IDOBJ_GID };
                }
            }
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (cbRelElements.SelectedValue == null || cbRel.SelectedItem == null) return;

            long sourceGid = (long)cbRelElements.SelectedValue;
            ModelCode relationProperty = (ModelCode)cbRel.SelectedItem;

            // Lista atributa koje želiš da izvučeš za POVEZANE objekte
            // Na primer: ako tražiš krajeve trafoa, želiš njihova imena
            List<ModelCode> columns = new List<ModelCode> { ModelCode.IDOBJ_NAME, ModelCode.IDOBJ_GID };

            // Ovde primenjujemo tvoja pravila 2/3:
            Association association = new Association()
            {
                PropertyId = relationProperty,
                Type = 0,             // 0 znači: "Daj mi sve, ne filtriraj po tipu"
                Inverse = false       // Kao što piše, nije od interesa, pa ostavi false
            };

            // Poziv klijenta
            string result = gdaClient.GetRelatedValues(sourceGid, association, columns);

            if (string.IsNullOrEmpty(result))
            {
                //richTextBox_results2.Document.Blocks.Clear();
                richTextBox_results2.Clear();
                richTextBox_results2.AppendText("Rezultat je prazan. Proveri da li ovaj objekat ima popunjenu relaciju.");
            }
            else
            {
                richTextBox_results2.Clear();
                //richTextBox_results2.Document.Blocks.Clear();
                richTextBox_results2.AppendText(result);
            }
        }

        private void AllUsedElements()
        {
            try
            {
                foreach (DMSType type in ComboBoxElementTypes)
                {
                    List<ResourceDescription> rds = gdaClient.GetExtentValuesObjects(type, new List<ModelCode> { ModelCode.IDOBJ_NAME });
                    if (rds != null)
                    {
                        foreach (var rd in rds)
                        {
                            ComboBoxRelElements.Add(new ObjectItem
                            {
                                GID = rd.Id,
                                Name = rd.Properties.FirstOrDefault(p => p.Id == ModelCode.IDOBJ_NAME)?.PropertyValue.StringValue ?? "N/A"
                            });
                        }
                    }
                }

                // Provera - ako je i dalje 0, ispisaće poruku
                if (ComboBoxRelElements.Count == 0)
                {
                    MessageBox.Show("Baza je prazna ili GDA servis ne odgovara.");
                }

                // Ručno osvežavanje izvora za svaki slučaj
                cbRelElements.ItemsSource = null;
                cbRelElements.ItemsSource = ComboBoxRelElements;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri inicijalizaciji: " + ex.Message);
            }
        }
    }
}
