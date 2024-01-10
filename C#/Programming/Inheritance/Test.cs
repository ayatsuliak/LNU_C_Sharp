using Lessons;
using System;

/*Створити тип Vehicle, який характеризується маркою, потужнiстю
двигуна, кiлькiстю колiс та вагою автомобiля. Також утворити похiднi типи Truck (додатково характеризується вантажопiдйомнiстю), а
також Car i Bus, якi характеризується кiлькiстю мiсць для сидiння, а
Bus – ще й кiлькiстю “стоячих” мiсць. Надати properties для даних, а
також метод для iнформування про характеристики автомобiля.*/


namespace Lesson
{
    class Vehicle
    {
        public string Mark { get; set; };
        public double Power { get; set; };
        public int NumberOfWheels { get; set; }
        public double Weight { get; set; }

        public Vehicle(string m, double p, int n, double w)
        {
            Mark = m;
            Power = p;
            NumberOfWheels = n;
            Weight = w;
        }

        public void Print()
        {
            Console.WriteLine("Mark: " + Mark);            
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var car = new Vehicle("fdd", 5.3, 5, 4.3);
            car.Print();

        }
    }

}