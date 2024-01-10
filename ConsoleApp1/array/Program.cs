using System;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.Write("Enter array size: ");
            int size = int.Parse(Console.ReadLine());

            int[] arr = new int[size];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = int.Parse(Console.ReadLine());
            }

            //1
            Console.Write("First task: ");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();

            //2
            /*Console.Write("Second task: ");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[arr.Length - (i+1)] + " ");
            }
            Console.WriteLine();*/
            Console.Write("Second task: ");
            for (int i = arr.Length - 1; i >= 0 ; i--)
            {
                Console.Write(arr[i]);
            }
            Console.WriteLine();

            //3
            Console.Write("Third task: ");
            int sum = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 == 0)
                {
                    sum += arr[i];
                    Console.Write(arr[i] + " ");
                }
            }
            Console.Write("Sum: " + sum);
            Console.WriteLine();

            //4
            Console.Write("Fourth task: ");
            //Console.Write(arr.Min());
            int a = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < a)
                {
                    a = arr[i];
                }
                
            }
            Console.Write(a);
        }
    }
}