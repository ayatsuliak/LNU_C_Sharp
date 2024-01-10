using System;
using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Program
{
    class Program
    {
        public static void ReadTextByStreamReader(string path)
        {
            using (var reader = new StreamReader(path))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        public static void addTextByStreamWriter(string path)
        {
            if (File.Exists(path)) File.Delete(path);
            var fs = new FileStream(path, FileMode.CreateNew);

            using (var writer = new StreamWriter(fs))
            {
                writer.WriteLine("Fuck");
                writer.Write("Fuck you");
            }
        }
        static void Main(string[] args)
        {
            string line;
            var lstVehicles = new List<TwoWheelVehicle>();
            string path1 = @"D:\\C#\\Programming\\linq 11.04.2023\\linq 11.04.2023\1.csv";
            string path2 = @"D:\C#\Programming\linq 11.04.2023\linq 11.04.2023\2.csv";
            string path3 = @"D:\C#\Programming\linq 11.04.2023\linq 11.04.2023\3.csv";

            using (FileStream stream = File.OpenRead(path1))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@path1);
                while ((line = file.ReadLine()) != null)
                {
                    string[] arrtibutes = line.Split(',');
                    lstVehicles.Add(new TwoWheelVehicle(Convert.ToString(arrtibutes[0]),
                    Convert.ToDouble(arrtibutes[1]),
                    Convert.ToUInt32(arrtibutes[2]),
                    Convert.ToUInt32(arrtibutes[3])));
                }
            }

            using (FileStream stream = File.OpenRead(path2))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@path2);
                while ((line = file.ReadLine()) != null)
                {
                    string[] arrtibutes = line.Split(',');
                    lstVehicles.Add(new WithDieselEngine(
                        Convert.ToString(arrtibutes[0]),
                        Convert.ToDouble(arrtibutes[1]),
                        Convert.ToUInt32(arrtibutes[2]),
                        Convert.ToUInt32(arrtibutes[3]),
                        Convert.ToUInt32(arrtibutes[4])));
                }
            }

            using (FileStream stream = File.OpenRead(path3))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@path3);
                while ((line = file.ReadLine()) != null)
                {
                    string[] attributes = line.Split(',');
                    lstVehicles.Add(new WithElectroEngine(
                        Convert.ToString(attributes[0]),
                        Convert.ToDouble(attributes[1]),
                        Convert.ToUInt32(attributes[2]),
                        Convert.ToUInt32(attributes[3]),
                        Convert.ToUInt32(attributes[4])
                        ));
                }
            }


            Console.WriteLine("Task A\nUnsorted: ");
            foreach (var i in lstVehicles) { Console.WriteLine(i); }

            Console.WriteLine("\nSorted: \n");

            var sortedVehicles = from v in lstVehicles
                                 orderby v.Mark
                                 select v;
            foreach (var i in sortedVehicles)
            {
                Console.WriteLine(i);
            }

            //(б)перелiк засобiв з найпотужнiшим двигуном
            Console.WriteLine("\n Task B: \n");

            var maxEnginePower = lstVehicles.Max(p => p.EnginePower);

            var maxPowerVihicles = from v in lstVehicles
                                   where v.EnginePower == maxEnginePower
                                   select v;

            foreach (var i in maxPowerVihicles) { Console.WriteLine(i); }

            //(в) для кожної категорiї перелiк засобiв з найпотужнiшим двигуном
            Console.WriteLine("\n Task C: \n");

            var maxEnginePowerByType = from v in lstVehicles
                                       group v by v.GetType() into g
                                       select new
                                       {
                                           Type = g.Key,
                                           MaxEnginePower = g.Max(v => v.EnginePower)
                                       };

            var resultQueryForEnginePower = from j in maxEnginePowerByType
                                            select new
                                            {
                                                Type = j.Type,
                                                result = from v in lstVehicles
                                                         where j.Type == v.GetType() && v.EnginePower == j.MaxEnginePower
                                                         select v
                                            };

            foreach (var i in resultQueryForEnginePower)
            {
                string type;
                if (i.Type == typeof(TwoWheelVehicle))
                    type = "TwoWheelVehicle";
                else if (i.Type == typeof(WithDieselEngine))
                    type = "WithDieselEngine";
                else
                    type = "WithElectroEngine";

                Console.WriteLine(type);
                foreach (var j in i.result)
                {
                    Console.WriteLine(j);
                }
            }

            //(г) перелiк пропозицiй електро-засобiв для користувача з вiдомою вагою,
            //впорядкований за зростанням максимальної ваги користувача
            Console.WriteLine("\n Task D: \n");

            var weight = int.Parse(Console.ReadLine());

            var electroCar = from v in lstVehicles
                             where v.GetType() == typeof(WithElectroEngine)
                             && weight >= v.MaxWeight
                             orderby v.MaxWeight
                             select (WithElectroEngine)v;            

            foreach (var i in electroCar)
            {
                Console.WriteLine(i);
            }

        }
    }

            
    public class TwoWheelVehicle
    {
        public string Mark { get; set; }
        public double EnginePower { get; set; }
        public uint MaxSpeed { get; set; }
        public uint MaxWeight { get; set; }
        public TwoWheelVehicle(string mark, double ngnpwr, uint whlnmbr, uint wght)
        {
            Mark = mark;
            EnginePower = ngnpwr;
            MaxSpeed = whlnmbr;
            MaxWeight = wght;
        }
        public override string ToString()
        {
            return "Mark: " + Mark + " EnginePower: " + EnginePower + " MaxSpeed: " + MaxSpeed + " Weight: " + MaxWeight;
        }
    }
    public class WithDieselEngine : TwoWheelVehicle
    {
        public uint EngineCapacity { get; set; }
        public WithDieselEngine(string mark, double ngnpwr, uint whlnmbr, uint wght, uint v) : base(mark, ngnpwr, whlnmbr, wght)
        {
            EngineCapacity = v;
        }
        public override string ToString()
        {
            return "Mark: " + Mark + " EnginePower: " + EnginePower + " WheelNumber: " + MaxSpeed + " MaxWeight: " + MaxWeight + " EngineCapacity: " + EngineCapacity;
        }
    }
    public class WithElectroEngine : TwoWheelVehicle
    {
        public uint EngineVolume { get; set; }
        public WithElectroEngine(string mark, double ngnpwr, uint whlnmbr, uint wght, uint ngnvlm) : base(mark, ngnpwr, whlnmbr, wght)
        {
            EngineVolume = ngnvlm;
        }
        public override string ToString()
        {
            return "Mark: " + Mark + " EnginePower: " + EnginePower + " WheelNumber: " + MaxSpeed + " Weight: " + MaxWeight + " EngineVolume: " + EngineVolume;
        }
    }
    public class VehicleComparer<T> : IComparer<T> where T : TwoWheelVehicle
    {
        public int Compare(T x, T y)
        {
            return x.Mark.CompareTo(y.Mark);
        }
    }
}