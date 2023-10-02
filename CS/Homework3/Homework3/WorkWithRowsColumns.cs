using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class WorkWithRowsColumns
    {
        static void Main1(string[] args)
        {
            const int NumberOfRow = 2;
            const int NumberOfColumn = 1;

            int[,] simpleArray = new int[4, 5];
            int maxValueInArray = 10;
            int sumOfRow = 0;
            int mulOfCollumn = 1;
            Random random = new Random();

            for (int i = 0; i < simpleArray.GetLength(0); i++)
            {
                for (int j = 0; j < simpleArray.GetLength(1); j++)
                {
                    simpleArray[i, j] = random.Next(maxValueInArray + 1);
                    Console.Write(simpleArray[i, j] + " ");
                }

                Console.WriteLine("");
            }

            for (int i = 0; i < simpleArray.GetLength(1); i++)
            {
                sumOfRow += simpleArray[NumberOfRow - 1, i];
            }

            Console.WriteLine($"Sum of elements in row {NumberOfRow}: {sumOfRow}.");

            for(int i = 0;i < simpleArray.GetLength(0); i++)
            {
                mulOfCollumn *= simpleArray[i, NumberOfColumn - 1];
            }

            Console.WriteLine($"Multiplication of elements in column {NumberOfColumn}: {mulOfCollumn}");
        }
    }
}
