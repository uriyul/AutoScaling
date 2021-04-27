using System;

namespace AutoScalingClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var randomRequestsSender = new RandomRequestsSender();
            randomRequestsSender.SendRequestsBulk(10);

            Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }
    }
}
