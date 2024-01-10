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
            string filePathWorker = @"C:\C#\ConsoleApp1\ConsoleApp1\workers.xml";
            string filePathPosition = @"C:\C#\ConsoleApp1\ConsoleApp1\positions.xml";
            string filePathProject = @"C:\C#\ConsoleApp1\ConsoleApp1\projects.xml";
            string filePathReport = @"C:\C#\ConsoleApp1\ConsoleApp1\report.xml";
            string filePathTaskA = @"C:\C#\ConsoleApp1\ConsoleApp1\forTaskA.xml";
            string filePathTaskB = @"C:\C#\ConsoleApp1\ConsoleApp1\forTaskB.xml";
            string filePathTaskC = @"C:\C#\ConsoleApp1\ConsoleApp1\forTaskC.xml";
            string filePathTaskD = @"C:\C#\ConsoleApp1\ConsoleApp1\forTaskD.xml";

            using (FileStream f1 = new FileStream(filePathWorker, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(filePathPosition, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(filePathProject, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(filePathReport, FileMode.Open))
                        {
                            var workers = XElement.Load(f1);
                            var positions = XElement.Load(f2);
                            var projects = XElement.Load(f3);
                            var reports = XElement.Load(f4);
                            
                            //(а) xml-файл, де звiти систематизованi за схемою<назва проєкту, перелiк прiзвищ (з iнiцiалами) працiвникiв разом iз
                            //сумарною кiлькiстю годин,вiдпрацьованих кожним з них>;
                            var result1 = from r in reports.Elements("report")
                                          join w in workers.Elements("worker") on (uint)r.Element("worker_id") equals (uint)w.Element("id")
                                          join p in projects.Elements("project") on (string)r.Element("project_id") equals (string)p.Element("id")
                                          join ps in positions.Elements("position") on (uint)w.Element("position") equals (uint)ps.Element("id")
                                          select new
                                          {
                                              NameProject = (string)p.Element("name"),
                                              Surname = (string)w.Element("surname") + " " + (string)w.Element("name").Value.Substring(0, 1),
                                              Hours =(uint)r.Element("hours"),
                                              Seller = (uint)r.Element("hours") * (uint)ps.Element("seller")
                                          };

                            var taskA = new XElement("TaskA",
                                    from i in result1
                                    group i by i.NameProject into g
                                    select new XElement("project", new XAttribute("project", g.Key),
                                        from p in g
                                        group p by p.Surname into sg
                                        select new XElement("surname", new XAttribute("surname", sg.Key),
                                            from j in sg
                                            select new XElement("hours", sg.Sum(k => k.Hours))))
                                );

                            taskA.Save(filePathTaskA);

                            //(б) xml-файл, описаний у попередньому завданнi, але подати крiм вiдпрацьованих годин ще й зароблену суму грошей;
                            var taskB = new XElement("TaskB",
                                    from i in result1
                                    group i by i.NameProject into g
                                    select new XElement("project", new XAttribute("project", g.Key),
                                        from p in g
                                        group p by p.Surname into sg
                                        select new XElement("surname", new XAttribute("surname", sg.Key),
                                            from j in sg
                                            select new XElement("hours", sg.Sum(k => k.Hours)),
                                            from j in sg
                                            select new XElement("totalSeller", j.Seller)))
                                );

                            taskB.Save(filePathTaskB);

                            //(в) xml-файл, в якому для кожного проєкту(заданого iдентифiкатором) вказати сумарний час ро-боти над ним працiвниками вiдповiдних посад;
                            var result2 = from r in reports.Elements("report")
                                          join w in workers.Elements("worker") on (uint)r.Element("worker_id") equals (uint)w.Element("id")
                                          join p in projects.Elements("project") on (string)r.Element("project_id") equals (string)p.Element("id")
                                          join ps in positions.Elements("position") on (uint)w.Element("position") equals (uint)ps.Element("id")
                                          select new
                                          {
                                              NameProject = (string)p.Element("id"),
                                              Hours = (uint)r.Element("hours"),
                                              Seller = (uint)r.Element("hours") * (uint)ps.Element("seller")
                                          };
                            var taskC = new XElement("TaskC",
                                    from i in result2
                                    group i by i.NameProject into g
                                    select new XElement("project", new XAttribute("project", g.Key),
                                        new XElement("hours", g.Sum(k => k.Hours)))
                                );

                            taskC.Save(filePathTaskC);

                            //(г) xml-файл, де для кожного проєкту(заданого iдентифiкатором) вказати сумарний час
                            //роботинад ним i освоєну суму грошей; цi результати впорядкувати за сумарним часом у спадномупорядку.
                            var taskD = new XElement("TaskC",
                                    from i in result2
                                    group i by i.NameProject into g
                                    orderby g.Sum(k => k.Hours)
                                    select new XElement("project", new XAttribute("project", g.Key),
                                        new XElement("hours", g.Sum(k => k.Hours)),
                                        from i in g
                                        select new XElement("totalSeller", g.Sum(k => k.Seller)))
                                );

                            taskD.Save(filePathTaskD);
                        }
                    }
                }
            }
        }
    }
}