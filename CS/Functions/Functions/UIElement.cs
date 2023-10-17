using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    internal class UIElement
    {
        static void Main1(string[] args)
        {
            int maxPercent = 100;
            int filledPercent = 75;
            int barLength = 20;

            PrintBar(filledPercent, maxPercent, barLength, fullChar: '#');
        }

        static void PrintBar(int filledPercent, int maxPercent, int barLength,
            char startChar = '[', char fullChar = '*', char emptyChar = '_', char finishChar = ']')
        {
            int filledLength = 0;

            filledLength = filledPercent * barLength / maxPercent;

            Console.SetCursorPosition(2, 20);
            Console.Write(startChar);

            PrintChars(filledLength, fullChar);
            PrintChars(barLength - filledLength, emptyChar);

            Console.WriteLine(finishChar);
        }

        static void PrintChars(int charsCount, char charToPrint)
        {
            for (int i = 0; i < charsCount; i++)
            {
                Console.Write(charToPrint);
            }
        }
    }
}
