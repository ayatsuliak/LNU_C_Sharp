using System;

namespace Lessons
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string str = Console.ReadLine();
            int a = int.Parse(str);

            if (a % 2 == 0)
            {
                Console.WriteLine("It's steamy");
            }
            else
            {
                Console.WriteLine("It's not steamy");
            }

            switch (a)
            {
                case 1:
                case 2:
                    Console.WriteLine("1 or 2"); 
                    break;
                case 3:
                    Console.WriteLine("3");
                    break;
                default:
                    Console.WriteLine("don't know");
                    break;
            }

            ConsoleKey consKey = Console.ReadKey().Key;
            switch (consKey) //дає світч дляч всіх клавіш клавіатури
            {
                
                default:
                    break;
            }*/



            int count = 0;
            while (count < 3)
            {
                count++;
                Console.WriteLine(count);
            }



        }
    }
}