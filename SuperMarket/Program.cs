using System;
using System.Collections.Generic;
namespace Supermarket
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string input = Console.ReadLine();
            int customerCount = int.Parse(Console.ReadLine());
            int maxQueueLength = int.Parse(Console.ReadLine());
            int cashierCount = int.Parse(Console.ReadLine());
            string input = Console.ReadLine();*/
            int customerCount = 100;
            int maxQueueLength = 5;
            int cashierCount = 3;
            string input = "Alena 11 Bolek 17 Cecil 19";
            string[] parameteres = input.Split();
            List <Cashier> cashiers = new List<Cashier>();
            int breakTime = 5;
            int counter = 1;
            for (int i = 0; i < 2 * cashierCount; i+=2)
            {
                cashiers.Add(new Cashier(parameteres[i], int.Parse(parameteres[i+1]), maxQueueLength, breakTime*counter));
                counter++;
            }
            Supermarket supermarket = new Supermarket(cashiers, customerCount);
            supermarket.Run();
        }
    }
}