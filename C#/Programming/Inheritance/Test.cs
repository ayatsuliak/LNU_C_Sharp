using Lessons;
using System;

/*�������� ��� Vehicle, ���� ��������������� ������, ������i���
�������, �i���i��� ���i� �� ����� �������i��. ����� �������� ���i��i ���� Truck (��������� ��������������� ��������i�����i���), �
����� Car i Bus, ��i ��������������� �i���i��� �i��� ��� ���i���, �
Bus � �� � �i���i��� ��������� �i���. ������ properties ��� �����, �
����� ����� ��� i����������� ��� �������������� �������i��.*/


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