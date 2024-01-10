using System;

namespace Lessons
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ex1:");
                double ex1 = double.Parse(Console.ReadLine());
                Console.WriteLine("ch:");
                char ch = char.Parse(Console.ReadLine());
                Console.WriteLine("ex2:");
                double ex2 = double.Parse(Console.ReadLine());

                double res;

                switch (ch)
                {
                    case '+':
                        res = ex1 + ex2;
                        Console.WriteLine(res);
                        break;
                    case '-':
                        res = ex1 - ex2;
                        Console.WriteLine(res);
                        break;
                    case '*':
                        res = ex1 * ex2;
                        Console.WriteLine(res);
                        break;
                    case '/':
                        res = ex1 / ex2;
                        Console.WriteLine(res);
                        break;
                }
                Console.ReadLine();
            }
            
        }
    }
}

