using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QuanLiTrongTrot.Model
{
    public class DataProvider
    {

        // Thay connection string theo máy bạn
        public static string connectionString = @"Data Source=LAPTOP-FB9U3RUR\SQLEXPRESS;Initial Catalog=QuanLiTrongTrotdb;Integrated Security=True";

        #region Core Database Methods

        public static DataTable ExecuteQuery(string query, object[] parameters = null)
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

        public static int ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Parameters.AddRange(parameters);
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Generic CRUD Methods

        public static string data_list(string[] list)
        {
            return string.Join(", ", list);
        }

        /// <summary>
        /// Lấy ID nhỏ nhất còn trống hoặc ID tiếp theo cho bất kỳ bảng nào
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="idColumnName">Tên cột ID (mặc định là "Id")</param>
        /// <returns>ID tiếp theo</returns>
        public static int GetNextId(string tableName, string idColumnName = "Id")
        {
            // Kiểm tra ID = 1 có tồn tại không
            string checkId1Query = $"SELECT COUNT(*) FROM {tableName} WHERE {idColumnName} = 1";
            int hasId1 = ExecuteScalar(checkId1Query);
            if (hasId1 == 0)
                return 1;

            // Tìm ID nhỏ nhất còn trống (gap)
            string gapQuery = $@"
                SELECT TOP 1 t1.{idColumnName} + 1 AS NextId
                FROM {tableName} t1
                LEFT JOIN {tableName} t2 ON t1.{idColumnName} + 1 = t2.{idColumnName}
                WHERE t2.{idColumnName} IS NULL
                ORDER BY t1.{idColumnName}";

            int gapId = ExecuteScalar(gapQuery);
            if (gapId > 0)
                return gapId;

            // Nếu không có gap, lấy MAX + 1
            string maxQuery = $"SELECT ISNULL(MAX({idColumnName}), 0) + 1 FROM {tableName}";
            return ExecuteScalar(maxQuery);
        }

        /// <summary>
        /// Insert với ID tự động lấp gap
        /// </summary>
        public static int INSERT_DATA_WITH_ID(string[] values, string[] data_names, string table)
        {
            if (values.Length != data_names.Length)
                throw new ArgumentException("Số lượng giá trị và tên cột phải bằng nhau!");

            int newId = GetNextId(table);

            // Thêm Id vào đầu
            string[] newValues = new string[values.Length + 1];
            string[] newDataNames = new string[data_names.Length + 1];
            
            newValues[0] = newId.ToString();
            newDataNames[0] = "Id";
            
            for (int i = 0; i < values.Length; i++)
            {
                newValues[i + 1] = values[i];
                newDataNames[i + 1] = data_names[i];
            }

            string columns = data_list(newDataNames);
            string parameters = string.Join(", ", newDataNames.Select(n => "@" + n));
            
            string query = $@"
                SET IDENTITY_INSERT {table} ON;
                INSERT INTO {table} ({columns}) VALUES ({parameters});
                SET IDENTITY_INSERT {table} OFF;";

            var sqlParams = newDataNames.Select((n, i) => new SqlParameter($"@{n}", newValues[i])).ToArray();
            return ExecuteNonQuery(query, sqlParams);
        }

        public static int INSERT_DATA(string[] values, string[] data_names, string table)
        {
            if (values.Length != data_names.Length)
                throw new ArgumentException("Số lượng giá trị và tên cột phải bằng nhau!");

            string columns = data_list(data_names);
            string parameters = string.Join(", ", data_names.Select(n => "@" + n));
            string query = $"INSERT INTO {table} ({columns}) VALUES ({parameters})";
            var sqlParams = data_names.Select((n, i) => new SqlParameter($"@{n}", values[i])).ToArray();
            return ExecuteNonQuery(query, sqlParams);
        }

        public static int DELETE_DATA(string name, string data_name, string table)
        {
            return ExecuteNonQuery($"DELETE FROM {table} WHERE {data_name} = @{data_name}",
                new SqlParameter($"@{data_name}", name));
        }

        public static bool CHECK_SAME_ROW_DATA(string[] values, string[] data_names, string table)
        {
            string conditions = string.Join(" AND ", data_names.Select(n => $"{n} = @{n}"));
            string query = $"SELECT COUNT(*) FROM {table} WHERE {conditions}";
            var sqlParams = data_names.Select((n, i) => new SqlParameter($"@{n}", values[i])).ToArray();
            return ExecuteScalar(query, sqlParams) > 0;
        }

        public static bool CHECK_DATA_EXISTS(string name, string data_name, string table)
        {
            return ExecuteScalar($"SELECT COUNT(*) FROM {table} WHERE {data_name} = @{data_name}",
                new SqlParameter($"@{data_name}", name)) > 0;
        }

        public static int CHANGE_DATA(string[] values, string[] data_names, string table)
        {
            DELETE_DATA(values[0], data_names[0], table);
            return INSERT_DATA(values, data_names, table);
        }

        #endregion
    }
}