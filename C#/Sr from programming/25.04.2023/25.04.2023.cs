using System;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

/*    а) xml-файл, де результати систематизованi для кожної групи за схемою<назва студента, перелiк його результатiв за 
 *    порядком номерiв завдання з вказанням назви 
        теми i зарахованих балiв>; результати впорядкувати за назвою групи i прiзвищем у лексикографiчному порядку(без повторень).*/


    class Program
    {
        static void Main(string[] args)
        {
            string pathTasks = @"D:\C#\Sr from programming\25.04.2023\tasks.xml";
            string pathStudents = @"D:\C#\Sr from programming\25.04.2023\students.xml";
            string pathResults = @"D:\C#\Sr from programming\25.04.2023\results.xml";
            string pathForA = @"D:\C#\Sr from programming\25.04.2023\taskA.xml";

            var tasks = XElement.Load(pathTasks);
            var students = XElement.Load(pathStudents);
            var results = XElement.Load(pathResults);

            var forTaskA = from i in results.Elements()
                           join p in tasks.Elements() on i.Element("TaskNumber") equals p.Element("Number")
                           join j in students.Elements() on i.Element("StudId") equals j.Element("StudId")
                           select new
                           {
                               Group = j.Element("Group").Value,
                               Name = j.Element("Name").Value,
                               Result = i.Element("Points").Value,

                            };
        }      

    }
}