using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    internal class Test
    {
        static void Main1(string[] args)
        {
            string test = "One";
            Add(test);
            Console.WriteLine(test);
        }

        static void Add(string test)
        {
            test = test + "two";
            Console.WriteLine(test + 1);
        }
        
    }

}
