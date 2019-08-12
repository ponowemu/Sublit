using MySql.Data.MySqlClient;
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
    class ShopDatabase
    {
        public static MySqlConnection sql_conn;
        private static string Host = "25.22.236.167\\INSERTGT";
        private static string Login = "sa";
        private static string Pass = "";
        private static string Db = "A_M_Consultants";


        public ShopDatabase()
        {
            try
            {
                MySqlConnection thisConnection = new MySqlConnection(@"Server=" + Host + ";Database=" + Db + ";User Id=" + Login + ";Password=" + Pass + ";");
                thisConnection.Open();

                sql_conn = thisConnection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<Document> GetDocuments()
        {
            try
            {
                List<Document> docList = new List<Document>();

                string query = "SELECT * FROM dok__Dokument WHERE dok_Typ = 2 ORDER BY dok_Id DESC";

                using (MySqlCommand command = new MySqlCommand(query, sql_conn))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
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
                                DocType = reader.GetInt32(reader.GetOrdinal("dok_Typ"))
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
