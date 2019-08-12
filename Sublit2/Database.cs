using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sublit2
{
    class Database
    {
        public static SqlConnection sql_conn;
        private static string Host = "25.22.236.167\\INSERTGT";
        private static string Login = "sa";
        private static string Pass = "";
        private static string Db = "A_M_Consultants";


        public Database()
        {
            try
            {
                SqlConnection thisConnection = new SqlConnection(@"Server=" + Host + ";Database=" + Db + ";User Id=" + Login + ";Password=" + Pass + ";");
                thisConnection.Open();

                sql_conn = thisConnection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public Customer GetCustomer(int id)
        {
            var customer = new Customer();
            try
            {
                string query = "SELECT * FROM kh__Kontrahent WHERE kh_Id = " + id + "";
                using (SqlCommand command = new SqlCommand(query, sql_conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customer.Email = reader.GetString(reader.GetOrdinal("kh_EMail"));
                            customer.Id = reader.GetInt32(reader.GetOrdinal("kh_Id"));
                            customer.Name = reader.GetString(reader.GetOrdinal("kh_Kontakt"));
                        }
                    }
                }
                return customer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public string GetDocumentNumber(string reference)
        {
            try
            {
                string docNumber = "";

                string query = "SELECT * FROM dok__Dokument WHERE dok_Typ = 2 AND dok_DoDokNrPelny = '"+reference+"' ORDER BY dok_Id DESC";

                using (SqlCommand command = new SqlCommand(query, sql_conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            docNumber = reader.GetString(reader.GetOrdinal("dok_NrPelny"));
                        }
                    }
                }
                return docNumber;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<Document> GetDocuments()
        {
            try
            {
                List<Document> docList = new List<Document>();

                string query = "SELECT * FROM dok__Dokument WHERE dok_Typ = 2 ORDER BY dok_Id DESC";

                using (SqlCommand command = new SqlCommand(query, sql_conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int item = reader.GetInt32(reader.GetOrdinal("dok_Id"));
                            docList.Add(new Document()
                            {
                                Checked = false,
                                Id = reader.GetInt32(reader.GetOrdinal("dok_Id")),
                                CreationDate = reader.GetDateTime(reader.GetOrdinal("dok_DataWyst")),
                                Creator = reader.GetString(reader.GetOrdinal("dok_Wystawil")),
                                Number = reader.GetString(reader.GetOrdinal("dok_NrPelny")),
                                Name = reader.GetInt32(reader.GetOrdinal("dok_Nr")).ToString(),
                                Description = reader.GetString(reader.GetOrdinal("dok_Uwagi")),
                                NetPrice = reader.GetDecimal(reader.GetOrdinal("dok_WartNetto")),
                                GrossPrice = reader.GetDecimal(reader.GetOrdinal("dok_WartBrutto")),
                                DocType = reader.GetInt32(reader.GetOrdinal("dok_Typ")),
                                OrderNumber = reader.GetString(reader.GetOrdinal("dok_DoDokNrPelny")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("dok_OdbiorcaId"))
                            });
                        }
                    }
                }
                return docList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
