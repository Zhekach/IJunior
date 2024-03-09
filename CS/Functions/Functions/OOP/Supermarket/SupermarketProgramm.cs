using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.OOP.Supermarket
{
    internal class SupermarketProgramm
    {
    }

    internal class Supermarket
    {
        public readonly List<Product> priducts = new List<Product>() 
            {
                new Product("Молоко", 56),
                new Product("Яйца", 48),
                new Product("Колбаса", 100),
                new Product("Хлеб", 10.5f),
                new Product("Мороженое", 25),
                new Product("Изюм", 59)
            };


    }

    internal class Client
    {
        public Basket basket;

        public Client(Supermarket supermarket)
        {
            
        }

        private Product GetProduct(Product product)
        {
            Product newProduct = null;
            return newProduct;
        }
    }

    internal class Basket
    {
        private readonly Random _random = new Random();
        public List<Product> Products;

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
            Products.RemoveAt(removeIndex);
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }

    internal class Product
    {
        public Product (string name, float price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        
        public float Price { get; private set;}

        public void PrintInfo()
        {
            Console.WriteLine($"Товар {Name} стоит {Price} рублей.");
        }
    }
}
