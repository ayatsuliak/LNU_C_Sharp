using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Event
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"D:\C#\Programming\NewEvents\goods.xml";
            string filePathEvents = @"D:\C#\Programming\NewEvents\events.xml";

            var storage = new Storage();
            storage.ReadData(filePath);
            storage.AddGoods("maslo", 4, 20);
            storage.DeleteGoods("PC");
            storage.WriteData(filePathEvents);
        }
        class GoodsArgs : EventArgs
        {
            public string Name { get; }
            public uint Count { get; }
            public uint Price { get; }
            public GoodsArgs(string name, uint count, uint price)
            {
                Name = name;
                Count = count;
                Price = price;
            }
        }
        class Storage
        {
            public event EventHandler<GoodsArgs> GoodsChanged;
            private List<GoodsArgs> goodEvent = new List<GoodsArgs>();
            public XElement products { get; set; }
            public void ReadData(string filePath) 
            {
                products = XElement.Load(filePath);
            }
            public void AddGoods(string name, uint count, uint price)
            {
                var elem1 = new XElement("good",
                        new XElement("name", name),
                        new XElement("count", count),
                        new XElement("price", price)
                    );
                products.Elements().Append(elem1);
                products.Save(@"D:\C#\Programming\NewEvents\data.xml");
                var elem = new GoodsArgs(name, count, price);
                goodEvent.Add(elem);
                OnGoodsChanged(new GoodsArgs(name, count, price));
            }
            public void DeleteGoods(string name)
            {
                foreach (var i in products.Elements("good"))
                {
                    if((string)i.Element("name") == name)
                    {
                        var count = (uint)i.Element("count");
                        var price = (uint)i.Element("price");
                        var elem = new GoodsArgs(name, count, price);
                        goodEvent.Add(elem);
                        OnGoodsChanged(new GoodsArgs(name, count, price));
                        i.Remove();
                        products.Save(@"D:\C#\Programming\NewEvents\data.xml");
                    }
                }
                
            }
            public void CountInfo(string name, uint count)
            {
                foreach (var i in products.Elements("good"))
                {
                    if ((string)i.Element("name") == name)
                    {
                        var price = (uint)i.Element("price");
                        i.SetElementValue("count", count);
                        var elem = new GoodsArgs(name, count, price);
                        goodEvent.Add(elem);
                        OnGoodsChanged(new GoodsArgs(name, count, price));
                        products.Save(@"D:\C#\Programming\NewEvents\data.xml");
                    }
                }
            }
            public void OnGoodsChanged(GoodsArgs args)
            {
                var handler = GoodsChanged;
                if (handler != null) 
                {
                    handler(this, args);
                }
            }
            public void WriteData(string filePath)
            {
                var result = from i in goodEvent
                             select new
                             {
                                 Name = i.Name,
                                 Count = i.Count,
                                 Price = i.Price
                             };

                var doc = new XElement("result",
                        from i in result
                        select new XElement("good", 
                            new XElement("name", i.Name),
                            new XElement("count", i.Count),
                            new XElement("price", i.Price)
                        )
                    );
                doc.Save( filePath );

            }
        }            
    }
}