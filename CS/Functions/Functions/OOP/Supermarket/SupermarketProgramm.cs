using System;
using System.Collections.Generic;

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

        private IEnumerable<Product> _productsCopy => _products;

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
                new Client(_productsCopy, 5, 100),
                new Client(_productsCopy, 3, 150),
                new Client(_productsCopy, 10, 200),
                new Client(_productsCopy, 4, 100)
            };

            return clients;
        }

        private void ServeClient(Client client)
        {
            Console.WriteLine("\nНовый покупатель выбирает покупки");

            client.ChoseProducts(_productsCopy);

            while (client.IsBasketPaid == false)
            {
                float totalPrice = GetTotalPrice(client);

                if (client.CanPayProducts(totalPrice))
                {
                    _revenue += totalPrice;
                    client.PackProducts();
                }
            }
        }
    }

    internal class Client
    {
        private int _maxProductsCount;
        private float _money;
        private IEnumerable<Product> _productsAvailible;
        private List<Product> _bag;
        private Basket _basket;

        public Client(IEnumerable<Product> productsAvailible, int maxProductsCount, int money)
        {
            _maxProductsCount = maxProductsCount;
            _money = money;
            _bag = new List<Product>();
            _productsAvailible = productsAvailible;
            _basket = new Basket();
            IsBasketPaid = false;
        }

        public IEnumerable<Product> Products => _basket.Products;
        public bool IsBasketPaid { get; private set; }

        public void ChoseProducts(IEnumerable<Product> products)
        {
            List<Product> productsAvailible = new List<Product>(products);
            _basket.AddRandomProducts(productsAvailible, _maxProductsCount);
        }

        public bool CanPayProducts(float totalPrice)
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

                RemoveRandomProduct();

                return false;
            }
        }

        public void RemoveRandomProduct()
        {
            Console.Write("Клиент выкладывает один случайный продукт из корзины: ");

            _basket.RemoveRandomProduct();
        }

        public void PackProducts()
        {
            Console.WriteLine("Клиент упаковывает покупки");

            _bag.AddRange(_basket.RemoveAllProducts());

            Console.WriteLine("Клиент упаковал покупки и ушёл довольный");
        }
    }

    internal class Basket
    {
        private Random _random;
        private List<Product> _products;

        public Basket()
        {
            _random = Util.Random;
            _products = new List<Product>();
        }

        public IEnumerable<Product> Products => _products;

        public void RemoveRandomProduct()
        {
            int removeIndex = _random.Next(_products.Count);

            Console.WriteLine($"{_products[removeIndex].Name}");

            _products.RemoveAt(removeIndex);
        }

        public List<Product> RemoveAllProducts()
        {
            List<Product> copyOfProducts = new List<Product>();

            foreach (Product product in _products)
            {
                copyOfProducts.Add(product);
            }

            _products.Clear();

            return copyOfProducts;
        }

        public void AddRandomProducts(List<Product> productsAvailible, int maxCount)
        {
            int productsCount = _random.Next(1, maxCount + 1);

            for (int i = 0; i < productsCount; i++)
            {
                int newProductId = _random.Next(productsAvailible.Count);
                _products.Add(productsAvailible[newProductId]);

                Console.WriteLine($"В корзину добавлен новый продукт: {productsAvailible[newProductId].Name}");
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

    internal class Util
    {
        public static Random Random = new Random();
    }
}