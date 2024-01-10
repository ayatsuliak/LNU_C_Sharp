using System;

namespace Lesson
{
    class Vehicle
    {
        public string Mark { get; set; }
        public double Power { get; set; }
        public uint NumberOfWheels { get; set; }
        public double Weight { get; set; }

        public Vehicle(string m = "", double p = 0.0, uint n = 0, double w = 0.0)
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
        public Truck(string m = "", double p = 0.0, uint n = 0, double w = 0.0, double c = 0.0) : base(m, p, n, w)
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
        public Car(string m = "", double p = 0.0, uint n = 0, double w = 0.0, uint s = 0) : base(m, p, n, w)
        {
            Seating = s;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Seating: {Seating}";
        }
    }

    class Bus : Car
    {
        public uint StandingPlaces { get; set; }
        public Bus(string m = "", double p = 0.0, uint n = 0, double w = 0.0, uint s = 0, uint sp = 0) : base(m, p, n, w, s)
        {
            StandingPlaces = sp;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Standing places: {StandingPlaces}";
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var arr = new Vehicle[] { new Vehicle("Marka1", 5.3, 5, 4.3), new Vehicle("Marka2", 8.1, 4, 3.2),
                new Vehicle("Marka3", 4.4, 6, 3.3), new Car("Volvo", 8.1, 4, 1, 5),
                new Car("BMW", 3.2, 4, 3.2, 5), new Car("Renaut", 2.2, 4, 2.2, 5), new Truck("Scania", 7.3, 6, 6.3, 6.2),
                new Truck("Mercedes", 4.3, 6, 2.3, 7.2), new Bus("Mercedes", 4.5, 4, 5.5, 9, 5), new Bus("Bogdan", 5.5, 4, 7.5, 28, 46) };
            //a
            foreach (var i in arr)
            {
                Console.WriteLine(i.ToString());
            }

            //б
            double CapacitySum = 0.0;
            foreach (var i in arr)
            {
                if (i.GetType() == typeof(Truck))
                {
                    var elem = (Truck)i;
                    CapacitySum += elem.CarryingCapacity;
                }

            }
            Console.WriteLine($"Capacity sum: {CapacitySum}");

            //в
            double maxPower = 0.0;
            foreach (var i in arr)
            {
                if (i.Power > maxPower)
                {
                    maxPower = i.Power;
                }
            }
            Console.Write("Car with max power: ");
            foreach (var i in arr)
            {
                if (i.Power == maxPower)
                {
                    Console.Write($"{i.Mark} | ");
                }
            }
            Console.WriteLine();

            //г
            uint countPassengers1 = 0;
            uint countPassengers2 = 0;
            foreach (var i in arr)
            {
                if (i is Car)
                {
                    var elem = (Car)i;
                    countPassengers1 += elem.Seating;
                }
            }
            foreach (var i in arr)
            {
                if (i.GetType() == typeof(Bus))
                {
                    var elem = (Bus)i;
                    countPassengers2 += elem.StandingPlaces;
                }
            }
            uint countPassengers = countPassengers1 + countPassengers2;
            Console.WriteLine($"Count Passangers:  {countPassengers}");
        }
    }

}