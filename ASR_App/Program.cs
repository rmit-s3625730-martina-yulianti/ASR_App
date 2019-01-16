using Controller;
using System;

namespace ASR_App
{
    /* WDT Assignment 1 : Appointment Scheduling and Reservation (ASR) system
     * @Author : Martina Yulianti (s3625730)
     * @Date : 16 January 2019
     */ 

    public class Program
    {
        private static void Main()
        {
            try
            {
                ASRController ASR_Application = new ASRController();
                ASR_Application.Start();
               
            }catch(Exception err)
            {
                Console.WriteLine(err.Message);
            }                
        }
    }
}
