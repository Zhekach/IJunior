using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class ArraySort
    {
        static void Main1(string[] args)
        {
            int tempValue = 0;
            bool isUnsorted = true;
            int arrayLength = 10;
            int[] array = new int[arrayLength];
            int maxRandomValue = 15;

            Random random = new Random();

            for (int i = 0; i < arrayLength; i++)
            {
                array[i] = random.Next(maxRandomValue);
                Console.Write(array[i] + " ");
            }

            while (isUnsorted)
            {
                int sortsCounter = 0;

                for (int i = 0; i < arrayLength - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        sortsCounter++;
                        tempValue = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = tempValue;
                    }
                }

                if (sortsCounter == 0)
                {
                    isUnsorted = false;
                }
            }

            Console.WriteLine($"\n Sorted array:");

            for (int i = 0; i < arrayLength; i++)
            {
                Console.Write(array[i] + " ");
            }
        }
    }
}
