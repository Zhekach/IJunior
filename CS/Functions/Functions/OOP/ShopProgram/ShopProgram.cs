using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.OOP.ShopProgram
{
    internal class ShopProgram
    {
        static void Main1()
        {
            Shop shop = new Shop();
            shop.Run();
        }
    }

    class Shop
    {
        private Seller _seller;
        private Buyer _buyer;
        private bool _isUserExited;

        public Shop()
        {
            Random random = new Random();
            _seller = new Seller();
            _buyer = new Buyer(random);
        }

        public void Run()
        {
            while (_isUserExited == false)
            {
                PrintUI();

                int userInput = ReadInt();

                switch (userInput)
                {
                    case (int)UserCommands.ShowProducts:
                        _seller.PrintProductsInfo();
                        break;

                    case (int)UserCommands.BuyProductByIndex:
                        SellProduct();
                        break;

                    case (int)UserCommands.ShowBuyerMoney:
                        _buyer.PrintMoney();
                        break;

                    case (int)UserCommands.ShowBuyerProducts:
                        _buyer.PrintProductsInfo();
                        break;

                    case (int)UserCommands.Exit:
                        _isUserExited = true;
                        break;

                    default:
                        Console.WriteLine("Вы ввели неверную команду. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения.");
                Console.ReadKey();
                Console.Clear();
            }

        }

        public void SellProduct()
        {
            Console.WriteLine("введит номер продукта, который хотите купить.");

            int index = ReadInt();
            index--;
            _seller.SellProductByIndex(index, _buyer);
        }

        private void PrintUI()
        {
            Console.WriteLine("Введите команду:\n" +
                $"{(int)UserCommands.ShowProducts} - показать товары в магазине\n" +
                $"{(int)UserCommands.BuyProductByIndex} - купить продукт по его номеру\n" +
                $"{(int)UserCommands.ShowBuyerMoney} - показать мои деньги\n" +
                $"{(int)UserCommands.ShowBuyerProducts} - показать мои покупки\n" +
                $"{(int)UserCommands.Exit} - выйти из программы\n");
        }

        private int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                string enteredString;

                Console.WriteLine("Введите число:");
                enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                if (isIntEntered)
                {
                    Console.WriteLine($"Введенное число распознано: {parsedInt}");
                }
                else
                {
                    Console.WriteLine("Вы ввели некорректное число. Попробуйте ещё.\n");
                }
            }

            return parsedInt;
        }
    }

    abstract class Person
    {
        protected int Money;
        protected List<Product> Products;

        public void PrintProductsInfo()
        {
            for (int i = 0; i < Products.Count; i++)
            {
                Console.Write(i + 1 + ". ");

                Products[i].PrintInfo();
            }
        }
    }

    class Seller : Person
    {
        public Seller()
        {
            Money = 0;
            Products = new List<Product>()
            {
                new Product("Хлеб", 25),
                new Product("Колбаса", 500),
                new Product("Яйца", 1400),
                new Product("Молоко", 75),
                new Product("Хамон", 400)
            };
        }

        public void SellProductByIndex(int index, Buyer buyer)
        {
            if (index < 0 || index > Products.Count)
            {
                Console.WriteLine("Нет такого товара.");
            }
            else
            {
                Product product = Products[index];

                if (buyer.HaveEnoughMoney(product.Price))
                {
                    Money += buyer.BuyProduct(product);
                    Products.Remove(product);
                    Console.WriteLine("Товар куплен");
                }
                else
                {
                    Console.WriteLine("Похоже, у вас не хватает денег, хе-хе");
                }
            }
        }
    }

    class Buyer : Person
    {
        public Buyer(Random random)
        {
            Money = random.Next(100, 1001);
            Products = new List<Product>();
        }

        public void PrintMoney()
        {
            Console.WriteLine($"У вас осталось {Money} рублей");
        }

        public bool HaveEnoughMoney(int price)
        {
            return Money >= price;
        }

        public int BuyProduct(Product product)
        {
            Products.Add(product);
            Money -= product.Price;

            return product.Price;
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }

        public void PrintInfo()
        {
            Console.WriteLine($"Товар - {Name}, стоит - {Price} рублей.");
        }
    }

    enum UserCommands
    {
        ShowProducts = 1,
        BuyProductByIndex = 2,
        ShowBuyerMoney = 3,
        ShowBuyerProducts = 4,
        Exit = 5,
    }
}