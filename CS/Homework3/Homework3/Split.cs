using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    internal class Split
    {
        static void Main1(string[] args)
        {
            string testString = "Мама мыла раму";
            char splitChar = ' ';
            string[] split = testString.Split(splitChar);

            foreach (var item in split)
            {
                Console.WriteLine(item);
            }
        }
    }
}
