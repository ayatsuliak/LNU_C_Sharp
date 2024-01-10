using System;

namespace Functions
{
    class Functions
    {
        static void Method1<T>(ref T[] a, int i = 0)
        {
            if (i < a.Length)
            {
                Console.WriteLine(a[i]);
            }
            else
            {
                return;
            }
            i++;
            Method1(ref a, i);
        }

        static int Sum(ref int[] a, int i = 0)
        {
            if (i >= a.Length) 
            {
                return 0;
            }

            return a[i] + Sum(ref a, i + 1);
        }

        static int Method3(int value)
        {
            if(value < 10)
            {
                return value;
            }

            int digit = value % 10;

            int nextVal = value / 10;

            return digit + Method3(nextVal);
        }

        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3 };
            Method1(ref arr);
            Console.WriteLine();

            Console.WriteLine(Sum(ref arr));

            Console.WriteLine();
            Console.WriteLine(Method3(561));

        }
    }

}