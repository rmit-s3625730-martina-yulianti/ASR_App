using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{   /* Custome Console class to get input from user 
      @ param: string
      @ return: string and int from user input
    */
    class Console
    {
        // return string response from user
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
            catch (Exception)
            {
                throw new FormatException("Input was not a number");
            }
        }
    }
}
