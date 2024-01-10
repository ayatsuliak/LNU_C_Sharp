using System;
using System.Linq;

public static class Kata
{
    public static string OddOrEven(int[] array)
    {
        return array.Sum() % 2 == 0 ? "even" : "odd";
    }

    static void Main(string[] args)
    {
        int[] arr = { 0, 1, 4 };
        Console.WriteLine(OddOrEven(arr));        
    }
}