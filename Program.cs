using Microsoft.FlightSimulator.SimConnect;

namespace HelloSimConnect
{
    internal class Program
    {
        static void Main()
        {
            SimConnect simConnect = new SimConnect("Hello, SimConnect!", IntPtr.Zero, 0, null, 0);

            simConnect.Dispose();
        }
    }
}
