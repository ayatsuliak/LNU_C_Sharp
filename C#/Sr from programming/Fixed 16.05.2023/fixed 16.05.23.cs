using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Fixed16052023
{
    class Program
    {
        static void Main(string[] args)
        {
            string teachersPath = @"D:\C#\Sr from programming\16.05.2023\teachers.xml";
            string studentsPath = @"D:\C#\Sr from programming\16.05.2023\students.xml";
            string discipline1Path = @"D:\C#\Sr from programming\16.05.2023\discipline1.xml";
            string discipline2Path = @"D:\C#\Sr from programming\16.05.2023\discipline2.xml";
            string forTaskA = @"D:\C#\Sr from programming\Fixed 16.05.2023\forTaskA.xml";
            string forTaskB = @"D:\C#\Sr from programming\Fixed 16.05.2023\forTaskB.xml";
            string forTaskC = @"D:\C#\Sr from programming\Fixed 16.05.2023\forTaskC.xml";
            string forTaskD = @"D:\C#\Sr from programming\Fixed 16.05.2023\forTaskD.xml";

            using(FileStream f1 = new FileStream(teachersPath, FileMode.Open))
            {
                using (FileStream f2 = new FileStream(studentsPath, FileMode.Open))
                {
                    using (FileStream f3 = new FileStream(discipline1Path, FileMode.Open))
                    {
                        using (FileStream f4 = new FileStream(discipline2Path, FileMode.Open))
                        {
                            var students = XElement.Load(f2);
                            var teachers = XElement.Load(f1);
                            var discipline1 = XElement.Load(f3); 
                            var discipline2 = XElement.Load(f4);
                            var disciplines = new XElement("disciplines",
                                discipline1,
                                discipline2
                                );

                            //Console.WriteLine(disciplines);

                            /*(а)xml - файл, де результати систематизованi за схемою < назва дисциплiни, прiзвище та iнiцiали викладача,
                                назва групи, перелiк результатiв у виглядi пар<прiзвище та iнiцiали студента, кiлькiсть балiв>; 
                            вмiст впорядкувати у лексико-графiчному порядку за назвою дисциплiни, назвою групи i прiзвищем студента;*/

                            var result1 = from discipline in disciplines.Elements("discipline")
                                          from result in discipline.Elements("result")
                                          join student in students.Elements("student") on (uint)result.Element("student_id") equals (uint)student.Element("student_id")
                                          join teacher in teachers.Elements("teacher") on (uint)discipline.Element("teacher_id") equals (uint)teacher.Element("teacher_id")
                                          orderby (string)discipline.Element("name"), (string)student.Element("group"), (string)student.Element("last_name")
                                          select new
                                          {
                                              Discipline = discipline.Element("name").Value,
                                              Teacher = (string)teacher.Element("last_name") + " " + teacher.Element("first_name").Value.Substring(0, 1),
                                              Student = (string)student.Element("last_name") + " " + student.Element("first_name").Value.Substring(0, 1),
                                              Group = (string)student.Element("group"),
                                              Score = (uint)result.Element("score")
                                          };

                            var task1 = new XElement("TaskA",
                                from i in result1
                                group i by i.Discipline into g
                                orderby g.Key
                                select new XElement("discipline",
                                    new XAttribute("name", g.Key),
                                    from teacherGroup in g
                                    group teacherGroup by teacherGroup.Teacher into t
                                    select new XElement("teacher",
                                        new XAttribute("name", t.Key),
                                        from studentGroup in t
                                        group studentGroup by studentGroup.Group into sg
                                        orderby sg.Key
                                        select new XElement("group",
                                            new XAttribute("name", sg.Key),
                                            from r in sg
                                            select new XElement("student",
                                                new XAttribute("name", r.Student),
                                                new XElement("score", r.Score)
                                            )
                                        )
                                    )
                                )
                            );

                            task1.Save(forTaskA);

                            /*xml - файл, де результати систематизованi за схемою < назва групи, перелiк результатiв у виглядi 
                                < прiзвище та iнiцiали студента > та пари < назва дисциплiни, кiлькiсть балiв>; вмiст впорядкувати у 
                                лексико - графiчному порядку за назвою групи i прiзвищем студента;*/
                            var result2 = from discipline in disciplines.Elements("discipline")
                                          from result in discipline.Elements("result")
                                          join student in students.Elements("student") on (uint)result.Element("student_id") equals (uint)student.Element("student_id")
                                          select new
                                          {
                                              Student = (string)student.Element("last_name") + " " + student.Element("first_name").Value.Substring(0, 1),
                                              Group = (string)student.Element("group"),
                                              Score = (uint)result.Element("score"),
                                              Discipline = discipline.Element("name").Value
                                          };

                            var task2 = new XElement("taskB",
                                    from i in result2
                                    group i by i.Group into g
                                    orderby g.Key
                                    select new XElement("group", new XAttribute("name", g.Key),
                                        from j in g
                                        group j by j.Student into sg
                                        orderby sg.Key
                                        select new XElement("student", new XAttribute("name", sg.Key),
                                            from k in sg
                                            select new XElement("discipline", new XAttribute("name", k.Discipline), new XElement("score", k.Score))
                                        )
                                    )
                                );

                            task2.Save(forTaskB);

                            // xml - файл, описаний у попередньому завданнi, але без врахування студентiв з незадовiльними балами(меншими 51)

                            var task3 = new XElement("taskC",
                                    from i in result2
                                    group i by i.Group into g
                                    orderby g.Key
                                    select new XElement("group", new XAttribute("name", g.Key),
                                        from j in g
                                        group j by j.Student into sg
                                        orderby sg.Key
                                        select new XElement("student", new XAttribute("name", sg.Key),
                                            from k in sg
                                            where k.Score >= 51
                                            select new XElement("discipline", new XAttribute("name", k.Discipline), new XElement("score", k.Score))
                                        )
                                    )
                                );

                            task3.Save(forTaskC);

                            /*xml - файл, в якому подано рейтинг студентiв за сумарною кiлькiстю балiв з усiх дисциплiн без
                                врахування студентiв з незадовiльними балами*/

                            var task4 = new XElement("taskD",
                                from j in result2
                                group j by j.Student into sg
                                where sg.All(k => k.Score >= 51)
                                orderby sg.Key
                                select new XElement("student",
                                    new XAttribute("name", sg.Key),
                                    new XElement("scores", sg.Sum(k => k.Score))
                                )
                            );
                            task4.Save(forTaskD);
                        }
                    }
                }
            }
        }
    }
}