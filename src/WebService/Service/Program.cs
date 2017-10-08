using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args) 
        {
            Console.WriteLine("Press any key to start service.");
            Console.ReadKey();

            var serviceHost = new ServiceHost(typeof(Database));
            serviceHost.Open();
            Console.WriteLine("Service started.");

            Console.WriteLine("Press Enter to stop.");
            Console.ReadLine();

            Console.WriteLine("Service is stopping...");
            serviceHost.Close();

            Console.WriteLine("Service stopped.");
            Console.WriteLine();
        }
    }
}
