using System;
using System.Threading.Tasks;

namespace AutoScalingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var randomRequestsSender = new RandomRequestsSender();
            randomRequestsSender.SendRequestsBulk(10);
            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }

    }
}
