using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;

namespace HelloSimConnect
{
    internal class Program
    {
        static bool isSimConnectInitialized = false;
        static bool isSimConnectOpen = false;

        enum EventId
        {
            FourSeconds = 1,
            OneSecond,
            SixHertz
        }
        
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

                simConnect.SubscribeToSystemEvent(EventId.FourSeconds, "4sec");

                simConnect.SubscribeToSystemEvent(EventId.OneSecond, "1sec");

                simConnect.SubscribeToSystemEvent(EventId.SixHertz, "6Hz");

                simConnect.OnRecvEvent += OnSimConnectEvent;

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

        private static void OnSimConnectEvent(SimConnect sender, SIMCONNECT_RECV_EVENT data)
        {
            string message = "";

            switch (data.uEventID)
            {
                case (uint)EventId.FourSeconds:
                    {
                        message = "System event: '4sec'";
                        sender.SetSystemEventState(EventId.SixHertz, SIMCONNECT_STATE.ON);
                        break;
                    }
                case (uint)EventId.OneSecond:
                    {
                        message = "System event: '1sec'";
                        sender.SetSystemEventState(EventId.SixHertz, SIMCONNECT_STATE.OFF);
                        break;
                    }
                case (uint)EventId.SixHertz:
                    {
                        message = "System event: '6Hz'";
                        break;
                    }
            }

            Console.WriteLine(message);
        }
    }
}
