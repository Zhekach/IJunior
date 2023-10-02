using System;
class SumOfNumbers
{
    static void Main1()
    {
        int maxNumber = 100;
        int dividerFirst = 3;
        int dividerSecond = 5;
        int targetRandomNumber;
        int sumOfNumbers = 0;

        Random rand = new Random();
        targetRandomNumber = rand.Next(maxNumber + 1);
        Console.WriteLine($"Случайное число: {targetRandomNumber}");

        for (int i = 0; i <= targetRandomNumber; i++)
        {
            if (i % dividerFirst == 0 || i % dividerSecond == 0)
            {
                Console.Write(i + " ");
                sumOfNumbers += i;
            }
        }

        Console.WriteLine(" ");
        Console.WriteLine($"Искомая сумма чисел: {sumOfNumbers}");
    }
}