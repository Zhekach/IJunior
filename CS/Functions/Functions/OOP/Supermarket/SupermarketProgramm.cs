using System;
using System.Collections.Generic;
using System.Threading;

//TODO Fix removing components from list
//Fix not random random in random product adding
//Products.RemoveAll(Predicate < Product > true);

namespace Functions.OOP.Supermarket
{
    internal class SupermarketProgramm
    {
        public static void Main()
        {
            Supermarket supermarket = new Supermarket();
            
            List<Client> clients = new List<Client>()
            {
                new Client(supermarket, 5, 100),
                new Client(supermarket, 3, 200),
                new Client(supermarket, 10, 100),
                new Client(supermarket, 4, 100)
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
            Console.WriteLine("Супермаркет начинает свою работу");

            foreach (Client client in clients)
            {
                Console.WriteLine("\nНовый покупатель выбирает покупки");

                client.ChoseProducts();
                
                while (client.IsBasketPaid == false)
                {
                    float totalPrice = GetTotalPrice(client);
                    
                    if (client.PayProducts(totalPrice))
                    {
                        _revenue += totalPrice;
                        client.PackProducts();
                    }
                }
            }

            Console.WriteLine($"\nИтого, супермаркет заработал: {_revenue}");
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
        private Bag _bag;
        
        public Client(Supermarket supermarket, int maxProductsCount, int money)
        {
            _maxProductsCount = maxProductsCount;
            _money = money;
            _supermarket = supermarket;
            _bag = new Bag();
            Basket = new Basket();
            IsBasketPaid = false;
        }

        public Basket Basket { get; private set; }
        public bool IsBasketPaid { get; private set; }

        public void ChoseProducts()
        {
            Basket.AddRandomProducts(_supermarket, _maxProductsCount);
        }

        public bool PayProducts(float totalPrice)
        {
            if (totalPrice <= _money)
            {
                _money -= totalPrice;
                IsBasketPaid = true;

                Console.WriteLine("Клиент оплатил покупки");

                return true;
            }
            else
            {
                Console.WriteLine("Клиент не смог оплатить покупки");
                Console.Write("Клиент выкладывает один продукт: ");
                
                Basket.RemoveRandomProduct();

                return false;
            }
        }

        public void PackProducts()
        {
            Console.WriteLine("Клиент упаковывает покупки");
            
            _bag.AddProducts(Basket.RemoveAllProducts());

            Console.WriteLine("Клиент упаковал покупки и ушёл довольный");
        }
    }

    internal class Basket
    {
        private Random _random = new Random();
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

        public List<Product> RemoveAllProducts()
        {
            List<Product> copyOfProducts = new List<Product>();

            foreach (Product product in Products)
            {
                copyOfProducts.Add(product);
            }

            return copyOfProducts;
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

    internal class Bag
    {
        public List<Product> Products;

        public Bag()
        {
            Products = new List<Product>();
        }

        public void AddProducts(List<Product> newProducts)
        {
            foreach (Product product in newProducts)
            {
                Products.Add(product);
            }
        }
    }

    internal struct Product
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