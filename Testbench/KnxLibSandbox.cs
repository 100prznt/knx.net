using KNXLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testbench
{
    class KnxLibSandbox
    {
        static void Main(string[] args)
        {
            #region Startup
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            var attribute = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                    .Cast<AssemblyDescriptionAttribute>().FirstOrDefault();
            var appName = string.Format("{0} v{1}", typeof(KnxLibSandbox).Assembly.GetName().Name, versionInfo.ProductVersion);

            Console.Title = appName;
            Console.WindowWidth = 81;
            Console.BufferWidth = 81;
            Console.WindowHeight = 36;


            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(appName);
            Console.ResetColor();
            if (attribute != null)
                Console.WriteLine(attribute.Description);
            Console.WriteLine();
            Console.WriteLine(versionInfo.LegalCopyright);
            Console.WriteLine(String.Empty.PadLeft(80, '-'));
            Console.WriteLine();
            #endregion

            const string IP = "192.168.1.1"; //IP of KNX router
            const int PORT = 555; //Port of KNX router
            const string TEST_ADDR = "5/0/2";

            Console.WriteLine($"Connect to {IP}:{PORT}");
            var connection = new KnxConnectionRouting(IP, PORT);
            connection.Connect();
            connection.KnxEventDelegate += KnxEvent;
            connection.Action(TEST_ADDR, false);
            Console.WriteLine("Reset " + TEST_ADDR);
            Thread.Sleep(5000);
            connection.Action(TEST_ADDR, true);
            Console.WriteLine("Set " + TEST_ADDR);
            //Thread.Sleep(5000);
            Console.WriteLine("Wait for events on bus. Press any key to abort and quit the programm.");
            Console.ReadKey();
        }

        static void KnxEvent(string address, string state)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("New Event: device " + address + " has status " + state);
            Console.ResetColor();
        }
    }
}
