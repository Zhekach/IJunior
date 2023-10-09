using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class ShiftArray
    {
        static void Main1(string[] args)
        {
            int[] testArray = { 1, 2, 3, 4, 5};
            int shiftsCount = 0;

            Console.WriteLine("Base array:");

            foreach (var integer in testArray)
            {
                Console.Write(integer + " ");
            }

            Console.Write("\nEnter count of shifts: ");

            shiftsCount = Convert.ToInt32(Console.ReadLine());
            shiftsCount = shiftsCount % testArray.Length;

            Console.WriteLine("\nShifted array:");

            for(int i = 0; i < shiftsCount; i++)
            {
                int tempValue = testArray[0];

                for (int j = 0; j < testArray.Length - 1; j++)
                {
                    testArray[j] = testArray[j + 1];
                }

                testArray[testArray.Length - 1] = tempValue;
            }

            foreach (var integer in testArray)
            {
                Console.Write(integer + " ");
            }
        }
    }
}
