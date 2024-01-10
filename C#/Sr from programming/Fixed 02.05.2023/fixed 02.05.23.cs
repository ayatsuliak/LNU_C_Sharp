using System;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathCategories = @"D:\C#\Sr from programming\02.05.2023\categories.xml";
            string pathOperations = @"D:\C#\Sr from programming\02.05.2023\operations.xml";
            string pathReceipts = @"D:\C#\Sr from programming\02.05.2023\receipts.xml";
            string pathForA = @"D:\C#\Sr from programming\Fixed 02.05.2023\taskA.xml";
            string pathForB = @"D:\C#\Sr from programming\Fixed 02.05.2023\taskB.xml";
            string pathForC = @"D:\C#\Sr from programming\Fixed 02.05.2023\taskC.xml";

            using (FileStream f1 = new FileStream(pathCategories, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(pathOperations, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(pathReceipts, FileMode.Open))
                    {
                        var xmlCategories = XElement.Load(f1);
                        var xmlOperations = XElement.Load(f2);
                        var xmlReceipts = XElement.Load(f3);

                        /*(а)xml - файл, де для кожної категорiї виробiв(впорядкування у лексико - графiчному порядку) 
                            вказати кiлькiсть кожної з виконаних операцiй у форматi<назва операцiї: кiлькiсть >, цей перелiк
                            впорядкувати у спадному порядку за кiлькiстю*/

                        var task1 = new XElement(
                                            new XElement("TaskA",
                                                from r in xmlReceipts.Elements("Receipt")
                                                join c in xmlCategories.Elements("Category") on r.Element("CategoryId").Value equals c.Element("CategoryId").Value
                                                join o in xmlOperations.Elements("Operation") on r.Element("OperationId").Value equals o.Element("OperationId").Value
                                                group o by (string)c.Element("Name") into g
                                                orderby g.Key
                                                select new XElement("Category",
                                                    new XElement("Name", g.Key),
                                                    new XElement("Operations",
                                                        from op in g
                                                        group op by (string)op.Element("OperationName") into opGroup
                                                        orderby opGroup.Count() descending
                                                        select new XElement("Operation",
                                                            $"{opGroup.Key}: {opGroup.Count()}"
                                                        )
                                                    )
                                                )
                                            )
                                        );

                        task1.Save(pathForA);

                        /*(б)xml - файл, де для кожної категорiї виробiв(впорядкування у лексико - графiчному порядку) 
                            вказати зароблену суму по кожнiй операцiї<назва операцiї: сума >, цей перелiк впорядкований у
                            спадному порядку стосовно суми*/

                        var task2 = new XElement(
                                            new XElement("TaskB",
                                                from r in xmlReceipts.Elements("Receipt")
                                                join c in xmlCategories.Elements("Category") on r.Element("CategoryId").Value equals c.Element("CategoryId").Value
                                                join o in xmlOperations.Elements("Operation") on r.Element("OperationId").Value equals o.Element("OperationId").Value
                                                group o by (string)c.Element("Name") into g
                                                orderby g.Key
                                                select new XElement("Category",
                                                    new XElement("Name", g.Key),
                                                    new XElement("Operations",
                                                        from op in g
                                                        group op by (string)op.Element("OperationName") into opGroup
                                                        orderby (uint)opGroup.First().Element("Price") descending
                                                        select new XElement("Operation",
                                                            $"{opGroup.Key}: {opGroup.Count() * (uint)opGroup.First().Element("Price")}"
                                                        )
                                                    )
                                                )
                                            )
                                        );

                        task2.Save(pathForB);

                        /*(в)xml - файл, де для заданої категорiї виробiв вказати кiлькiсть виконаних операцiй для виробiв
                            на гарантiї, перелiк впорядкований у спадному порядку за кiлькiстю*/
                        DateTime currentTime = DateTime.Now;
                        string targetCategory = "Technology";

                        var task3 = new XElement(
                            new XElement("TaskC",
                                from c in xmlCategories.Elements("Category")
                                join r in xmlReceipts.Elements("Receipt") on (int)c.Element("CategoryId") equals (int)r.Element("CategoryId")
                                join o in xmlOperations.Elements("Operation") on (int)r.Element("OperationId") equals (int)o.Element("OperationId")
                                where (string)c.Element("Name") == targetCategory && currentTime < DateTime.ParseExact(r.Element("ReleaseDate").Value, "yyyy.MM.dd", CultureInfo.InvariantCulture).AddMonths((int)c.Element("NumOfMon"))
                                group o by (string)o.Element("OperationName") into g
                                orderby g.Count() descending
                                select new XElement("Category",
                                    new XElement("Name", targetCategory),
                                    new XElement("Operations", 
                                        new XElement("Operation", $"{g.Key}: {g.Count()}")
                                    )
                                )
                            )
                        );

                        task3.Save(pathForC);
                    }

                }
            }

        }
    }
}