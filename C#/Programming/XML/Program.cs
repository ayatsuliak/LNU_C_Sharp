using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Linq;

namespace Program
{
    class Program
    {
        static void xmlTreeLinq(Stream xmlstr)
        {
            var xmlTree = new XElement("Root",
                new XElement("Child",
                    new XElement("GrandChild1", 1),
                    new XElement("GrandChild2", 2)
                ),
                new XElement("Child",
                    new XElement("GrandChild3", 3),
                    new XElement("GrandChild4", 4)
                ),
                new XElement("Child",
                    new XElement("GrandChild5", 5),
                    new XElement("GrandChild6", 6)
                )
            );
            xmlTree.Save(xmlstr);

            var grandChilds = from el in xmlTree.Elements("Child")
                              select el;

            foreach (var i in grandChilds)
            {
                Console.WriteLine(i);
            }
        }

        static XElement xmlTreeLinq2(Stream xmlstr)
        {
            var xmlTree = new XElement("People",
                new XElement("person", new XAttribute("firstName", "john"), new XAttribute("lastName", "doe"),
                    new XElement("contactdetails",
                        new XElement("emailadress", "joch@unknown.com"))
                ),
                new XElement("person", new XAttribute("firstName", "jane"), new XAttribute("lastName", "doe"),
                    new XElement("contactdetails",
                        new XElement("emailadress", "jane@unknown.com"),
                        new XElement("phonenumber", 0491243242)
                            )
                )
            );
            xmlTree.Save(xmlstr);

            return xmlTree;
        }
        static void xmlTreeConstLinq(Stream xmlstr)
        {
            var srcTree = new XElement("Root",
                new XElement("Element", 1),
                new XElement("Element", 2),
                new XElement("Element", 3)
            );
            var xse1 = new XStreamingElement("Root1",
                        from el in srcTree.Elements()
                        where (int)el > 2
                        select el
            );
            var xse2 = new XElement("Root2",
            from el in srcTree.Elements()
            where (int)el > 2
            select el
            );
            srcTree.Add(new XElement("Element", 4));

            Console.WriteLine(xse1);

            Console.WriteLine(xse2);


            var xmlTree = new XElement("nr", xse1,
            new XElement("Child", 1),
            new XElement("Child"),
            new XElement("Child", 2)
            );
            srcTree.Add(new XElement("Element", 5));
            Console.WriteLine(xmlTree);
        }
        static void xmlLinqStream(Stream xmlstr)
        {
            var people = XElement.Load(xmlstr);
            var personNames = from p in people.Elements()
                              select (string)p.Attribute("firstName") + " " + (string)p.Attribute("lastName");

            foreach (var personName in personNames)
            {
                Console.WriteLine(personName);
            }
        }

        static void xmlLoadStream(Stream xmlstr)
        {
            var tree = XElement.Load(xmlstr);
            //Console.WriteLine(tree);

            var query = from i in tree.DescendantsAndSelf().Ancestors("Child") //це вузли, які вкладені в "Child"
                        select i.Name;
            foreach (var i in query)
            {
                Console.WriteLine(i);
            }
        }
        static void Main(string[] args)
        {
            string path = @"D:\C#\Programming\XML\people.xml";
            string path2 = @"D:\C#\Programming\XML\people2.xml";
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                xmlLoadStream(fs);
                /*var query = from i in xmlTree.Elements()
                            select i;
                foreach (var i in query)
                {
                    Console.WriteLine(i);
                }*/
            }

            /*XElement xmlTree = new XElement("Root",
                new XElement("Child1",
                    new XElement("GrandChild1",
                        new XElement("GreatGrandChild1", "content")
                    )
                ),
                new XElement("Child2",
                    new XElement("GrandChild2",
                        new XElement("GreatGrandChild2", "content")
                    )
                )
            );
            IEnumerable<XElement> greatGrandChildren =
                from el in xmlTree.Descendants() //всі вкладені нащадки

                where el.Name.LocalName.StartsWith("Great")
                select el;

            *//*Console.WriteLine("Great Grand Children Elements");
            Console.WriteLine("----");
            foreach (XElement de in greatGrandChildren)
                Console.WriteLine(de.Name);*//*

            IEnumerable<XElement> allAncestors =
                from el in greatGrandChildren.Ancestors().Distinct()
                select el;

            Console.WriteLine("");
            Console.WriteLine("Ancestors");
            Console.WriteLine("----");
            foreach (XElement de in allAncestors)
                Console.WriteLine(de.Name);*/
        }
    }
}