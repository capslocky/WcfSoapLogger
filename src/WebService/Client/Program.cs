using System;
using System.Collections.Generic;
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
            var client = new DatabaseClient();

            var juiceInfo = new JuiceInfo();
            juiceInfo.Id = 2;

            var similarArray = client.FindSimilar(juiceInfo);

            foreach (var juice in similarArray)
            {
                Console.WriteLine(juice.Name);
            }

            Console.ReadKey();
        }
    }
}
