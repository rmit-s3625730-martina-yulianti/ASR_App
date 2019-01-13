using System;
using System.Data;
using System.Data.SqlClient;

namespace Util
{   /* Custome Console class to get input from user.
       Create database connection to sql server
       Reference : Tutorial week 3
    */
    public static class  Console
    {
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

                
        // return char response from user
        public static string AskChar(string question)
        {
                System.Console.Write(question);
                char c = Char.Parse(System.Console.ReadLine());
                if ( (c >='A' && c <= 'Z')|| c >= 'a' && c <= 'z')
                {
                    return c.ToString();
                }else
                    throw new FormatException("Input was not a letter.");


        }

        // return char response from user
        public static string Ask(string question)
        {

            System.Console.Write(question);
            return System.Console.ReadLine();

        }

        //return int response from user and give error message if the user input was not a number
        public static int AskInt(string question)
        {
            try
            {
                System.Console.Write(question);
                return int.Parse(System.Console.ReadLine());
            }
            catch (FormatException)
            {
                throw new FormatException("Input was not a number");
            }
        }
    }
}
