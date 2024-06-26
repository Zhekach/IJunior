using System;
using System.Collections.Generic;

namespace Functions.OOP.Supermarket
{
    internal class SupermarketProgramm
    {
        public static void Main()
        {
            Supermarket supermarket = new Supermarket();
            
            supermarket.Run();
        }
    }

    internal class Supermarket
    {
        private Random _random;
        private float _revenue;
        private List<Client> _clients;
        private readonly List<Product> _products = new List<Product>()
        {
            new Product("Молоко", 56),
            new Product("Яйца", 48),
            new Product("Колбаса", 100),
            new Product("Хлеб", 10.5f),
            new Product("Мороженое", 25),
            new Product("Изюм", 59)
        };

        public Supermarket()
        {
            _random = new Random();
            _clients = new List<Client>();
        }

        public void Run()
        {
            Console.WriteLine("Супермаркет начинает свою работу");
         
            CreateClients();

            foreach (Client client in _clients)
            {
                Console.WriteLine("\nНовый покупатель выбирает покупки");

                client.ChoseProducts();
                
                while (client.IsBasketPaid == false)
                {
                    float totalPrice = GetTotalPrice(client);
                    
                    if (client.TryPayProducts(totalPrice))
                    {
                        _revenue += totalPrice;
                        client.PackProducts();
                    }
                }
            }

            Console.WriteLine($"\nИтого, супермаркет заработал: {_revenue}");
            Console.WriteLine("Нажмите любую клавишу для выхода.");
            Console.ReadKey();
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

        private void CreateClients()
        {
            Console.WriteLine("Появляется очередь из покупателей.");
            _clients.Add(new Client(_random, _products, 5, 100));
            _clients.Add(new Client(_random, _products, 3, 150));
            _clients.Add(new Client(_random, _products, 10, 200));
            _clients.Add(new Client(_random, _products, 4, 100));
        }
    }

    internal class Client
    {
        private int _maxProductsCount;
        private float _money;
        private Bag _bag;

        public Client(Random random, List<Product> products, int maxProductsCount, int money)
        {
            _maxProductsCount = maxProductsCount;
            _money = money;
            _bag = new Bag();
            Random = random;
            Products = products;
            Basket = new Basket(Random, Products);
            IsBasketPaid = false;
        }

        public Random Random { get; private set; }
        public List<Product> Products { get; private set; }
        public Basket Basket { get; private set; }
        public bool IsBasketPaid { get; private set; }

        public void ChoseProducts()
        {
            Basket.AddRandomProducts(_maxProductsCount);
        }

        public bool TryPayProducts(float totalPrice)
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
        private Random _random;
        private List<Product> _productsAvailible;

        public Basket(Random random, List<Product> products)
        {
            _productsAvailible = new List<Product>(products);
            _random = random;
            Products = new List<Product>();
        }

        public List<Product> Products { get; private set; }

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

            Products.Clear();

            return copyOfProducts;
        }

        public void AddRandomProducts(int maxCount)
        {
            int productsCount = _random.Next(1, maxCount + 1);

            for (int i = 0; i < productsCount; i++)
            {
                int newProductId = _random.Next(_productsAvailible.Count);
                Products.Add(_productsAvailible[newProductId]);
                
                Console.WriteLine($"В корзину добавлен новый продукт: {_productsAvailible[newProductId].Name}");
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