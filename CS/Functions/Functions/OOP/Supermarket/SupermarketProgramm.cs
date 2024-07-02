using Functions.OOP.ShopProgram;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Functions.OOP.Supermarket
{
    internal class SupermarketProgramm
    {
        public static void Main1()
        {
            Supermarket supermarket = new Supermarket();

            supermarket.Run();
        }
    }

    internal class Supermarket
    {
        private readonly List<Product> _products;
        private List<Client> _clients;
        private float _revenue;

        public Supermarket()
        {
            _products = CreateProducts();
            _clients = new List<Client>();
        }

        public void Run()
        {
            Console.WriteLine("Супермаркет начинает свою работу");

            _clients = CreateClients();

            foreach (Client client in _clients)
            {
                ServeClient(client);
            }

            Console.WriteLine($"\nИтого, супермаркет заработал: {_revenue}");
            Console.WriteLine("Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }

        public float GetTotalPrice(Client client)
        {
            float totalPrice = 0;

            foreach (Product product in client.Products)
            {
                totalPrice += product.Price;
            }

            Console.WriteLine($"Сумма покупок клиента - {totalPrice}");

            return totalPrice;
        }

        private List<Product> CreateProducts()
        {
            Console.WriteLine("Завозят продукты в магазин.");

            List<Product> products = new List<Product>()
            {
                new Product("Молоко", 56),
                new Product("Яйца", 48),
                new Product("Колбаса", 100),
                new Product("Хлеб", 10.5f),
                new Product("Мороженое", 25),
                new Product("Изюм", 59)
            };

            return products;
        }

        private List<Client> CreateClients()
        {
            Console.WriteLine("Появляется очередь из покупателей.");

            List<Client> clients = new List<Client>()
            {
                new Client(5, 100),
                new Client(3, 150),
                new Client(10, 200),
                new Client(4, 100)
            };

            return clients;
        }

        private void ServeClient(Client client)
        {
            Console.WriteLine("\nНовый покупатель выбирает покупки");

            GiveProducts(client);

            while (client.IsBasketPaid == false)
            {
                float totalPrice = GetTotalPrice(client);

                if (client.CanPayProducts(totalPrice))
                {
                    client.PayProducts(totalPrice);
                    _revenue += totalPrice;
                    client.PackProducts();
                }
                else
                {
                    client.RemoveRandomProduct();
                }
            }
        }

        private void GiveProducts(Client client)
        {
            int availibleProducts = client.MaxProductsCount;

            while (availibleProducts > 0)
            {
                int productIndex = Util.GetRandomNumber(_products.Count);
                Product product = _products[productIndex];

                client.AddProductToBasket(product);

                Console.WriteLine($"В корзину добавлен новый продукт: {product.Name}");

                availibleProducts--;
            }
        }
    }


    internal class Client
    {
        private float _money;
        private List<Product> _basket;
        private List<Product> _bag;

        public Client(int maxProductsCount, int money)
        {
            _money = money;
            _basket = new List<Product>();
            _bag = new List<Product>();

            IsBasketPaid = false;
            MaxProductsCount = maxProductsCount;
        }

        public bool IsBasketPaid { get; private set; }
        public int MaxProductsCount { get; private set; }
        public IEnumerable<Product> Products => _basket;

        public bool CanPayProducts(float totalPrice)
        {
            if (totalPrice <= _money)
            {
                Console.WriteLine("Клиент оплатит покупки");

                return true;
            }
            else
            {
                Console.WriteLine("Клиент не смог оплатить покупки");

                return false;
            }
        }
        public void PackProducts()
        {
            Console.WriteLine("Клиент упаковывает покупки");

            _bag.AddRange(RemoveAllProducts(_basket));

            Console.WriteLine("Клиент упаковал покупки и ушёл довольный");
        }

        public void AddProductToBasket(Product product)
        {
            _basket.Add(product);
        }

        public void PayProducts(float totalPrice)
        {
            _money -= totalPrice;
            IsBasketPaid = true;
        }

        public void RemoveRandomProduct()
        {
            Console.Write("Клиент выкладывает один случайный продукт из корзины: ");

            int productIndex = Util.GetRandomNumber(_basket.Count);

            Console.WriteLine($"{_basket[productIndex].Name}");

            _basket.RemoveAt(productIndex);
        }

        private List<Product> RemoveAllProducts(List<Product> products)
        {
            List<Product> copyOfProducts = new List<Product>();

            foreach (Product product in products)
            {
                copyOfProducts.Add(product);
            }

            products.Clear();

            return copyOfProducts;
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

    internal class Util
    {
        private static Random s_random = new Random();

        public static int GetRandomNumber(int maxValue)
        {
            return s_random.Next(maxValue);
        }

        public static int GetRandomNumber(int minValue, int maxValue)
        {
            return s_random.Next(minValue, maxValue);
        }
    }
}