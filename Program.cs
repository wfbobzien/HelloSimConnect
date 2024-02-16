using Microsoft.FlightSimulator.SimConnect;
using System.Numerics;
using System.Runtime.InteropServices;

namespace HelloSimConnect
{
    internal class Program
    {
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

            simConnect?.Dispose();
        }
    }
}
