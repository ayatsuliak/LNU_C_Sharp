using System;
using System.Xml.Linq;

namespace Observer
{
    class Program
    {
        static void Main(string[] args)
        {
            string teachersPath = @"D:\C#\Sr from programming\16.05.2023\teachers.xml";
            string studentsPath = @"D:\C#\Sr from programming\16.05.2023\students.xml";
            string discipline1Path = @"D:\C#\Sr from programming\16.05.2023\discipline1.xml";
            string discipline2Path = @"D:\C#\Sr from programming\16.05.2023\discipline2.xml";
            string forTaskA = @"D:\C#\Sr from programming\16.05.2023\forTaskA.xml";
            string forTaskB = @"D:\C#\Sr from programming\16.05.2023\forTaskB.xml";
            string forTaskD = @"D:\C#\Sr from programming\16.05.2023\forTaskD.xml";

            using (FileStream f1 = new FileStream(teachersPath, FileMode.Open))
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

                            //xml-файл, де результати систематизованi за схемою <назва дисциплiни, прiзвище та iнiцiали
                            //викладача, назва групи, перелiк результатiв у виглядi пар <прiзвище та iнiцiали студента,кiлькiсть балiв>;
                            //вмiст впорядкувати у лексико-графiчному порядку за назвою дисциплiни,назвою групи i прiзвищем студента;

                            var groupedResults = from discipline in discipline1.Elements()
                                                 let disciplineName = (string)discipline.Element("name")
                                                 let teacherId = (string)discipline.Element("teacher_id")
                                                 let teacher = teachers.Elements("teacher").FirstOrDefault(t => (string)t.Element("teacher_id") == teacherId)
                                                 orderby disciplineName
                                                 select new
                                                 {
                                                     Discipline = (string)discipline.Element("name"),
                                                     Teacher = (string)teacher.Element("last_name") + " " + (string)teacher.Element("first_name").Value.Substring(0, 1),
                                                     Results = from result in discipline.Element("results").Elements("result")
                                                               let studentId = (string)result.Element("student_id")
                                                               let student = students.Elements("student").FirstOrDefault(s => (string)s.Element("student_id") == studentId)
                                                               let studentFullName = $"{student.Element("last_name")} {student.Element("first_name")}"
                                                               orderby (string)student.Element("last_name"), (string)student.Element("first_name")
                                                               select new
                                                               {
                                                                   Student = (string)student.Element("last_name") + " " + (string)student.Element("first_name"),
                                                                   Score = (string)result.Element("score")
                                                               }
                                                 };

                            var sortedResults = from result in groupedResults
                                                orderby result.Discipline, result.Results.First().Student
                                                select result;

                            var taskA = new XElement("results",
                                from result in sortedResults
                                select new XElement("result",
                                    new XElement("discipline", result.Discipline),
                                    new XElement("teacher", result.Teacher),
                                    new XElement("students",
                                        from studentResult in result.Results
                                        select new XElement("student",
                                            new XElement("name", studentResult.Student),
                                            new XElement("score", studentResult.Score)
                                        )
                                    )
                                )
                            );

                            taskA.Save(forTaskA);


                            var groupedResults2 = from student in students.Elements("student")
                                                  let groupId = (string)student.Element("group")
                                                  orderby (string)student.Element("group"), (string)student.Element("last_name"), (string)student.Element("first_name")
                                                  select new
                                                  {
                                                      Group = groupId,
                                                      Student = (string)student.Element("last_name") + " " + (string)student.Element("first_name").Value.Substring(0, 1),
                                                      Results = from discipline in discipline1.Elements("discipline")
                                                                let disciplineName = (string)discipline.Element("name")
                                                                let result = discipline.Element("results").Elements("result").FirstOrDefault(r => (string)r.Element("student_id") == (string)student.Element("student_id"))
                                                                let score = result != null ? (string)result.Element("score") : string.Empty
                                                                select new
                                                                {
                                                                    Discipline = disciplineName,
                                                                    Score = score
                                                                }
                                                  };

                            var sortedResults2 = from result in groupedResults2
                                                 orderby result.Group, result.Student
                                                 select result;

                            /*var taskB = new XElement("results",
                                            from result in sortedResults2
                                            group result by result.Group into g
                                            orderby g.Key
                                            select new XElement("group",
                                                new XElement("name", g.Key),
                                                new XElement("students",
                                                    from studentResult in g
                                                    group studentResult by studentResult.Student into sg
                                                    orderby sg.Key
                                                    select new XElement("student",
                                                        new XElement("name", sg.Key),
                                                        new XElement("disciplines",
                                                            from disciplineResult in sg
                                                            group disciplineResult by sg.Discipline into dg
                                                            orderby dg.Key
                                                            select new XElement("discipline",
                                                                new XElement("name", dg.Key),
                                                                new XElement("score", dg.First().Score)
                                                            )
                                                        )
                                                    )
                                                )
                                            )
                                        );*/

                            var taskB = new XElement("results",
                                                from result in sortedResults2
                                                select new XElement("group",
                                                    new XElement("name", result.Group),
                                                    new XElement("students",
                                                        new XElement("student",
                                                            new XElement("name", result.Student),
                                                            new XElement("disciplines",
                                                                from disciplineResult in result.Results
                                                                select new XElement("discipline",
                                                                    new XElement("name", disciplineResult.Discipline),
                                                                    new XElement("score", disciplineResult.Score)
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                            );


                            taskB.Save(forTaskB);

                            var rankedStudents = from result in sortedResults2
                                                 group result by result.Group into g
                                                 orderby g.Key
                                                 let totalScore = g.Sum(r => int.Parse(r.Results.FirstOrDefault()?.Score ?? "0"))
                                                 select new
                                                 {
                                                     Group = g.Key,
                                                     Students = g.Select(r => new { Student = r.Student, Discipline = r.Results.Select(d => new { d.Discipline, d.Score }) }),
                                                     TotalScore = totalScore
                                                 };


                            var ranking = new XElement("ranking",
                                from groupResult in rankedStudents
                                select new XElement("group",
                                    new XElement("name", groupResult.Group),
                                    new XElement("total_score", groupResult.TotalScore),
                                    from studentResult in groupResult.Students
                                    select new XElement("student",
                                        new XElement("name", studentResult.Student),
                                        from disciplineResult in studentResult.Discipline
                                        select new XElement("discipline",
                                            new XElement("name", disciplineResult.Discipline),
                                            new XElement("score", disciplineResult.Score)
                                        )
                                    )
                                )
                            );

                            ranking.Save("forTaskD.xml");
                        }
                    }
                }
            }
        }
    }
}