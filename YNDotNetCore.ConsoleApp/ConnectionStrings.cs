using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNDotNetCore.ConsoleApp
{
    internal static class ConnectionStrings
    {
        public static SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource=".",
            InitialCatalog= "DotNetTrainingBatch4",
            UserID = "sa",
            Password = "@naing?007bl@ck#",
            TrustServerCertificate = true,

        };
    } 
}
