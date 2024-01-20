using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Collections
{
    internal class DynamicArrayPro
    {
        static void Main1(string[] args)
        {
            const string SumCommand = "sum";
            const string ExitCommand = "exit";

            List<int> dynamicArrayPro = new List<int>();

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
                        Console.WriteLine($"Sum of all elements in array = {GetSumOfList(dynamicArrayPro)}");
                        break;

                    default:
                        int tempValue = 0;

                        if (int.TryParse(userInput, out tempValue))
                        {
                            dynamicArrayPro.Add(tempValue);
                        }
                        else
                        {
                            Console.WriteLine("Wrong value or command");
                        }
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

        static int GetSumOfList(List<int> list)
        {
            int result = 0;

            foreach (int item in list)
            {
                result += item;
            }

            return result;
        }
    }
}