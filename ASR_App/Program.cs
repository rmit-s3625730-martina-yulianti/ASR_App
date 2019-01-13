using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;


namespace ASR_App
{
    public static class Program
    {
        // Build configuration setting for database connection
        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().SetBasePath("D:\\PG_Material\\Year_2\\Semester1_2019\\WebDevelopment\\Assignment\\WebDev_Assignment1\\ASR_App\\ASR_App").AddJsonFile("appsettings.json",optional: false).Build();

        public static string ConnectionString { get; } = Configuration["ConnectionString"];

        static void Main()
        {
            try
            {
                ASR_App driver = new ASR_App();
                driver.Start();
            }catch(Exception err)
            {
                Console.WriteLine(err.ToString());
            }
            
            
        }

       
    }
}
