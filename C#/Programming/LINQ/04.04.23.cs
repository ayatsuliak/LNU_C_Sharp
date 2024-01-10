using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            // Запис об'єктів у файли
            Vehicle vehicle1 = new Vehicle("Tesla", 456.7, 4, 1900.5);
            WriteVehicleToFile(vehicle1, "vehicle1.txt");

            Vehicle newVehicle = new Vehicle("BMW", 300, 4, 1700);
            WriteVehicleToFile(newVehicle, "vehicle2.txt");

            Truck truck1 = new Truck("MAN", 800.0, 6, 12000.0, 4500.0);
            WriteVehicleToFile(truck1, "truck1.txt");

            Car car1 = new Car("Honda", 200.0, 4, 1200.0, 5);
            WriteVehicleToFile(car1, "car1.txt");

            Truck newTruck = new Truck("Volvo", 400.0, 6, 3000.0, 5000.0);
            WriteVehicleToFile(newTruck, "truck2.txt");

            Car newCar = new Car("Toyota", 150.0, 4, 1000.0, 5);
            WriteVehicleToFile(newCar, "car2.txt");

            // Читання об'єктів з файлив
            Vehicle vehicle2 = ReadVehicleFromFile("vehicle1.txt");

            Vehicle newVehicle2 = ReadVehicleFromFile("vehicle2.txt");

            Truck truck2 = (Truck)ReadVehicleFromFile("truck1.txt");

            Car car2 = (Car)ReadVehicleFromFile("car1.txt");

            Truck newTruck2 = (Truck)ReadVehicleFromFile("truck2.txt");

            Car newCar2 = (Car)ReadVehicleFromFile("car2.txt");


            var arr = new Vehicle[] { vehicle2, truck2, car2, newVehicle2, newTruck2, newCar2 };

            Console.WriteLine("\t Task а");
            //a
            var sortedA = from i in arr
                          orderby i.Mark
                          select i;

            foreach (var i in sortedA)
            {
                Console.WriteLine(i.ToString());
            }

            Console.WriteLine("\t Task в");



            //в

            double capacity = 0;
            foreach (var i in arr)
            {
                if (i.GetType() == typeof(Truck))
                {
                    var elem = (Truck)i;
                    if (elem.CarryingCapacity > capacity)
                    {
                        capacity = elem.CarryingCapacity;
                    }
                }
            }
            var maxCapacity = from i in arr
                              where typeof(i) == Truck
                              and 


            

            /*var arrTruck = new Truck[] { };
            foreach (var i in arr)
            {
                if (i.GetType() == typeof(Truck))
                {
                    var elem = (Truck)i;
                    if (elem.CarryingCapacity == capacity)
                    {
                        arrTruck.Append(elem);
                    }
                }
            }
            Array.Sort(arrTruck, new VehicleComparer<Vehicle>());
            foreach (var i in arrTruck)
            {
                i.ToString();
            }*/


            Console.WriteLine("\t Task г");
            //г
            uint seating = 0;
            foreach (var i in arr)
            {
                if (i.GetType() == typeof(Car))
                {
                    var elem = (Car)i;
                    if (elem.Seating > seating)
                    {
                        seating = elem.Seating;
                    }
                }
            }

            var arrCar = new Car[] { };
            foreach (var i in arr)
            {
                if (i.GetType() == typeof(Car))
                {
                    var elem = (Car)i;
                    if (elem.Seating == seating)
                    {
                        arrCar.Append(elem);
                    }
                }
            }
            Array.Sort(arrCar, new VehicleComparer<Vehicle>());
            foreach (var i in arrCar)
            {
                i.ToString();
            }
        }
        public class Vehicle
        {
            public string Mark { get; set; }
            public double Power { get; set; }
            public uint NumberOfWheels { get; set; }
            public double Weight { get; set; }

            public Vehicle(string m, double p, uint n, double w)
            {
                Mark = m;
                Power = p;
                NumberOfWheels = n;
                Weight = w;
            }

            public override string ToString()
            {
                return $"Makr: {Mark} | Power: {Power} | Number of wheels: {NumberOfWheels} | Weight: {Weight}";
            }
        }
        public class VehicleComparer<T> : IComparer<T> where T : Vehicle
        {
            public int Compare(T first, T second)
            {
                return first.Mark.CompareTo(second.Mark);
            }
        }

        class Truck : Vehicle
        {
            public double CarryingCapacity { get; set; }
            public Truck(string m, double p, uint n, double w, double c) : base(m, p, n, w)
            {
                CarryingCapacity = c;
            }

            public override string ToString()
            {
                return base.ToString() + $" | Carrying capacity: {CarryingCapacity}";
            }
        }

        class Car : Vehicle
        {
            public uint Seating { get; set; }
            public Car(string m, double p, uint n, double w, uint s) : base(m, p, n, w)
            {
                Seating = s;
            }

            public override string ToString()
            {
                return base.ToString() + $" | Seating: {Seating}";
            }
        }

        static void WriteVehicleToFile(Vehicle vehicle, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(vehicle.GetType().Name);
                writer.WriteLine(vehicle.Mark);
                writer.WriteLine(vehicle.Power);
                writer.WriteLine(vehicle.NumberOfWheels);
                writer.WriteLine(vehicle.Weight);

                if (vehicle is Truck truck)
                {
                    writer.WriteLine(truck.CarryingCapacity);
                }
                else if (vehicle is Car car)
                {
                    writer.WriteLine(car.Seating);
                }
            }
        }

        static Vehicle ReadVehicleFromFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                string type = reader.ReadLine();
                string mark = reader.ReadLine();
                double power = double.Parse(reader.ReadLine());
                uint numOfWheels = uint.Parse(reader.ReadLine());
                double weight = double.Parse(reader.ReadLine());

                Vehicle vehicle;

                switch (type)
                {
                    case nameof(Truck):
                        double carryingCapacity = double.Parse(reader.ReadLine());
                        vehicle = new Truck(mark, power, numOfWheels, weight, carryingCapacity);
                        break;
                    case nameof(Car):
                        uint seating = uint.Parse(reader.ReadLine());
                        vehicle = new Car(mark, power, numOfWheels, weight, seating);
                        break;
                    default:
                        vehicle = new Vehicle(mark, power, numOfWheels, weight);
                        break;
                }
                return vehicle;
            }
        }
    }

}