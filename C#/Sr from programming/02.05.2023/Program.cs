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
            string pathForA = @"D:\C#\Sr from programming\02.05.2023\taskA.xml";
            string pathForB = @"D:\C#\Sr from programming\02.05.2023\taskB.xml";
            string pathForC = @"D:\C#\Sr from programming\02.05.2023\taskC.xml";

            using (FileStream f1 = new FileStream(pathCategories, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(pathOperations, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(pathReceipts, FileMode.Open))
                    {
                        var xmlCategories = XElement.Load(f1);
                        var xmlOperations = XElement.Load(f2);
                        var xmlReceipts = XElement.Load(f3);

                        var task1 = new XElement(
                                            new XElement("TaskA",
                                                from r in xmlReceipts.Elements("Receipt")
                                                join c in xmlCategories.Elements("Category") on r.Element("CategoryId").Value equals c.Element("CategoryId").Value
                                                join o in xmlOperations.Elements("Operation") on r.Element("OperationId").Value equals o.Element("OperationId").Value
                                                group o by (string)c.Element("Name") into g
                                                orderby g.Count() descending
                                                select new XElement("Category",
                                                    new XElement("Name", g.Key),
                                                    new XElement("Operations",
                                                        from op in g
                                                        group op by (string)op.Element("OperationName") into opGroup
                                                        select new XElement("Operation",
                                                            $"{opGroup.Key}: {opGroup.Count()}"
                                                        )
                                                    )
                                                )
                                            )
                                        );


                        task1.Save(pathForA);

                        var task2 = new XElement(
                                        new XElement("TaskB",
                                        from r in xmlReceipts.Elements("Receipt")
                                        join c in xmlCategories.Elements("Category") on r.Element("CategoryId").Value equals c.Element("CategoryId").Value into categoryReceipts
                                        join o in xmlOperations.Elements("Operation") on r.Element("OperationId").Value equals o.Element("OperationId").Value into categoryOperations
                                        let operationCounts = categoryOperations.GroupBy(op => (string)op.Element("OperationName"))
                                                                                    .Select(op => new
                                                                                    {
                                                                                        OperationName = op.Key,
                                                                                        TotalAmount = op.Count() * (int)op.First().Element("Price")
                                                                                    })
                                                                                    .OrderByDescending(op => op.TotalAmount)
                                            orderby (string)categoryReceipts.First().Element("Name")
                                            select new XElement("Category",
                                                new XElement("Name", (string)categoryReceipts.First().Element("Name")),
                                                new XElement("Operations",
                                                    from opCount in operationCounts
                                                    select new XElement("Operation", $"{opCount.OperationName}: {opCount.TotalAmount}")
                                                )
                                            )
                                        )
                                    );

                        task2.Save(pathForB);


                        string targetCategory = "Technology"; // Задана категорія виробів

                        var task3 = new XElement(
                            new XElement("TaskC",
                                from c in xmlCategories.Elements("Category")
                                where (string)c.Element("Name") == targetCategory
                                join r in xmlReceipts.Elements("Receipt") on (int)c.Element("CategoryId") equals (int)r.Element("CategoryId")
                                join o in xmlOperations.Elements("Operation") on (int)r.Element("OperationId") equals (int)o.Element("OperationId")
                                group o by (string)o.Element("OperationName") into g
                                orderby g.Count() descending
                                select new XElement("Category",
                                    new XElement("Name", targetCategory),
                                    new XElement("Operations",
                                        from op in g
                                        select new XElement("Operation", $"{op.Element("OperationName")}: {g.Count()}")
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