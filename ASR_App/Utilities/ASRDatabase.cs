using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ASR_App
{
    public static class ASRDatabase
    {
        // Build configuration setting for database connection
        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

        public static string ConnectionString { get; } = Configuration["ConnectionString"];

        // Create database connection
        public static SqlConnection CreateConnection(this string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        // Get data from database table
        public static DataTable GetDataTable(this SqlCommand command)
        {
            var table = new DataTable();
            new SqlDataAdapter(command).Fill(table);

            return table;
        }



    }
}
