using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    internal class ReadNumber
    {
        static void Main1(string[] args)
        {
            int parsedNumber = 0;
            parsedNumber = ReadInt();
        }

        static int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                string enteredString;

                Console.WriteLine("Введите целое число:");
                enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                if (isIntEntered)
                {
                    Console.WriteLine($"Введенное число распознанно, это: {parsedInt}");
                }
                else
                {
                    Console.WriteLine("Вы ввели неверно, попробуйте ещё раз)\n");
                }
            }

            return parsedInt;
        }
    }
}
