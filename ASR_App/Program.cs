using System;

namespace ASR_App
{
    public class Program
    {
       
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
