using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace FixedSr
{
    class Program
    {
        public static List<Vehicle> ReadTextByStreamReader(string path)
        {
            var vehicles = new List<Vehicle>();
            var lines = File.ReadAllLines(path).ToList();

            using (FileStream stream = File.OpenRead(path))
            {
                foreach (var i in lines)
                {
                    string[] entries = i.Split(',');

                    string type = entries[0];
                    string mark = entries[1];
                    double power = Convert.ToDouble(entries[2]);
                    uint numOfWheels = Convert.ToUInt32(entries[3]);
                    double weight = Convert.ToDouble(entries[4]);

                    Vehicle vehicle;

                    switch (type)
                    {
                        case nameof(Truck):
                            double carryingCapacity = Convert.ToDouble(entries[5]);
                            vehicle = new Truck(mark, power, numOfWheels, weight, carryingCapacity);
                            break;
                        case nameof(Car):
                            uint seating = Convert.ToUInt32(entries[5]);
                            vehicle = new Car(mark, power, numOfWheels, weight, seating);
                            break;
                        default:
                            vehicle = new Vehicle(mark, power, numOfWheels, weight);
                            break;
                    }
                    vehicles.Add(vehicle);
                }
            }
            return vehicles;
        }
        static void Main(string[] args)
        {
            string filePath = @"D:\C#\Sr 04.04.2023\TextFile1.txt";
            string filePathForTaskA = @"D:\C#\Sr 04.04.2023\taskA.txt";
            string filePathForTaskB = @"D:\C#\Sr 04.04.2023\taskB.txt";
            string filePathForTaskC = @"D:\C#\Sr 04.04.2023\taskC.txt";
            string filePathForTaskD = @"D:\C#\Sr 04.04.2023\taskD.txt";

            var vehicles = ReadTextByStreamReader(filePath);


            //(а) повнi описи усiх транспортних засобiв автопарку кожного типу, впорядкованi за маркою у лексико-графiчному порядку;
            var sortedVehicles = from vehicle in vehicles
                                 orderby vehicle.Mark
                                 select vehicle;

            var listForA = new List<string>();
            foreach(var i in sortedVehicles)
            {
                listForA.Add(i.ToString());
            }

            File.WriteAllLines(filePathForTaskA, listForA);

            /*(б) для кожної марки перелiк усiх транспортних засобiв, 
             * впорядкований за потужнiстю двигуна у спадному порядку,
             * впорядковувати за маркою у лексико-графiчному порядку;*/
            var groupingByMark = from vehicle in sortedVehicles
                                 group vehicle by vehicle.Mark into markGroup
                                 select new
                                 {
                                     Mark = markGroup.Key,
                                     Vehicles = from vehicle in markGroup
                                                orderby vehicle.Power descending
                                                select vehicle
                                 };

            var listForB = new List<string>();
            foreach(var i in groupingByMark)
            {
                listForB.Add(i.Mark);
                foreach (var j in i.Vehicles)
                {
                    listForB.Add(j.ToString());
                }
                
            }

            File.WriteAllLines(filePathForTaskB, listForB);


            //(в)у категорiї Truck знайти авто з найбiльшою потужністю;
            //перелiк впорядкувати за кiлькiстю колiс у зростаючому порядку;
            var truckVehicles = from vehicle in sortedVehicles
                                where vehicle.GetType() == typeof(Truck)
                                select (Truck)vehicle;

            var maxPowerOfTruck = truckVehicles.Max(truck => truck.Power);

            var queryWithMaxPowerTruck = from truck in truckVehicles
                                         where truck.Power == maxPowerOfTruck
                                         orderby truck.NumberOfWheels 
                                         select truck;

            var listForC = new List<string>();
            foreach (var i in queryWithMaxPowerTruck)
            {
                listForC.Add(i.ToString());
            }

            File.WriteAllLines(filePathForTaskC, listForC);


            //(г) у категорiї Car знайти авто з найбiльшою кiлькiстю мiсць; перелiк впорядкувати за вагою у
            //зростаючому порядку
            var carVehicles = from vehicle in sortedVehicles
                                where vehicle.GetType() == typeof(Car)
                                select (Car)vehicle;

            var maxSeatingOfTruck = carVehicles.Max(car => car.Seating);

            var queryWithMaxSeatingCar = from car in carVehicles
                                         where car.Seating == maxSeatingOfTruck
                                         orderby car.Weight
                                         select car;

            var listForD = new List<string>();
            foreach (var i in queryWithMaxSeatingCar)
            {
                listForD.Add(i.ToString());
            }

            File.WriteAllLines(filePathForTaskD, listForD);
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
    }
}