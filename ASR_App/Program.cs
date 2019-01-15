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
                ASR_App ASR_Application = new ASR_App();
                ASR_Application.Start();

            }catch(Exception err)
            {
                Console.WriteLine(err.ToString());
            }                
        }
    }
}
