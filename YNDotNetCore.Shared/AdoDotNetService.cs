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

        public List<T> Query<T>(string query,params AdoDotNetParameter[] parameters) 
        { //AdoDotNetParameter[] parameters=null (do not use param) param use when the request the input from user 
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
           
            SqlCommand cmd = new SqlCommand(query, connection);
            if (parameters.Length>0 && parameters is not null)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Name, item.Value);
                }
                /* cmd.Parameters.AddRange(parameters.Select(item=>new SqlParameter(item.Name,item.Value)).ToArray());*/
            }
            
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            string json=JsonConvert.SerializeObject(dt); // C# to Json
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json); //Json to C#
            return list;
        }

        public T QueryFirstOrDefault<T>(string query, params AdoDotNetParameter[] parameters)
        { //AdoDotNetParameter[] parameters=null (do not use param) param use when the request the input from user 
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            if (parameters.Length > 0 && parameters is not null)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Name, item.Value);
                }
                /* cmd.Parameters.AddRange(parameters.Select(item=>new SqlParameter(item.Name,item.Value)).ToArray());*/
            }

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            string json = JsonConvert.SerializeObject(dt); // C# to Json
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json); //Json to C#
            return list[0];
        }

        public int Execute(string query, params AdoDotNetParameter[] parameters)
        { //AdoDotNetParameter[] parameters=null (do not use param) param use when the request the input from user 
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            if (parameters.Length > 0 && parameters is not null)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue(item.Name, item.Value);
                }
                /* cmd.Parameters.AddRange(parameters.Select(item=>new SqlParameter(item.Name,item.Value)).ToArray());*/
            }

            var result=cmd.ExecuteNonQuery();
            connection.Close();

            
            return result;
 
        }

    }

    public class AdoDotNetParameter
    { public AdoDotNetParameter() { }
        public AdoDotNetParameter(string name,object value) {
            Name = name;
            Value = value;
        }
        public string? Name { get; set; }

        public Object? Value { get; set; }

    }
}
