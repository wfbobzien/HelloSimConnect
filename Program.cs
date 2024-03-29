﻿using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;

namespace HelloSimConnect
{
    internal class Program
    {
        static bool isSimConnectInitialized = false;
        static bool isSimConnectOpen = false;

        static void Main()
        {
            SimConnect? simConnect = null;

            try
            {
                simConnect = new SimConnect("Hello, SimConnect!", IntPtr.Zero, 0, null, 0);
            }
            catch (COMException comEx)
            {
                Console.WriteLine($"A COM exception occurred. Make sure Microsoft Flight Simulator is running. Exception: {comEx.Message}");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"An unexpected exception occurred. Exception: {ex.Message}");
            }

            if (simConnect != null)
            {
                simConnect.OnRecvOpen += OnSimConnectOpen;

                simConnect.OnRecvQuit += OnSimConnectQuit;
                
                while (!isSimConnectInitialized || isSimConnectOpen) simConnect.ReceiveMessage();

                Console.WriteLine("Press any key to exit the program.");

                Console.ReadKey(true);

                simConnect?.Dispose();
            }
        }

        private static void OnSimConnectOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.WriteLine($"SimConnect connection to application '{data.szApplicationName}' opened.");

            isSimConnectInitialized = true;
            
            isSimConnectOpen = true;
        }

        private static void OnSimConnectQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.WriteLine($"User quit the connected application.");

            isSimConnectOpen = false;
        }
    }
}
