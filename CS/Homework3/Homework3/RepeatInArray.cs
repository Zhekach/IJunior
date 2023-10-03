using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class RepeatInArray
    {
        static void Main1(string[] args)
        {
            int arrayLength = 30;
            int[] baseArray = new int[arrayLength];
            int maxRandomValue = 3;

            int currentRepeatCounter = 0;
            int maxRepeatCounter = 0;
            int repeatNumber = 0;
            int previousNumber = 0;

            Random random = new Random();

            for (int i = 0; i < arrayLength; i++)
            {
                baseArray[i] = random.Next(maxRandomValue);
                Console.Write(baseArray[i] + " ");
            }

            previousNumber = baseArray[0];

            for (int i = 1; i < baseArray.Length; i++)
            {
                if (baseArray[i] == previousNumber)
                {
                    currentRepeatCounter++;

                    if (currentRepeatCounter > maxRepeatCounter)
                    {
                        maxRepeatCounter = currentRepeatCounter;
                        repeatNumber = previousNumber;
                    }
                }
                else
                {
                    currentRepeatCounter = 0;
                }

                previousNumber = baseArray[i];
            }

            if (maxRepeatCounter > 0)
            {
                Console.WriteLine($"\nNumber {repeatNumber} repeats {maxRepeatCounter + 1} times");
            }
            else
            {
                Console.WriteLine("\nThere are no repeating numbers");
            }
        }
    }
}
