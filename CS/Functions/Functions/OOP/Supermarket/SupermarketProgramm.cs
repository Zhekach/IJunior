using Functions.OOP.ShopProgram;
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

            GiveProducts(client);

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

        private void GiveProducts(Client client)
        {
            int availibleProducts = client.MaxProductsCount;

            while (availibleProducts > 0)
            {
                int newProductId = Util.GetRandomNumber(_products.Count);
                Product newProduct = _products[newProductId];

                client.AddProductToBasket(newProduct);

                Console.WriteLine($"В корзину добавлен новый продукт: {newProduct.Name}");

                availibleProducts--;
            }
        }
    }


    internal class Client
    {
        private float _money;
        private IEnumerable<Product> _productsAvailible;
        private List<Product> _bag;
        private List<Product> _basket;

        public Client(IEnumerable<Product> productsAvailible, int maxProductsCount, int money)
        {
            _money = money;
            _productsAvailible = productsAvailible;
            _bag = new List<Product>();
            _basket = new List<Product>();
            IsBasketPaid = false;
            MaxProductsCount = maxProductsCount;
        }

        public IEnumerable<Product> Products => _basket;
        public bool IsBasketPaid { get; private set; }
        public int MaxProductsCount { get; private set; }

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

                RemoveRandomProduct(_basket);

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

        private void RemoveRandomProduct(List<Product> products)
        {
            Console.Write("Клиент выкладывает один случайный продукт из корзины: ");

            int removeIndex = Util.GetRandomNumber(products.Count);

            Console.WriteLine($"{products[removeIndex].Name}");

            products.RemoveAt(removeIndex);
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