using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    internal class BracketsCounter
    {
        static void Main1(string[] args)
        {
            const char StartSymbol = '(';
            const char FinishSymbol = ')';
            
            int depthMax = 0;
            int depthCurrent = 0;
            string bracketString;

            Console.Write("Input bracket string: ");
            bracketString = Console.ReadLine();

            for (int i = 0; i < bracketString.Length; i++)
            {
                if (bracketString[i] == StartSymbol)
                {
                    depthCurrent++;
                }
                else if (bracketString[i] == FinishSymbol)
                {
                    depthCurrent--;
                }

                if (depthCurrent < 0)
                {
                    break;
                }
                else if (depthCurrent > depthMax)
                {
                    depthMax = depthCurrent;
                }
            }

            if (depthCurrent == 0)
            {
                Console.WriteLine($"String is bracketly correct, max depth = {depthMax}");
            }
            else
            {
                Console.WriteLine("String is incorrect.");
            }

        }
    }
}
