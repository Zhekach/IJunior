using System;

namespace Functions
{
    internal class KanzasShuffle
    {
        static void Main1(string[] args)
        {
            int arraySize = 10;
            int maxArrayValue = 20;
            int[] numbers = new int[arraySize];

            Random random = new Random();

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(maxArrayValue);
            }

            PrintArray(numbers);

            numbers = ShuffleArrray(numbers);

            PrintArray(numbers);
        }

        static int[] ShuffleArrray(int[] baseArray)
        {
            int[] shuffledArray = new int[baseArray.Length];
            Random random = new Random();

            for (int i = 0; i < shuffledArray.Length; i++)
            {
                int index = random.Next(baseArray.Length);
                shuffledArray[i] = baseArray[index];
                baseArray = DeleteElementInArray(baseArray, index);
            }
            return shuffledArray;
        }

        static void PrintArray(int[] baseArray)
        {
            foreach (int number in baseArray)
            {
                Console.Write(number + " ");
            }

            Console.WriteLine("");
        }

        static int[] DeleteElementInArray(int[] baseArray, int index)
        {
            int[] newArray = new int[baseArray.Length - 1];

            for (int i = 0; i < index; i++)
            {
                newArray[i] = baseArray[i];
            }

            for (int i = index; i < newArray.Length; i++)
            {
                newArray[i] = baseArray[i + 1];
            }

            return newArray;
        }
    }
}
