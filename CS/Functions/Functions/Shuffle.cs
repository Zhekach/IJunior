using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    internal class Shuffle
    {
        static void Main1(string[] args)
        {
            int arraySize = 10;
            int maxArrayValue = 20;
            int[] numbers = new int[arraySize];

            Random random = new Random();
          
            for(int i = 0; i < numbers.Length; i++) 
            {
                numbers[i] = random.Next(maxArrayValue);
            }

            PrintArray(numbers);

            numbers = ShuffleArrray(numbers)

            PrintArray(numbers);
        }

        static int[] ShuffleArrray(int[] baseArray)
        {
            int[] shuffledArray = new int[baseArray.Length];    
            Random random = new Random();

            for(int i = 0;i < shuffledArray.Length; i++)
            {
                shuffledArray[i] = random.Next(i, shuffledArray.Length);
            }
            return shuffledArray;
        }

        static void PrintArray(int[] baseArray)
        {
            foreach (int number in numbers)
            {
                Console.Write(number + " ");
            }
        }
    }
}
