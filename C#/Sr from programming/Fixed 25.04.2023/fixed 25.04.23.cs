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

namespace Collection
{
    /*Розробити засоби для пiдведення пiдсумкiв виконання завдань самостiйної роботи з програмування
    на окремому курсi ф-ту.
    Завдання характеризується номером, назвою теми i датою, до якої потрiбно подати звiт про виконання(формат<день, мiсяць>)
    Студент характеризується номером студквитка, прiзвищем та iм’ям, назвою групи.
    Результат виконання завдання фiксують за схемою: номер завдання, номер студквитка, отриманi
    бали за повноту виконання завдання, дата поданого звiту про виконання завдання.
    Якщо звiт вчасно не подано, то зараховується 50% вiд отриманих балiв.
    Усi данi задано окремими xml-файлами.*/


    class Program
    {
        static void Main(string[] args)
        {
            string pathTasks = @"D:\C#\Sr from programming\Fixed 25.04.2023\tasks.xml";
            string pathStudents = @"D:\C#\Sr from programming\Fixed 25.04.2023\students.xml";
            string pathResults = @"D:\C#\Sr from programming\Fixed 25.04.2023\results.xml";
            string pathForA = @"D:\C#\Sr from programming\Fixed 25.04.2023\taskA.xml";
            string pathForB = @"D:\C#\Sr from programming\Fixed 25.04.2023\taskB.xml";
            string pathForC = @"D:\C#\Sr from programming\Fixed 25.04.2023\taskC.xml";

            using (FileStream f1 = new FileStream(pathTasks, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(pathStudents, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(pathResults, FileMode.Open))
                    {
                        var tasks = XElement.Load(f1);
                        var students = XElement.Load(f2);
                        var results = XElement.Load(f3);

                        /*(а)xml - файл, де результати систематизованi для кожної групи за схемою<прiзвище з iнiцiалом
                        iменi студента, перелiк його результатiв за порядком номерiв завдання з вказанням назви теми i зарахованих балiв>; 
                        результати впорядкувати за назвою групи i прiзвищем у лексико-графiчному порядку(без повторень).*/

                        var groupedResults = from result in results.Elements("Result")
                                             join task in tasks.Elements("Task") on (int)result.Element("TaskNumber") equals (int)task.Element("Number")
                                             join student in students.Elements("Student") on (int)result.Element("StudId") equals (int)student.Element("StuId")
                                             orderby (string)student.Element("Group"), (string)student.Element("Surname")
                                             select new
                                             {
                                                 Group = (string)student.Element("Group"),
                                                 FullName = (string)student.Element("Surname") + " " + (string)student.Element("Name").Value.Substring(0, 1),
                                                 TaskNumber = (int)result.Element("TaskNumber"),
                                                 Topic = (string)task.Element("Name"),
                                                 Points = (int)result.Element("Points")
                                             };

                        var task1 = new XElement("Groups",
                            from result in groupedResults
                            group result by result.Group into g
                            orderby g.Key
                            select new XElement("Group",
                                new XElement("Name", g.Key),
                                from studentResult in g
                                group studentResult by studentResult.FullName into sg
                                orderby sg.Key
                                select new XElement("Student",
                                    new XElement("FullName", sg.Key),
                                    from r in sg
                                    select new XElement("Result",
                                        new XElement("TaskNumber", r.TaskNumber),
                                        new XElement("Topic", r.Topic),
                                        new XElement("Points", r.Points)
                                    )
                                )
                            )
                        );

                        task1.Save(pathForA);

                        /*(б)xml - файл, в якому для кожної групи подати результати за схемою < назва теми, рейтинговий
                            список студентiв з сумарними результатами за усi завдання з цiєї теми(прiзвище та iнiцiал,
                            кiлькiсть балiв);*/

                        var groupedResults2 = from result in results.Elements("Result")
                                             join task in tasks.Elements("Task") on (int)result.Element("TaskNumber") equals (int)task.Element("Number")
                                             join student in students.Elements("Student") on (int)result.Element("StudId") equals (int)student.Element("StuId")
                                             orderby (string)task.Element("Name"), (string)student.Element("Surname")
                                             group new
                                             {
                                                 Surname = (string)student.Element("Surname") + " " + student.Element("Name").Value.Substring(0, 1),
                                                 Points = (int)result.Element("Points"),
                                             } by new
                                             {
                                                 Group = (string)student.Element("Group"),
                                                 Task = (string)task.Element("Name")
                                             } into g
                                             select new XElement("Group",
                                                 new XAttribute("Name", g.Key.Group),
                                                 new XElement("Task",
                                                     new XAttribute("Name", g.Key.Task),
                                                     from r in g
                                                     select new XElement("Student",
                                                         new XElement("Surname", r.Surname),
                                                         new XElement("Points", r.Points)
                                                     )
                                                 )
                                             );
                        var task2 = new XElement("Results", groupedResults2);

                        task2.Save(pathForB);

                        /*(в)Отримати xml-файл за схемою<назва групи, список студентiв з сумарною кiлькiстю зарахованих 
                            балiв (прiзвище з iнiцiалом iменi, сумарна кiлькiсть зарахованих балiв); такий список
                            подати у лексико - графiчному порядку(за прiзвищем, без повторень).*/

                        var groupedResults3 = from student in students.Elements("Student")
                                             join result in results.Elements("Result") on (int)student.Element("StuId") equals (int)result.Element("StudId")
                                             join task in tasks.Elements("Task") on (int)result.Element("TaskNumber") equals (int)task.Element("Number")
                                             group new
                                             {
                                                 Surname = (string)student.Element("Surname") + " " + student.Element("Name").Value.Substring(0, 1),
                                                 Points = (int)result.Element("Points")
                                             } by new
                                             {
                                                 Group = (string)student.Element("Group")
                                             } into g
                                             orderby g.Key.Group
                                             select new XElement("Group",
                                                 new XAttribute("Name", g.Key.Group),
                                                 from r in g
                                                 group r by r.Surname into s
                                                 orderby s.Key
                                                 select new XElement("Student",
                                                     new XElement("Surname", s.Key),
                                                     new XElement("Points", s.Sum(r => r.Points))
                                                  )
                                             );

                        var task3 = new XElement("Results", groupedResults3);

                        task3.Save(pathForC);
                    }
                }
            }

                

            /*а) xml - файл, де результати систематизованi для кожної групи за схемою<назва студента, перелiк його результатiв за
             *порядком номерiв завдання з вказанням назви теми i зарахованих балiв >; результати впорядкувати за назвою
             *групи i прiзвищем у лексико-графiчному порядку(без повторень)*/

            
            /*foreach (var i in withNewPoints)
            {
                Console.WriteLine(i);
                //Console.WriteLine(i.Result.Value);   
            }*/
        }

    }
}