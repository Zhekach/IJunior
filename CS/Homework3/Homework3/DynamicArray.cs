using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class DynamicArray
    {
        static void Main1(string[] args)
        {
            const string SumCommand = "sum";
            const string ExitCommand = "exit";

            int[] dynamicArray = new int[0];
            bool isProgramOpen = true;

            Console.WriteLine("Enter integer value to add it to the array.\n" +
                $"Enter \"{SumCommand}\" to show sum of current elements.\n" +
                $"Enter \"{ExitCommand}\" to close program.");

            while (isProgramOpen)
            {
                Console.Write("Enter new element, or command:");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case ExitCommand:
                        isProgramOpen = false;
                        break;

                    case SumCommand:
                        int sumOfArray = 0;

                        for (int i = 0; i < dynamicArray.Length; i++)
                        {
                            sumOfArray += dynamicArray[i];
                        }

                        Console.WriteLine($"Sum of all elements in array = {sumOfArray}");

                        sumOfArray = 0;
                        break;

                    default:
                        int tempValue = 0;

                        if (int.TryParse(userInput, out tempValue))
                        {
                            int[] tempArray = new int[dynamicArray.Length + 1];

                            for (int i = 0; i < dynamicArray.Length; i++)
                            {
                                tempArray[i] = dynamicArray[i];
                            }

                            dynamicArray = tempArray;
                            dynamicArray[dynamicArray.Length - 1] = tempValue;
                        }
                        else
                        {
                            Console.WriteLine("Wrong value or command");
                        }
                        break;
                }
            }
        }
    }
}
