using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Linq;

/*Розробити засоби для облiку послуг стоматолога.
Пацiєнт характеризується реєстрацiйним номером, прiзвищем, iм’ям, роком народження.
Послуга характеризується iдентифiкацiйним номером, назвою, вартiстю.
Данi про спектр послуг i про пацiєнтiв подано окремими xml-файлами.
Данi про наданi послуги задано xml-файлом за схемою: дата обслуговування, реєстрацiйний номер
пацiєнта, iдентифiкацiйний номер послуги.*/


/*l-файл, де послуги систематизованi за датою обслуговування за схемою <прiзвище пацiєнта, 
 * надана йому у цей день послуга з вказанням назви i вартостi>; прiзвища впорядкувати у
лексико-графiчному порядку, послуги – за спаданням вартостi.*/

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathProcedures = @"D:\C#\Programming\25.04.2023\procedures.xml";
            string pathPatients = @"D:\C#\Programming\25.04.2023\patients.xml";
            string pathServices = @"D:\C#\Programming\25.04.2023\services.xml";
            string pathtaskA = @"D:\C#\Programming\25.04.2023\taskA.xml";
            string pathtaskB = @"D:\C#\Programming\25.04.2023\taskB.xml";
            string pathtaskC = @"D:\C#\Programming\25.04.2023\taskC.xml";

            using (FileStream fs1 = new FileStream(pathProcedures, FileMode.Open))
            {
                
                using (FileStream fs2 = new FileStream(pathPatients, FileMode.Open))
                {
                    using (FileStream fs3 = new FileStream(pathServices, FileMode.Open))
                    {
                        var procedures = XElement.Load(fs1);
                        var patients = XElement.Load(fs2);
                        var services = XElement.Load(fs3);

                        /*(а)xml - файл, де послуги систематизованi за датою обслуговування за схемою<прiзвище пацiєнта, 
                            надана йому у цей день послуга з вказанням назви i вартостi>; прiзвища впорядкувати у
                            лексико - графiчному порядку, послуги – за спаданням вартостi*/

                        var result = from s in services.Elements("Service")
                                     join pt in patients.Elements("Patient") on s.Element("PatientId").Value equals pt.Element("Id").Value
                                     join pr in procedures.Elements("Procedure") on s.Element("ProcedureId").Value equals pr.Element("Id").Value
                                     group new { pt, pr } by (DateTime)s.Element("Date") into g
                                     orderby g.Key
                                     select new
                                     {
                                         Date = g.Key,
                                         Patients = from item in g
                                                    orderby (string)item.pt.Element("Surname"), (string)item.pt.Element("Name") ascending
                                                    select new
                                                    {
                                                        Surname = (string)item.pt.Element("Surname"),
                                                        Name = (string)item.pt.Element("Name"),
                                                        Procedure = (string)item.pr.Element("ProcedureName"),
                                                        Price = (decimal)item.pr.Element("Price")
                                                    }
                                     };

                        var task1 = new XElement("TaskA",
                            from g in result
                            select new XElement("Dates",
                                new XElement("Date", g.Date),
                                from p in g.Patients
                                select new XElement("Patient",
                                    new XElement("Surname", p.Surname),
                                    new XElement("Procedure", $"{p.Procedure}: {p.Price}")
                                )
                            )
                        );

                        task1.Save(pathtaskA);

                        /*(б)xml - файл, де для кожної послуги(за назвою), подана iнформацiя за схемою<кiлькiсть повторень за весь 
                            перiод, сумарна вартiсть>; перелiк впорядкувати за зростанням вартостi*/

                        var result2 = from s in services.Elements("Service")
                                      join pt in patients.Elements("Patient") on s.Element("PatientId").Value equals pt.Element("Id").Value
                                      join pr in procedures.Elements("Procedure") on s.Element("ProcedureId").Value equals pr.Element("Id").Value
                                      group pr by pr.Element("ProcedureName").Value into g
                                      orderby g.Sum(x => double.Parse(x.Element("Price").Value))
                                      select new
                                      {
                                          ProcedureName = g.Key,
                                          Count = g.Count(),
                                          Total = g.Sum(x => double.Parse(x.Element("Price").Value))
                                      };

                        var task2 = new XElement("TaskB",
                            from item in result2
                            select new XElement("Procedure",
                                new XElement("ProcedureName", item.ProcedureName),
                                new XElement("Count", item.Count),
                                new XElement("Total", item.Total)
                            )
                        );

                        task2.Save(pathtaskB);

                        /*(в)xml - файл, в якому вказати назви найпоширенiших послуг для таких вiкових груп
                            1) до 18 рокiв
                            2) 19 - 60 рокiв
                            3) бiльше 61 року.*/

                        
                    }
                }
            }
        }
    }
}