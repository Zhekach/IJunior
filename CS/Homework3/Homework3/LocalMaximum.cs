using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class LocalMaximum
    {
        static void Main1(string[] args)
        {
            int arrayLength = 30;
            int maxRandomValue = 50;
            int[] testArray = new int[arrayLength];

            Random random = new Random();

            for (int i = 0; i < arrayLength; i++)
            {
                testArray[i] = random.Next(maxRandomValue);
                Console.Write(testArray[i] + " ");
            }

            Console.WriteLine("\nLocal maximums\n");

            if (testArray[0] > testArray[1])
            {
                Console.Write(testArray[0] + " ");
            }
            else
            {
                Console.Write("-" + " ");
            }

            for (int i = 1; i < arrayLength - 1; i++)
            {
                if (testArray[i] > testArray[i - 1] && testArray[i] > testArray[i + 1])
                {
                    Console.Write(testArray[i] + " ");
                }
                else
                {
                    Console.Write("-" + " ");
                }
            }

            if (testArray[testArray.Length-1] > testArray[testArray.Length - 2])
            {
                Console.Write(testArray[testArray.Length-1] + " ");
            }
            else
            {
                Console.Write("-" + " ");
            }
        }
    }
}
