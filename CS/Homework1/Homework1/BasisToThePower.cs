using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    internal class BasisToThePower
    {
        static void Main1(string[] args)
        {
            const int Basis = 2;
            int minNRange = 0;
            int maxNRange = 1000;
            Random random = new Random();
            int numberN = random.Next(minNRange, maxNRange + 1);
            int basisToPower = 0;
            int resultNumber;

            Console.WriteLine($"Number N in range [{minNRange};{maxNRange}]: {numberN}");

            for (resultNumber = 1; resultNumber <= numberN; resultNumber *= Basis)
            {
                basisToPower++;
            }

            Console.WriteLine($"Result number: {resultNumber}, this is two to the power of: {basisToPower}");
        }
    }
}
