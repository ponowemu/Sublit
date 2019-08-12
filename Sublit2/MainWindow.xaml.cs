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

namespace Sublit2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Subiekt su;
        private List<Document> documentsList = null;
        public MainWindow()
        {
            InitializeComponent();

            Subiekt subiekt = new Subiekt();
            subiekt.Connect();
            su = subiekt;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var db = new Database();
            var documents = db.GetDocuments();
            list.ItemsSource = documents;
            documentsList = documents;
        }
        private void SearchDocument(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                list.ItemsSource = null;
                list.ItemsSource = documentsList.Where(d => d.Number.ToUpper().Replace(" ","").Contains(search.Text.ToUpper().Replace(" ",""))).ToList();
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(documentsList == null || documentsList.Count == 0)
                MessageBox.Show("Nie możesz pobrać dokumentów dla pustej listy", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                var list = documentsList.Where(d => d.Checked == true).ToList();
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    var path = dialog.SelectedPath;
                    try
                    {
                        foreach(var el in list)
                        {
                            MessageBox.Show(path);
                            var res = su.GenerateInvoice(el.Number, path);
                            
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
