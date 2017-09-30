using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.DatabaseService;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            SpawnThreads();
            Console.ReadKey();
        }

        private static void SpawnThreads()
        {
            int count = 30;

            for (int i = 0; i < count; i++)
            {
                int id = i;
                var task = Task.Factory.StartNew(() => SendRequest(id));
            }
        }

        private static void SendRequest(int id)
        {
            Console.WriteLine(id + ": sending.");
            var client = new DatabaseClient();

            var juiceInfo = new JuiceInfo();
            juiceInfo.Name = "Amazing lemon";
            juiceInfo.Id = id;

            var similarArray = client.FindSimilar(juiceInfo);
            Console.WriteLine(id + ": finished. Count: " + similarArray.Length);
        }


    }
}
