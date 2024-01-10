using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Mine
{
    class Program
    {
        public static void Main()
        {
            using (FileStream f1 = new FileStream("Tasks.xml", FileMode.Open))
            {
                using (FileStream f2 = new FileStream("Students.xml", FileMode.Open))
                {
                    using (FileStream f3 = new FileStream("Results.xml", FileMode.Open))
                    {
                        var xmlTasks = XElement.Load(f1);
                        var xmlStudents = XElement.Load(f2);
                        var xmlResults = XElement.Load(f3);


                        if (File.Exists("Task1.txt")) File.Delete("Task1.txt");
                        // create the file

                        var res1 = new FileStream("Task1.txt", FileMode.CreateNew);

                        using (var writer = new StreamWriter(res1))
                        {
                            //var task1 = new XElement("Root",
                            //                from student in xmlStudents.Elements()
                            //                join res in xmlResults.Elements() on student.Element("Student_id").Value equals res.Element("Student_id").Value
                            //                join task in xmlTasks.Elements() on res.Element("Task_id").Value equals task.Element("Task_id").Value
                            //                orderby student.Element("Group").Value
                            //                group student by (string)student.Element("Group").Value into gr
                            //                select new XElement("Group",
                            //                    new XAttribute("ID", gr.Key),
                            //                    from g in gr
                            //                    select new XElement("Data",
                            //                        g.Element("LastName"),
                            //                        g.Element("FirstName"),
                            //                        g.Element("Points"),
                            //                        g.Element("Theme")
                            //                    )
                            //                )       
                            //            );


                            var task1 = new XElement("Root",
                                            from student in xmlStudents.Elements()
                                            join res in xmlResults.Elements() on student.Element("Student_id").Value equals res.Element("Student_id").Value
                                            join task in xmlTasks.Elements() on res.Element("Task_id").Value equals task.Element("Task_id").Value
                                            orderby student.Element("Group").Value
                                            group new
                                            {
                                                LastName = student.Element("LastName").Value,
                                                FirstInitial = student.Element("FirstName").Value.Substring(0, 1),
                                                Points = res.Element("Points").Value,
                                                Theme = task.Element("Theme").Value
                                            } by (string)student.Element("Group").Value into gr
                                            select new XElement("Gr",
                                                   new XAttribute("Number", gr.Key),
                                                   from g in gr

                                                   select new XElement("Student",
                                                       new XElement("LastName", g.LastName),
                                                       new XElement("FirstNameLetter", g.FirstInitial),
                                                       new XElement("Points", g.Points),
                                                       new XElement("Theme", g.Theme)
                                       )
                                                       )
                                        );

                            // Console.WriteLine(task1);


                            writer.Write(task1);


                            //select new
                            //{
                            //    gr = student.Element("Group").Value,
                            //    sur = student.Element("LastName").Value,
                            //    n = student.Element("FirstName").Value[0],
                            //    t = res.Element("Points").Value,
                            //    th = task.Element("Theme").Value


                            //};
                            //group student by (string)student.Element("Group").Value into newGroup
                            //orderby newGroup.Key
                            //select newGroup;
                            //select student.Element("Group");



                            //from p_s in xmlPatientsServices.Elements()
                            //join p in xmlPatients.Elements() on p_s.Element("patientID").Value equals p.Element("patientID").Value
                            //join s in xmlServices.Elements() on p_s.Element("serviceID").Value equals s.Element("serviceID").Value
                            //orderby p.Element("Name").Value, s.Element("price").Value descending
                            //select new
                            //{
                            //    Patient = p.Element("Name").Value,
                            //    Date = (DateTime?)p_s.Element("service_date"),


                            //foreach (var i in task1)
                            //{
                            //    Console.WriteLine(i);
                            //}


                        }
                    }
                }
            }
        }
    }
}