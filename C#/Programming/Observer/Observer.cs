using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Observer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ibm = new IBM("IBM", 120.00);
            ibm.Attach(new Investor("Sorros"));
            ibm.Attach(new Investor("Berkshire"));

            ibm.Price = 120.10;
            ibm.Price = 121.0;
            ibm.Price = 120.50;
            ibm.Price = 120.75;

            Console.ReadKey();
        }

        public abstract class Stock
        {
            private double price;
            public Stock(string s, double p)
            {
                Symbol = s;
                price = p;
            }
            public List<IInvestor> Investors { get; set; } = new List<IInvestor>();
            public void Attach(IInvestor investor)
            {
                Investors.Add(investor);
            }
            public void Detach(IInvestor investor)
            {
                Investors.Remove(investor);
            }
            public void Notify()
            {
                foreach (var investor in Investors)
                {
                    investor.Update(this);
                }
                Console.WriteLine("");
            }
            public double Price
            {
                get { return price; }
                set
                {
                    if (price != value)
                    {
                        price = value;
                        Notify();
                    }
                }
            }
            public string Symbol { get; }
        }
        public class IBM: Stock 
        {
            public IBM(string symbol, double price):
                base(symbol, price) { }
        }
        public interface IInvestor 
        {
            void Update(Stock stock);              
        }
        public class Investor : IInvestor 
        {
            public Investor(string name)
            {
                Name = name;
            }
            public void Update(Stock stock) 
            {
                Console.WriteLine($"Notified {Name} of {stock.Symbol}'s change to {stock.Price:C}");
            }
            public string Name { get; }
            public Stock Stock { get; set;  }
        }
    }
}