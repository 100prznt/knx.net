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

            const string ip = "192.168.1.1";
            const int port = 555;

            Console.WriteLine($"Connect to {ip}:{port}");
            var connection = new KnxConnectionRouting(ip, port);
            connection.Connect();
            connection.KnxEventDelegate += Event;
            connection.Action("5/0/2", false);
            Thread.Sleep(5000);
            connection.Action("5/0/2", true);
            //Thread.Sleep(5000);
            Console.ReadKey();
        }
        static void Event(string address, string state)
        {
            Console.WriteLine("New Event: device " + address + " has status " + state);
        }
    }
}
