using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    internal class UIElement
    {
        static void Main (string[] args)
        {
            int maxPercent = 100;
            int percent = 75;
            int barLength = 20;

            PrintBar(percent, maxPercent, barLength);
        }

        static void PrintBar(int percent, int maxPercent, int barLength)
        {
            char startChar = '[';
            char fullChar = '#';
            char emptyChar = '_';
            char finishChar = ']';
            int fullCharCount = 0;

            fullCharCount = percent / (maxPercent / barLength);
            
            Console.SetCursorPosition(2, 20);
            Console.Write(startChar);

            for(int i = 0; i < fullCharCount;  i++)
            {
                Console.Write(fullChar);
            }

            for (int i = fullCharCount; i < barLength; i++)
            {
                Console.Write(emptyChar);
            }

            Console.WriteLine(finishChar);
        }
    }
}
