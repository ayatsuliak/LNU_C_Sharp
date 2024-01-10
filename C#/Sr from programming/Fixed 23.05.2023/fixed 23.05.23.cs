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
            string filePathWorker = @"D:\C#\Sr from programming\ConsoleApp11\workers.xml";
            string filePathPosition = @"D:\C#\Sr from programming\ConsoleApp11\positions.xml";
            string filePathProject = @"D:\C#\Sr from programming\ConsoleApp11\projects.xml";
            string filePathReport = @"D:\C#\Sr from programming\ConsoleApp11\report.xml";
            string filePathTaskA = @"D:\C#\Sr from programming\Fixed 23.05.2023\forTaskA.xml";
            string filePathTaskB = @"D:\C#\Sr from programming\Fixed 23.05.2023\forTaskB.xml";
            string filePathTaskC = @"D:\C#\Sr from programming\Fixed 23.05.2023\forTaskC.xml";
            string filePathTaskD = @"D:\C#\Sr from programming\Fixed 23.05.2023\forTaskD.xml";

            using (FileStream f1 = new FileStream(filePathWorker, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(filePathPosition, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(filePathProject, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(filePathReport, FileMode.Open))
                        {
                            var workers = XElement.Load(f1);
                            var positionts = XElement.Load(f2);
                            var projects = XElement.Load(f3);
                            var reports = XElement.Load(f4);

                            //a
                            var result1 = from r in reports.Elements("report")
                                          join p in projects.Elements("project") on (string)r.Element("project_id") equals (string)p.Element("id")
                                          join w in workers.Elements("worker") on (uint)r.Element("worker_id") equals (uint)w.Element("id")
                                          join pos in positionts.Elements("position") on (uint)w.Element("position") equals (uint)pos.Element("id")
                                          select new
                                          {
                                              ProjectId = (string)p.Element("id"),
                                              ProjectName = (string)p.Element("name"),
                                              WorkerName = (string)w.Element("surname") + " " + (string)w.Element("name").Value.Substring(0, 1),
                                              Hours = (uint)r.Element("hours"),
                                              Seller = (uint)pos.Element("seller"),
                                              Position = (string)pos.Element("name")
                                          };

                            var forTaskA = new XElement("forTaskA",
                                    from i in result1
                                    group i by i.ProjectName into p
                                    orderby p.Key
                                    select new XElement("project", new XAttribute("name", p.Key),
                                        from j in p
                                        group j by j.WorkerName into w
                                        orderby w.Key
                                        select new XElement("worker", new XAttribute("name", w.Key),
                                            new XElement("hours", w.Sum(k => k.Hours)  )                                      
                                        )
                                    )
                                );

                            forTaskA.Save(filePathTaskA);

                            //b
                            var forTaskB = new XElement("forTaskB",
                                    from i in result1
                                    group i by i.ProjectName into p
                                    orderby p.Key
                                    select new XElement("project", new XAttribute("name", p.Key),
                                        from j in p
                                        group j by j.WorkerName into w
                                        orderby w.Key
                                        select new XElement("worker", new XAttribute("name", w.Key),
                                            new XElement("hours", w.Sum(k => k.Hours)),
                                            new XElement("total_seller", w.Select(k => k.Seller).First() * (uint)w.Sum(k => k.Hours))
                                        )
                                    )
                                );

                            forTaskB.Save(filePathTaskB);

                            //c
                            var forTaskC = new XElement("forTaskC",
                                    from i in result1
                                    group i by i.ProjectId into p
                                    orderby p.Key
                                    select new XElement("project", new XAttribute("id", p.Key),
                                        from j in p
                                        group j by j.Position into w
                                        orderby w.Key
                                        select new XElement("position", new XAttribute("name", w.Key),
                                           new XElement("hours", w.Sum(k => k.Hours))
                                        )
                                    )
                                );

                            forTaskC.Save(filePathTaskC);

                            //d
                            var forTaskD = new XElement("forTaskD",
                                from i in result1
                                group i by i.ProjectName into p
                                orderby p.Key
                                select new XElement("project", new XAttribute("name", p.Key),
                                    (from j in p
                                     orderby p.Sum(k => k.Hours) descending
                                     select new XElement("hours", p.Sum(k => k.Hours))).FirstOrDefault(),
                                    new XElement("total_seller", p.Sum(k => k.Seller * k.Hours))
                                )
                            );
                            forTaskD.Save(filePathTaskD);
                        }
                    }
                }
            }
        }
    }
}