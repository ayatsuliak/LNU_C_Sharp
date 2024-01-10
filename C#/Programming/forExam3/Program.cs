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
            string filePathTest = @"D:\C#\Programming\forExam3\tests.xml";
            string filePathServer = @"D:\C#\Programming\forExam3\servers.xml";
            string filePathPlatform = @"D:\C#\Programming\forExam3\platforms.xml";
            string filePatInfo = @"D:\C#\Programming\forExam3\infos.xml";
            string filePathTaskA = @"D:\C#\Programming\forExam3\forTaskA.xml";
            string filePathTaskB = @"D:\C#\Programming\forExam3\forTaskB.xml";
            string filePathTaskC = @"D:\C#\Programming\forExam3\forTaskC.xml";
            string filePathTaskD = @"D:\C#\Programming\forExam3\forTaskD.xml";

            using (FileStream f1 = new FileStream(filePathTest, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(filePathServer, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(filePathPlatform, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(filePatInfo, FileMode.Open))
                        {
                            var tests = XElement.Load(f1);
                            var servers = XElement.Load(f2);
                            var platforms = XElement.Load(f3);
                            var infos = XElement.Load(f4);

                            var result = from i in infos.Elements("info")
                                         join t in tests.Elements("test") on (uint)i.Element("test_id") equals (uint)t.Element("id")
                                         join s in servers.Elements("server") on (uint)i.Element("server_id") equals (uint)s.Element("id")
                                         join p in platforms.Elements("platform") on (uint)s.Element("platform_id") equals (uint)p.Element("id")
                                         select new
                                         {
                                             TestId = (uint)t.Element("id"),
                                             User = (string)t.Element("user"),
                                             Date = (DateTime)i.Element("date"),
                                             Result = (string)i.Element("result"),
                                             Platform = (string)p.Element("name"),
                                             Size = (int)t.Element("size"),
                                             Lanq = (string)t.Element("lanq")
                                         };

                            //a
                            var taskA = new XElement("taskA",
                                    from i in result
                                    group i by new { i.TestId, i.User, i.Date, i.Result } into g
                                    orderby g.Key.User
                                    select new XElement("results", new XElement("id", g.Key.TestId), new XElement("user", g.Key.User),
                                        new XElement("date", g.Key.Date), new XElement("result", g.Key.Result)
                                    )
                                );

                            taskA.Save(filePathTaskA);

                            //b
                            var taskB = new XElement("taskB",
                                    from i in result
                                    group i by new { i.TestId, i.User, i.Date, i.Result, i.Platform } into g
                                    orderby g.Key.Platform
                                    select new XElement("results", new XElement("id", g.Key.TestId), new XElement("user", g.Key.User),
                                        new XElement("date", g.Key.Date), new XElement("result", g.Key.Result), new XElement("platform", g.Key.Platform)
                                    )
                                );
                            taskB.Save(filePathTaskB);

                            //c
                            var taskC = new XElement("taskC",
                                    from i in result
                                    group i by i.Platform into g
                                    orderby g.Key
                                    select new XElement("platform", new XAttribute("name", g.Key),
                                        new XElement("total_size", g.Sum(i => i.Size))
                                    )
                                );
                            taskC.Save(filePathTaskC);

                            //d
                            var taskD = new XElement("taskD",
                                    from i in result
                                    group i by i.User into g
                                    orderby g.Key
                                    select new XElement("user", new XAttribute("name", g.Key),
                                        from i in g
                                        group i by i.Lanq into gr
                                        select new XElement("lanq", new XAttribute("name", gr.Key),
                                             new XElement("procent", $"{((float)g.Count(i => i.Result == "true") / (float)g.Count()) * 100} %")
                                            )
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