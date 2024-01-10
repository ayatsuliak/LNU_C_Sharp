using System;

namespace Functions
{
    class Functions
    {
        static void Func1()
        {
            string ch = Console.ReadLine();
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++) 
            {
                Console.Write(ch);
            }
        }

        static int Func2(int[] arr, int elem) 
        {
            int index = Array.FindIndex(arr, i => i == elem);

            return index;
        }

        static int[] GetRandom(int length)
        {
            int[] arr = new int[length];    

            Random random = new Random();

            for (int i = 0; i < length; i++) 
            {
                arr[i] = random.Next();
            }

            return arr;
        }

        static void Main(string[] args) 
        {
            //Func1();

            int[] arr = GetRandom(10);
            int elem = int.Parse(Console.ReadLine());

            Console.WriteLine(Func2(arr, elem));
        }
    }

}