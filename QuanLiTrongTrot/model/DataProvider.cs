using System.Data;
using System.Data.SqlClient;

namespace QuanLiTrongTrot.Model
{
    public class DataProvider
    {
        private static DataProvider _instance;

        public static DataProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataProvider();
                return _instance;
            }
        }

        // Thay connection string theo máy bạn
        private string connectionString = @"Data Source=LAPTOP-FB9U3RUR\SQLEXPRESS;Initial Catalog=KTPM;Integrated Security=True";

        public DataTable ExecuteQuery(string query, object[] parameters = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    string[] listParam = query.Split(' ');
                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains("@"))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
            }

            return data;
        }
    }
}