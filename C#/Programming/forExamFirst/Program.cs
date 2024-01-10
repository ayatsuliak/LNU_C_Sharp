using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePathPosv = @"D:\C#\Programming\forExamFirst\posvitchennia.xml";
            string filePathPractic = @"D:\C#\Programming\forExamFirst\practic.xml";
            string filePathTeory = @"D:\C#\Programming\forExamFirst\teory.xml";
            string filePatInfo = @"D:\C#\Programming\forExamFirst\infos.xml";
            string filePathTaskA = @"D:\C#\Programming\forExamFirst\forTaskA.xml";
            string filePathTaskB = @"D:\C#\Programming\forExamFirst\forTaskB.xml";
            string filePathTaskC = @"D:\C#\Programming\forExamFirst\forTaskC.xml";
            string filePathTaskD = @"D:\C#\Programming\forExamFirst\forTaskD.xml";

            using (FileStream f1 = new FileStream(filePathPosv, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(filePathPractic, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(filePathTeory, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(filePatInfo, FileMode.Open))
                        {
                            var illums = XElement.Load(f1);
                            var practics = XElement.Load(f2);
                            var teories = XElement.Load(f3);
                            var infos = XElement.Load(f4);

                            var result = from i in infos.Elements("info")
                                         join il in illums.Elements("illumination") on (uint)i.Element("il_id") equals (uint)il.Element("id")
                                         join p in practics.Elements("practic") on (uint)i.Element("prac_id") equals (uint)p.Element("id")
                                         join t in teories.Elements("teory") on (uint)i.Element("teo_id") equals (uint)t.Element("id")
                                         select new
                                         {
                                             LastName = (string)il.Element("Lastname"),
                                             Category = (string)il.Element("category"),
                                             Date = (uint)(((DateTime)il.Element("date") - (DateTime)i.Element("date")).TotalDays / 365),
                                             Points = (uint)p.Element("point") + (uint)t.Element("point"),
                                             QuestionName = (string)t.Element("name"),
                                             QuestionPoint = (uint)t.Element("point")
                                         };

                            //a
                            var taskA = new XElement("taskA",
                                    from i in result
                                    group i by i.LastName into g
                                    orderby g.Key
                                    select new XElement("person", new XAttribute("name", g.Key),
                                        from i in g
                                        select new XElement("category", i.Category),
                                        from i in g
                                        select new XElement("years", i.Date)
                                    )
                                );

                            taskA.Save(filePathTaskA);

                            //b
                            var taskB = new XElement("taskB",
                                    from i in result
                                    group i by i.LastName into g
                                    orderby g.Key
                                    select new XElement("person", new XAttribute("name", g.Key),
                                        from i in g
                                        select new XElement("category", i.Category),
                                        from i in g
                                        select new XElement("years", i.Date),
                                        from i in g
                                        select new XElement("points", i.Points)
                                    )
                                );

                            taskB.Save(filePathTaskB);

                            //c
                            var taskC = new XElement("taskC",
                                    from i in result
                                    group i by i.Category into g
                                    orderby g.Key
                                    select new XElement("category", new XAttribute("name", g.Key),
                                            from i in g
                                            where i.Date <= 2
                                            select new XElement("person", i.LastName)
                                        )
                                );

                            taskC.Save(filePathTaskC);

                            //d
                            var taskD = new XElement("taskD",
                                    from i in result
                                    group i by i.Category into g
                                    select new XElement("category", new XAttribute("name", g.Key),
                                        (from i in g
                                        where i.QuestionPoint == g.Min(i => i.QuestionPoint)
                                        select new XElement("question", i.QuestionName)).FirstOrDefault()
                                    )
                                );

                            taskD.Save(filePathTaskD);
                        }
                    }
                }
            }
        }
    }
}