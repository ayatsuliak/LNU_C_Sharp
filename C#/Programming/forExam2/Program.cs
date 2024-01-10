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
            string filePathIs = @"D:\C#\Programming\forExam2\is.xml";
            string filePathCategory = @"D:\C#\Programming\forExam2\categories.xml";
            string filePathAgensy = @"D:\C#\Programming\forExam2\agensies.xml";
            string filePatInfo = @"D:\C#\Programming\forExam2\infos.xml";
            string filePathTaskA = @"D:\C#\Programming\forExam2\forTaskA.xml";
            string filePathTaskB = @"D:\C#\Programming\forExam2\forTaskB.xml";
            string filePathTaskC = @"D:\C#\Programming\forExam2\forTaskC.xml";
            string filePathTaskD = @"D:\C#\Programming\forExam2\forTaskD.xml";

            using (FileStream f1 = new FileStream(filePathIs, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(filePathCategory, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(filePathAgensy, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(filePatInfo, FileMode.Open))
                        {
                            var iss = XElement.Load(f1);
                            var categories = XElement.Load(f2);
                            var agensies = XElement.Load(f3);
                            var infos = XElement.Load(f4);

                            var result = from inf in infos.Elements("info")
                                         join c in categories.Elements("category") on (uint)inf.Element("category_id") equals (uint)c.Element("id")
                                         join a in agensies.Elements("agensy") on (string)inf.Element("agensy_id") equals (string)a.Element("id")
                                         join i in iss.Elements("is") on (uint)inf.Element("is_id") equals (uint)i.Element("id")
                                         select new
                                         {
                                             ISName = (string)inf.Element("name"),
                                             Date = (DateTime)i.Element("date"),
                                             AgensyName = (string)a.Element("name"),
                                             Local = (string)c.Element("location"),
                                             Capacity = (uint)i.Element("capacity")
                                         };

                            //a
                            var taskA = new XElement("taskA",
                                    from i in result
                                    group i by new { i.ISName, i.Date, i.AgensyName } into g
                                    orderby g.Key.Date descending
                                    select new XElement("main", new XAttribute("name", g.Key.ISName),
                                        new XElement("date", g.Key.Date), new XElement("agensy_name", g.Key.AgensyName)
                                    )
                                );
                            taskA.Save(filePathTaskA);

                            //b
                            var taskB = new XElement("taskB",
                                    from i in result
                                    group i by i.Local into gr
                                    select new XElement("local", new XAttribute("name", gr.Key),
                                        from i in gr
                                        group i by new { i.ISName, i.Date, i.AgensyName } into g
                                        orderby g.Key.Date descending
                                        select new XElement("main", new XAttribute("name", g.Key.ISName),
                                            new XElement("date", g.Key.Date), new XElement("agensy_name", g.Key.AgensyName)
                                        )
                                    )
                                );
                            taskB.Save(filePathTaskB);

                            //c
                            var local_name = "local";
                            var taskC = new XElement("taskC",
                                        from i in result
                                        where i.Local == local_name
                                        group i by i.Local into gr
                                        select new XElement("local", new XAttribute("name", gr.Key),
                                            from i in gr
                                            group i by i.Date into g
                                            select new XElement("date", new XAttribute("date", g.Key),
                                                from i in g
                                                select new XElement("name", i.ISName)
                                            )                                            
                                        ));
                            taskC.Save(filePathTaskC);

                            //d
                            var taskD = new XElement("taskD",
                                    from i in result
                                    group i by i.Date into g
                                    select new XElement("date", new XAttribute("date", g.Key),
                                        from i in g
                                        where i.Capacity == g.Max(i => i.Capacity)
                                        select new XElement("name", i.AgensyName)
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