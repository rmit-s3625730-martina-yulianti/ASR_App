using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Utilities
{
    /* ASRDatabase class is a clas to create database connection to sql server
     * Reference : Database Connection is using the coding example from Tutorial week 3
     */

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

    } // End of ASRDatabase class
}
