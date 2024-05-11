using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YNDotNetCore.Shared
{
    public class AdoDotNetService
    {
        private readonly string _connectionString;

        public AdoDotNetService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<T> Query<T>(string query) {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            Console.WriteLine("Connection Open");


            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            string json=JsonConvert.SerializeObject(dt); // C# to Json
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json); //Json to C#
            return list;
        }
    }
}
