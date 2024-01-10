using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LINQ2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePathClients = @"D:\C#\Programming\TrainXML 29.05\clients.xml";
            string filePathCoaches = @"D:\C#\Programming\TrainXML 29.05\coaches.xml";
            string filePathGroups = @"D:\C#\Programming\TrainXML 29.05\groups.xml";
            string filePathInfos = @"D:\C#\Programming\TrainXML 29.05\infos.xml";
            /*string filePathTaskA = @"D:\C#\Programming\TrainXML 29.05\forTaskA.xml";
            string filePathTaskB = @"D:\C#\Programming\TrainXML 29.05\forTaskB.xml";
            string filePathTaskC = @"D:\C#\Programming\TrainXML 29.05\forTaskC.xml";
            string filePathTaskD = @"D:\C#\Programming\TrainXML 29.05\forTaskD.xml";*/

            using (FileStream f1 = new FileStream(filePathClients, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(filePathCoaches, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(filePathGroups, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(filePathInfos, FileMode.Open))
                        {
                            var clients = XElement.Load(f1);
                            var coaches = XElement.Load(f2);
                            var groups = XElement.Load(f3);
                            var infos = XElement.Load(f4);

                            //a
                            var result1 = from f in infos.Elements("info")
                                          join cl in clients.Elements("client") on (uint)f.Element("client_id") equals (uint)cl.Element("id")
                                          join g in clients.Elements("client") on (uint)f.Element("group_id") equals (uint)g.Element("id")
                                          join ch in coaches.Elements("coach") on (uint)g.Element("coach_id") equals (uint)ch.Element("id")
                                          select new
                                          {
                                              Group = (string)g.Element("name"),
                                              Client = (string)cl.Element("surname") + " " + cl.Element("name").Value.Substring(0, 1)
                                          };

                            var forTaskA = new XElement("TaskA",
                                    from i in result1
                                    group i by i.Group into gr
                                    orderby gr.Key
                                    select new XElement("group", new XAttribute("name", gr.Key),
                                        from i in gr
                                        select new XElement("client", i.Client)
                                    )
                                );
                            Console.WriteLine(forTaskA);
                            //forTaskA.Save(filePathTaskA);
                        }
                    }
                }
            }
        }
    }
}