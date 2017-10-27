using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {

            DataMessageConsumer dmc = new DataMessageConsumer();
            dmc.RunConsumer();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            dmc.HaltConsumer();
        }
    }
}
