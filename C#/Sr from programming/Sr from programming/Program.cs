using System;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Collection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            /*var goods = new Goods[]
            {
                new Goods (1, "Bike", 2, 155.5),
                new Goods (2, "Ice cream", 1, 15.6),
                new Goods (3, "Laptop", 3, 100),
                new Goods (4, "PC", 1, 110.5),
                new Goods (5, "Moto", 1, 155.6)
            };

            var categories = new Category[]
            {
                new Category(1, "Motobike", 0.1),
                new Category(2, "Product", 0.05),
                new Category(3, "Machinery", 0.15)
            };

            var releasedGoods = new ReleasedGoods[]
            {
                new ReleasedGoods(1, 2),
                new ReleasedGoods(2, 1),
                new ReleasedGoods(3, 3),
                new ReleasedGoods(4, 1),
                new ReleasedGoods(5, 2)
            };

            Dictionary<uint, uint> countForReleasedGoods = new Dictionary<uint, uint>();
            foreach (var i in releasedGoods)
            {
                if (!countForReleasedGoods.ContainsKey(i.Id))
                {
                    countForReleasedGoods[i.Id] = i.Count;
                }
                else
                {
                    countForReleasedGoods[i.Id] += i.Count;
                }
            }

            foreach (var i in countForReleasedGoods)
            {
                var newGoods = new Goods();
                foreach (var k in goods)
                {
                    if (i.Key == k.Id)
                    {
                        newGoods = k;
                    }
                }
                Console.Write($"Goods name: {newGoods.Name}, Total sold: {i.Value * newGoods.Price}");
            }*/
        }
        class Goods
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public uint CategoryNumber { get; set; }
            public double Price { get; set; }

            public Goods(uint id = 0, string name = "", uint categoryNumber = 0, double price = 0)
            {
                Id = id;
                Name = name;
                CategoryNumber = categoryNumber;
                Price = price;
            }

        }
        class Category
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public double Discount { get; set; }

            public Category(uint id = 0, string name = "", double discount = 0)
            {
                Id = id;
                Name = name;
                Discount = discount;
            }
        }
        class ReleasedGoods
        {
            public uint Id { get; set; }
            public uint Count { get; set; }
            public ReleasedGoods(uint id = 0, uint count = 0)
            {
                Id = id;
                Count = count;
            }
        }
    }
}