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
            var ibm = new IBM("IBM", 120.00);

            ibm.Price = 120.10;
            var ibm_S = new Investor("Sorros", ibm);
            ibm.Price = 121.0;
            var ibm_B = new Investor("Berkshire", ibm);
            ibm.Price = 120.50;
            ibm.Price = 120.75;
        }
        public class StockEventArgs : EventArgs
        {
            public StockEventArgs(string s) 
            {
                Message = s;
            }
            public string Message { get; set; }
        }
        public abstract class Stock
        {
            private double price;
            public event EventHandler<StockEventArgs> RaiseStockEvent;
            public Stock(string s, double p)
            {
                Symbol = s;
                price = p;
            }
            protected virtual void OnRaiseStockEvent(StockEventArgs e)
            {
                var handler = RaiseStockEvent;

                if (handler != null) 
                {
                    e.Message += $" at {DateTime.Now}";

                    handler(this, e);
                }
            }
            public double Price
            {
                get { return price; }
                set 
                { 
                    if(price != value ) 
                    {
                        price = value;
                        OnRaiseStockEvent(new StockEventArgs("price was changed"));
                    }
                }
            }
            public string Symbol { get; }
        }
        public class IBM : Stock
        {
            public IBM(string symbol, double price) :
                base(symbol, price)
            { }
        }
        public interface IInvestor
        {
            void Update(object sender, StockEventArgs e);
        }
        public class Investor : IInvestor
        {
            public Investor(string name, Stock stock)
            {
                Name = name;
                stock.RaiseStockEvent += Update;
            }
            public void Update(object sender, StockEventArgs e)
            {
                Console.WriteLine($"Notified {Name} of {((Stock)sender).Symbol}'s change to {((Stock)sender).Price:C} -- {e.Message}");
            }
            public string Name { get; }
            public Stock Stock { get; set; }
        }
    }
}