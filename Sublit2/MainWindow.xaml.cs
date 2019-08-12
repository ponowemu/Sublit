using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
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
        private Database db;
        private List<Document> documentsList = null;
        public MainWindow()
        {
            InitializeComponent();
            db = new Database();
            Subiekt subiekt = new Subiekt();
            subiekt.Connect();
            su = subiekt;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
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

        [STAThread]
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
                            var res = su.GenerateInvoice(el.Number, path);   
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("An error occured!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void SendEmail(string attachment, int shop, string receiver)
        {
            string sender = "";
            string topic = "";
            string prefix = "";
            string content = "";
            if(shop == 1)
            {
                sender = "info@sprzegla24.pl";
                topic = "[Zamówienie Sprzegla24.pl] Faktura VAT ";
                prefix = "s24";
                content = "<img src='https://www.sprzegla24.pl/img/theme-logo-1461570025.jpg' alt='Logo' /><h3>Szanowni Państwo </h3><br /><br /><p> Dziękujemy za złożenie zamówienia.W załączniku przesyłamy fakturę VAT.</p>";
            }
            else
            {
                sender = "info@sprzeglo.com.pl";
                topic = "[Zamówienie Sprzeglo.com.pl] Faktura VAT ";
                prefix = "spc";
                content = "<img src='https://www.sprzeglo.com.pl/img/sprzeglocompl-logo-1461228708.jpg' alt='Logo' /><h3>Szanowni Państwo </h3><br /><br /><p> Dziękujemy za złożenie zamówienia.W załączniku przesyłamy fakturę VAT.</p>";
            }
            var smtpServerName = ConfigurationManager.AppSettings[prefix + "_SmtpServer"];
            var port = ConfigurationManager.AppSettings[prefix + "_Port"];
            var senderEmailId = ConfigurationManager.AppSettings[prefix+"_SenderEmailId"];
            var senderPassword = ConfigurationManager.AppSettings[prefix + "_SenderPassword"];

            MailMessage message = new MailMessage(
               sender,
               receiver,
               topic,
               content);
            message.IsBodyHtml = true;

            string file = attachment;
            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            message.Attachments.Add(data);

            
            var smptClient = new SmtpClient(smtpServerName, Convert.ToInt32(port))
            {
                Credentials = new NetworkCredential(senderEmailId, senderPassword),
                EnableSsl = true
            };
            smptClient.Send(message);
            MessageBox.Show("Message Sent Successfully", "Sukces", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (documentsList == null || documentsList.Count == 0)
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
                        foreach (var el in list)
                        {
                            var res = su.GenerateInvoice(el.Number, path);
                            string email = "";

                            try
                            {
                                email = db.GetCustomer(el.CustomerId).Email;
                            }
                            catch
                            {
                                email = "";
                            }
                            
                            var correct = Interaction.InputBox("Wprowadź lub skoryguj adres e-mail odbiorcy!", "Odbiorca", email);

                            if (el.OrderNumber.Contains("SP24"))
                                this.SendEmail(res, 1, correct);
                            else
                                this.SendEmail(res, 2, correct);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
           
           
        }
    }
}
