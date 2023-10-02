using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class MaxElement
    {
        static void Main1(string[] args)
        {
            const int NumberOfRows = 10;
            const int NumberOfColumns = 10;

            int[,] arrayA = new int[NumberOfRows, NumberOfColumns];
            int maxRandomValue = 50;
            int maxValueInArray = int.MinValue;
            int maxValueInArrayCount = 0;

            Random random = new Random();

            for (int i = 0; i < arrayA.GetLength(0); i++)
            {
                for (int j = 0; j < arrayA.GetLength(1); j++)
                {
                    arrayA[i, j] = random.Next(maxRandomValue + 1);
                    Console.Write(arrayA[i,j] + " ");
                }

                Console.WriteLine("");
            }

            Console.WriteLine("\n\n");

            for (int i = 0;i < arrayA.GetLength(0); i++)
            {
                for(int j = 0;j < arrayA.GetLength(1); j++)
                {
                    if (arrayA[i, j] > maxValueInArray)
                    {
                        maxValueInArray = arrayA[i, j];
                    }
                }
            }

            Console.WriteLine($"Max value in array = {maxValueInArray} \n\n");

            for (int i = 0;i < arrayA.GetLength(0); i++)
            {
                for(int j = 0;j < arrayA.GetLength(1); j++)
                {
                    if (arrayA[i, j] == maxValueInArray)
                    {
                        arrayA[i, j] = 0;
                        maxValueInArrayCount++;
                    }
                    Console.Write(arrayA[i,j] + " ");
                }
            Console.WriteLine("");
            }

            Console.WriteLine($"\nThere were {maxValueInArrayCount} such values.");
        }
    }
}
