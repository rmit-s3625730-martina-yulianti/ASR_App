using System;

namespace ASR_App
{
    /* WDT Assignment 1 : Appointment Scheduling and Reservation (ASR) system
     * @Author : Martina Yulianti (s3625730)
     * @Date : 16 January 2019
     */ 

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
