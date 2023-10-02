using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    internal class MultiplesNumbers
    {
        static void Main1(string[] args)
        {
            int minMultiplesRange = 100;
            int maxMultiplesRange = 999;
            int minNRange = 1;
            int maxNRange = 27;
            int multiplesNCounter = 0;

            Random random = new Random();
            int numberN = random.Next(minNRange, maxNRange + 1);

            Console.WriteLine($"Number N in range [{minNRange};{maxNRange}]: {numberN}");

            for (int i = 0; i <= maxMultiplesRange; i += numberN)
            {
                if (i >= minMultiplesRange)
                {
                    multiplesNCounter++;
                }
            }

            Console.WriteLine($"Count multiples in range [{minMultiplesRange};{maxMultiplesRange}]: {multiplesNCounter}");
        }
    }
}
