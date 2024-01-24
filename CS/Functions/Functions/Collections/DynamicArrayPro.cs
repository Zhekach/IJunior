using System;
using System.Collections.Generic;

namespace Functions.Collections
{
    internal class DynamicArrayPro
    {
        static void Main1(string[] args)
        {
            const string SumCommand = "sum";
            const string ExitCommand = "exit";

            List<int> integers = new List<int>();

            bool isUserExited = false;

            PrintUI(SumCommand, ExitCommand);

            while (isUserExited == false)
            {
                Console.Write("Enter new element, or command:");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case ExitCommand:
                        isUserExited = true;
                        break;

                    case SumCommand:
                        PrintSumOfList(integers);
                        break;

                    default:
                        AddByUserInput(integers, userInput);
                        break;
                }
            }
        }

        static void PrintUI(string sumCommand, string exitCommand)
        {
            Console.WriteLine("Enter integer value to add it to the array.\n" +
                            $"Enter \"{sumCommand}\" to show sum of current elements.\n" +
                            $"Enter \"{exitCommand}\" to close program.");
        }

        static void PrintSumOfList(List<int> list)
        {
            int sum = 0;

            foreach (int item in list)
            {
                sum += item;
            }

            Console.WriteLine($"Sum of all elements in array = {sum}");
        }

        static void AddByUserInput(List<int> list, string userInput)
        {

            if (int.TryParse(userInput, out int tempValue))
            {
                list.Add(tempValue);
            }
            else
            {
                Console.WriteLine("Wrong value or command");
            }
        }
    }
}