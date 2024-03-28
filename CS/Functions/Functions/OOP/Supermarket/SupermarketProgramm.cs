using System;
using System.Collections.Generic;
using System.Threading;

namespace Functions.OOP.Supermarket
{
    internal class SupermarketProgramm
    {
        public static void Main()
        {
            Supermarket supermarket = new Supermarket();
            
            List<Client> clients = new List<Client>()
            {
                new Client(supermarket, 5, 100 ),
                new Client(supermarket, 3, 200),
                //new Client(supermarket, 100),
                //new Client(supermarket, 100)
            };
            
            supermarket.Run(clients);
        }
    }

    internal class Supermarket
    {
        private float _revenue;
        public readonly List<Product> Products = new List<Product>()
        {
            new Product("Молоко", 56),
            new Product("Яйца", 48),
            new Product("Колбаса", 100),
            new Product("Хлеб", 10.5f),
            new Product("Мороженое", 25),
            new Product("Изюм", 59)
        };

        public void Run(List<Client> clients)
        {
            foreach (Client client in clients)
            {
                bool hasClientPaid = false;
                client.ChoseProducts();
                
                while (hasClientPaid == false)
                {
                    float totalPrice = GetTotalPrice(client);
                    
                    if (client.PayProducts(totalPrice))
                    {
                        _revenue += totalPrice;
                        hasClientPaid = true;

                        Console.WriteLine("Клиент оплатил покупки");
                    }
                    else
                    {
                        
                        Console.WriteLine("Клиент не смог оплатить покупки");
                    }
                    //TODO вариант, если у клиента всё равно недостаточно денег!!!
                }
            }

            Console.WriteLine($"Итого, супермаркет заработал: {_revenue}");
        }

        public float GetTotalPrice(Client client)
        {
            float totalPrice = 0;
            
            foreach (Product product in client.Basket.Products)
            {
                totalPrice += product.Price;
            }

            Console.WriteLine($"Сумма покупок клиента - {totalPrice}");
            
            return totalPrice;
        }
    }

    internal class Client
    {
        private int _maxProductsCount;
        private float _money;
        private Supermarket _supermarket;
        
        public Client(Supermarket supermarket, int maxProductsCount, int money)
        {
            _maxProductsCount = maxProductsCount;
            _money = money;
            _supermarket = supermarket;
            Basket = new Basket();
        }

        public Basket Basket { get; private set; }

        public void ChoseProducts()
        {
            Basket.AddRandomProducts(_supermarket, _maxProductsCount);
        }

        public bool PayProducts(float totalPrice)
        {
            if (totalPrice <= _money)
            {
                _money -= totalPrice;
                return true;
            }
            else
            {
                Basket.RemoveRandomProduct();

                Console.Write("Клиент выложил один продукт: ");
                
                return false;
            }
        }
    }

    internal class Basket
    {
        private readonly Random _random = new Random();
        public List<Product> Products;

        public Basket()
        {
            Products = new List<Product>();
        }

        public void PrintInfo()
        {
            Console.WriteLine("В корзине находится:");

            foreach (Product product in Products)
            {
                product.PrintInfo();
            }
        }

        public void RemoveRandomProduct()
        {
            int removeIndex = _random.Next(Products.Count);

            Console.WriteLine($"{Products[removeIndex].Name}");
            
            Products.RemoveAt(removeIndex);
        }

        public void AddRandomProducts(Supermarket supermarket, int maxCount)
        {
            int productsCount = _random.Next(1, maxCount + 1);

            for (int i = 0; i < productsCount; i++)
            {
                Thread.Sleep(1000);

                int newProductId = _random.Next(supermarket.Products.Count);
                Products.Add(supermarket.Products[newProductId]);
                
                Console.WriteLine($"В корзину добавлен новый продукт: {supermarket.Products[newProductId].Name}");
            }
        }
    }

    internal class Product
    {
        public Product(string name, float price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }

        public float Price { get; private set; }

        public void PrintInfo()
        {
            Console.WriteLine($"Товар {Name} стоит {Price} рублей.");
        }
    }
}