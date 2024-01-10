using System;
using System.IO;

class Program
{
    static void Main()
    {
        string input = File.ReadAllText(@"C:\Users\компас\Downloads\Test\input.txt");
        string output = input.ToUpper();
        File.WriteAllText(@"C:\Users\компас\Downloads\Test\output.txt", output);
    }
}