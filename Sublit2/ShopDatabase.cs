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
        private static MySqlConnection sql_conn_s24;
        private static MySqlConnection sql_conn_spc;
        private static string s24_Host = "mysql-sprzegla.nano.pl";
        private static string s24_Login = "db100008759_apli";
        private static string s24_Pass = "7hV5vR5e";
        private static string s24_Db = "db100008759";

        private static string spc_Host = "mysql-sprzegla.nano.pl";
        private static string spc_Login = "db100008757_apli";
        private static string spc_Pass = "8ZAYsiU1";
        private static string spc_Db = "db100008757";


        public ShopDatabase()
        {
            try
            {
                MySqlConnection thisConnection = new MySqlConnection(@"Server=" + s24_Host + ";Port=3306;Database=" + s24_Db + ";Uid=" + s24_Login + ";Pwd=" + s24_Pass + ";");
                thisConnection.Open();
                sql_conn_s24 = thisConnection;

                MySqlConnection spc = new MySqlConnection(@"Server=" + spc_Host + ";Port=3306;Database=" + spc_Db + ";Uid=" + spc_Login + ";Pwd=" + spc_Pass + ";");
                spc.Open();
                sql_conn_spc = spc;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DUPA");
                Console.WriteLine(ex.Message);
            }
        }

        public List<Order> GetOrders(int shop)
        {
            var orders_list = new List<Order>();
            var date = DateTime.Now;
            date = date.AddMonths(-1);
            MySqlConnection sql;
            string query = "SELECT * FROM ps_orders AS A INNER JOIN ps_customer AS B ON A.id_customer = B.id_customer WHERE A.date_add >='" + date.ToString("yyyy-MM-dd") + "' ORDER BY A.date_add DESC";

            if (shop == 1)
                sql = sql_conn_s24;
            else
                sql = sql_conn_spc;

            using (MySqlCommand command = new MySqlCommand(query, sql))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders_list.Add(new Order() {
                            Created = reader.GetDateTime(reader.GetOrdinal("date_add")),
                            CustomerData = reader.GetString(reader.GetOrdinal("firstname")) + " " + reader.GetString(reader.GetOrdinal("lastname")),
                            Id = reader.GetInt32(reader.GetOrdinal("id_order")),
                            Reference = reader.GetString(reader.GetOrdinal("reference")),
                            TotalPaid = reader.GetDecimal(reader.GetOrdinal("total_paid")),
                            Updated = reader.GetDateTime(reader.GetOrdinal("date_upd")),
                            Email = reader.GetString(reader.GetOrdinal("email"))
                        });
                    }
                }
            }
            return orders_list;
        }
    }
}
