using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider() { }


        private string connectionSTR = @"Data Source=localhost,1433;Initial Catalog=QLQuanTS;User ID=sa;Password=ninh2405";

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectionSTR))
                {


                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    if (parameter != null)
                    {
                        string[] listPara = query.Split(' ');
                        int i = 0;
                        foreach (string items in listPara)
                        {
                            if (items.Contains('@'))
                            {
                                command.Parameters.AddWithValue(items, parameter[i]);
                                i++;
                            }
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(data);

                    connection.Close();
                }

                return data;
            } catch(Exception e)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Thông báo");
                return data;
            }
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string items in listPara)
                    {
                        if (items.Contains('@'))
                        {
                            command.Parameters.AddWithValue(items, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();
                connection.Close();
            }

            return data;
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string items in listPara)
                    {
                        if (items.Contains('@'))
                        {
                            command.Parameters.AddWithValue(items, parameter[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }
    }
}
