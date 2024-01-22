using System;
using System.Collections.Generic;


namespace Functions.Collections
{
    internal class QueueInShop
    {
        static void Main1(string[] args)
        {
            Queue<int> purchaseAmounts = new Queue<int>();

            int purchasesCount = 5;
            int amountMaxValue = 25;
            int accountValue = 0;

            CreateQueue(purchaseAmounts, purchasesCount, amountMaxValue);

            while (purchaseAmounts.Count > 0)
            {
                PrintQueue(purchaseAmounts);
                PrintUI(accountValue);
                ServeCustomer(purchaseAmounts, ref accountValue);

                Console.Clear();
            }

            Console.WriteLine($"По итогу денег заработано: {accountValue}");
        }

        static void PrintUI(int accountValue)
        {
            Console.SetCursorPosition(0, 3);
            Console.WriteLine($"Текущий счёт: {accountValue}");
            Console.WriteLine("Нажмите любую клавишу для обслуживания следующего клиента");
        }

        static void CreateQueue(Queue<int> queue, int size, int maxValue)
        {
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                queue.Enqueue(random.Next(maxValue));
            }
        }

        static void PrintQueue(Queue<int> purchaseAmounts)
        {
            Console.Write($"Сейчас в очереди {purchaseAmounts.Count} клиентов с суммами покупок: ");

            foreach (int purchase in purchaseAmounts)
            {
                Console.Write(purchase + ", ");
            }
        }

        static void ServeCustomer(Queue<int> purchaseAmounts, ref int accountValue)
        {
            Console.ReadKey();

            accountValue += purchaseAmounts.Peek();
            purchaseAmounts.Dequeue();
        }
    }
}
