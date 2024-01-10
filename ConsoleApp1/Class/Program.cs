using MyExtensions;
using System;

namespace Lessons
{
    class Student
    {
        private string surname;

        public string SurName
        {
            get { return surname; }
            set { surname = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Student(string name, DateTime date)
        {
            this.name = name;
            this.date = date;
        }

        public Student(string name, string surname, DateTime date) : this(name, date)
        {
            this.surname = surname;
        }

        public Student(Student student)
        {
            this.name = student.name;
            this.date = student.date;
            //this.surname = student.surname;
        }

        public void Print()
        {
            Console.WriteLine($"Name: {name} \tSurname: {surname} \tDate: {date}");
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            Student stud1 = new Student("Andriy", new DateTime(2003, 12, 9));
            Student stud2 = new Student("Andriy", "Yatsuliak", new DateTime(2003, 12, 9));
            /*stud1.Print();
            stud2.Print();*/

            stud2.PrintFullName();
        }
    }
}