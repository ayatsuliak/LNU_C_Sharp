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
            string filePathBook = @"D:\C#\Programming\30.05.2023\books.xml";
            string filePathGenge = @"D:\C#\Programming\30.05.2023\genres.xml";
            string filePathPublisher = @"D:\C#\Programming\30.05.2023\publishers.xml";
            string filePatInfo = @"D:\C#\Programming\30.05.2023\infos.xml";
            string filePathTaskA = @"D:\C#\Programming\30.05.2023\forTaskA.xml";
            string filePathTaskB = @"D:\C#\Programming\30.05.2023\forTaskB.xml";
            string filePathTaskC = @"D:\C#\Programming\30.05.2023\forTaskC.xml";
            string filePathTaskD = @"D:\C#\Programming\30.05.2023\forTaskD.xml";

            using (FileStream f1 = new FileStream(filePathBook, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(filePathGenge, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(filePathPublisher, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(filePatInfo, FileMode.Open))
                        {
                            var books = XElement.Load(f1);
                            var genres = XElement.Load(f2);
                            var publishers = XElement.Load(f3);
                            var infos = XElement.Load(f4);

                            //a
                            var result1 = from i in infos.Elements("info")
                                          join b in books.Elements("book") on (uint)i.Element("book_id") equals (uint)b.Element("id")
                                          join g in genres.Elements("genre") on (uint)i.Element("genre_id") equals (uint)g.Element("id")
                                          join p in publishers.Elements("publisher") on (string)i.Element("publisher_id") equals (string)p.Element("id")                                          
                                          select new
                                          {
                                              Autor = (string)b.Element("autor"),
                                              Book = (string)b.Element("name"),
                                              Publisher = (string)p.Element("name"),
                                              Date = (DateTime)i.Element("date"),
                                              Genre = (string)g.Element("name")
                                          };

                            var forTaskA = new XElement("TaskA",
                                    from i in result1
                                    group i by i.Autor into g                                    
                                    orderby g.Key
                                    select new XElement("autor", new XAttribute("name", g.Key),
                                        from i in g
                                        group i by i.Book into b
                                        orderby b.Key
                                        select new XElement("book", new XAttribute("name", b.Key),
                                            from i in b
                                            select new XElement("publisher", i.Publisher)
                                        )
                                      )
                                );

                            forTaskA.Save(filePathTaskA);

                            //b
                            var forTaskB = new XElement("TaskB",
                                    from i in result1   
                                    orderby i.Date descending
                                    group i by i.Autor into g
                                    select new XElement("autor", new XAttribute("name", g.Key),
                                        from i in g                                        
                                        where i.Date == g.Max(i => i.Date)
                                        group i by i.Book into b  
                                        select new XElement("book", new XAttribute("name", b.Key),
                                            from i in b
                                            select new XElement("publisher", i.Publisher)
                                        )
                                      )
                                );
                            forTaskB.Save(filePathTaskB);

                            //c
                            var forTaskC = new XElement("TaskC", 
                                    from i in result1
                                    group i by i.Publisher into g
                                    orderby g.Key
                                    select new XElement("publisher", new XAttribute("name", g.Key),
                                        from i in g
                                        orderby i.Autor
                                        select new XElement("autor", i.Autor)
                                    )
                                );

                            forTaskC.Save(filePathTaskC);

                            //d
                            var forTaskD = new XElement("TaskD",
                                    from i in result1
                                    group i by i.Genre into g
                                    let x = (from j in result1
                                             group j by j.Genre into gr
                                             select gr.Count()).Max()
                                    where g.Count() == x
                                    orderby g.Key
                                    select new XElement("genre", g.Key)
                                );

                            forTaskD.Save(filePathTaskD);
                        }
                    }
                }
            }
        }
    }
}