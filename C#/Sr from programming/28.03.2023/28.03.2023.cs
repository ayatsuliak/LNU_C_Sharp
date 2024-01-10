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
            var goods = new Goods[]
            {
                new Goods (4, "Bike", 1, 155.5),
                new Goods (2, "Ice cream", 2, 15.6),
                new Goods (3, "Laptop", 3, 100),
                new Goods (5, "PC", 3, 110.5),
                new Goods (1, "Moto", 1, 155.6)
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


            //a) для кожного вiдпущеного товару, заданого назвою, вказати загальну кiлькiсть проданих екземплярiв;
            //для такого перелiку застосувати лексико-графiчне впорядкування;
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
            Dictionary<string, uint> newDict = new Dictionary<string, uint>();

            foreach (var i in countForReleasedGoods)
            {
                var newGoods = new Goods();
                foreach (var k in goods)
                {
                    if (i.Key == k.Id)
                    {
                        newGoods = k;
                        newDict.Add(k.Name, i.Value);
                    }
                }
                Console.WriteLine($"Goods name: {newGoods.Name}, Count: {i.Value}");
            }
            var sortedDict = newDict.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            Console.WriteLine("---------------------------------------");
            foreach(var i in sortedDict)
            {
                Console.WriteLine($"Goods name: {i.Key}, Count: {i.Value}");
            }
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("---------------------------------------");
            //b) для кожної категорiї товарiв, заданої назвою, вказати перелiк вiдпущених товарiв, в якому
            //для кожного з товарiв вказувати назву i сумарну вартiсть; категорiї впорядкувати лексико-графiчно,
            //а товари у спадному порядку стосовно суми;
            Dictionary<uint, Dictionary<uint, double>> dict_for_b = new Dictionary<uint, Dictionary<uint, double>>();
            foreach(var i in categories)
            {
                dict_for_b.TryAdd(i.Id, new Dictionary<uint, double>());
                foreach(var j in goods)
                {
                    if(i.Id == j.CategoryNumber)
                    {
                        if (dict_for_b[i.Id].ContainsKey(j.Id))
                        {
                            dict_for_b[i.Id][j.Id] += j.Price - (j.Price * i.Discount);
                        }
                        else
                        {
                            dict_for_b[i.Id].Add(j.Id, (j.Price - (j.Price * i.Discount)));
                        }
                    }
                    
                }
            }
            var sortedDictionary = dict_for_b.OrderBy(x => x.Key)
                            .ToDictionary(x => x.Key, x => x.Value.OrderByDescending(y => y.Value).ToDictionary(y => y.Key, y => y.Value));

            var newD = new Dictionary<string, Dictionary<uint, double>>();
            var newD2 = new Dictionary<string, double>();

            foreach (var i in sortedDictionary)
            {
                var new_category = new Category();
                foreach (var k in categories)
                {
                    if (i.Key == k.Id)
                    {
                        new_category = k;
                        newD.Add(new_category.Name, i.Value);
                    }
                }
                Console.WriteLine(new_category.Name, ":");
                foreach (var j in i.Value)
                {
                    var new_product = new Goods();
                    foreach (var l in goods)
                    {
                        if (j.Key == l.Id)
                        {
                            new_product = l;
                            newD2.Add(new_product.Name, j.Value);
                        }
                    }
                    Console.WriteLine($" {new_product.Name}, {j.Value}");
                }                
            }
            var sortedDict2 = newD.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            Console.WriteLine("---------------------------------------");

            foreach (var i in sortedDict2)
            {
                Console.WriteLine($"{i.Key}:");
                foreach (var j in i.Value)
                {
                    var new_product = new Goods();
                    foreach (var l in goods)
                    {
                        if (j.Key == l.Id)
                        {
                            new_product = l;
                        }
                    }
                    Console.WriteLine($" {new_product.Name}, {j.Value}");
                }
            }
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("---------------------------------------");

            //c) перелiком категорiй у форматi < назва категорiї > , <загальна вартiсть вiдпущених товарiв>;
            //перелiк впорядкувати у спадному порядку за вартiстю.

        }
        public class Goods
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

        public class GoodsCompare<T> : IComparer<T> where T : Goods
        {
            public int Compare(T first, T second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal);
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