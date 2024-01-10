using Lessons;
using System;

namespace Lesson
{
    class Point2D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point2D(int x, int y)
        {
            X = x; 
            Y = y;
        }

        public void Print()
        {
            Console.WriteLine("X: \t" + X);
            Console.WriteLine("Y: \t" + Y);
        }
    }

    class Point3D : Point2D
    {
        public int Z { get; set; }
        public Point3D(int x, int y, int z) : base(x, y)
        {
            Z = z;
        }

        public new void Print()
        {
            base.Print();
            Console.WriteLine("Z: \t" + Z);
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }

}