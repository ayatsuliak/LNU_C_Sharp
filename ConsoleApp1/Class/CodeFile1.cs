using Lessons;
using System;

namespace MyExtensions
{
    static class MyExtensions
    {
        public static void PrintFullName(this Student student)
        {
            Console.WriteLine(student.SurName);
        }
    }

}
