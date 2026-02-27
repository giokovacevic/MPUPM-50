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
                DMSType selectedType = (DMSType)cb_element_types.SelectedItem;
                List<ModelCode> propsForTable = new List<ModelCode> { ModelCode.IDOBJ_GID, ModelCode.IDOBJ_NAME };

                List<ResourceDescription> rds = gdaClient.GetExtentValuesObjects(selectedType, propsForTable);

                List<ElementModel> listaZaTabelu = new List<ElementModel>();
                foreach (ResourceDescription rd in rds)
                {
                    Property nameProp = rd.Properties.FirstOrDefault(p => p.Id == ModelCode.IDOBJ_NAME);
                    string nameVal = (nameProp != null) ? nameProp.PropertyValue.StringValue : "undf";

                    listaZaTabelu.Add(new ElementModel{GID = rd.Id, GidHex = String.Format("0x{0:x16}", rd.Id), Name = nameVal});
                }

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
            if (dg_elements.SelectedItem is ElementModel selectedElement)
            {
                try
                {
                    long gid = selectedElement.GID;

                    DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gid);
                    List<ModelCode> allProperties = modelDesc.GetAllPropertyIds(type);

                    string detaljanIspis = gdaClient.GetValues(gid, allProperties);

                    richTextBox_2.Text = detaljanIspis;
                }
                catch (Exception ex)
                {
                    richTextBox_2.Text = "Greška pri učitavanju: " + ex.Message;
                }
            }
        }


        private void cbRelElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

                    cbRel.ItemsSource = rels; 
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
                    DMSType targetType = ModelResourcesDesc.GetTypeFromModelCode(relationCode);
                    List<ModelCode> targetAttributes = modelDesc.GetAllPropertyIds(targetType);

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

            List<ModelCode> columns = new List<ModelCode> { ModelCode.IDOBJ_NAME, ModelCode.IDOBJ_GID };

            Association association = new Association(){PropertyId = relationProperty, Type = 0, Inverse = false  };

            string result = gdaClient.GetRelatedValues(sourceGid, association, columns);

            if (string.IsNullOrEmpty(result))
            {
                richTextBox_results2.Clear();
                richTextBox_results2.AppendText("Rezultat je prazan. Proveri da li ovaj objekat ima popunjenu relaciju.");
            }
            else
            {
                richTextBox_results2.Clear();
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

                if (ComboBoxRelElements.Count == 0)
                {
                    MessageBox.Show("Baza je prazna ili GDA servis ne odgovara.");
                }

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
