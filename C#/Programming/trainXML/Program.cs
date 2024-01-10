using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace trainXML
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string goodsPath = @"D:\C#\Programming\trainXML\goods.xml";
            string clientsPath = @"D:\C#\Programming\trainXML\clients.xml";
            string ordersPath = @"D:\C#\Programming\trainXML\orders.xml";

            using(FileStream f1 = new FileStream(goodsPath, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(clientsPath, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(ordersPath, FileMode.Open))
                    {
                        

                    }
                }
            }
        }
    }
}
