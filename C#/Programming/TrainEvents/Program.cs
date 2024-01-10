using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Event
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"D:\C#\Programming\TrainEvents\data.xml";
            string filePathPassangers = @"D:\C#\Programming\TrainEvents\passangers.xml";
            string filePathTrucks = @"D:\C#\Programming\TrainEvents\trucks.xml";

            var detector = new Detector();
            var passangers = new Passanger(detector);
            var trucks = new Truck(detector);
            detector.Analize(filePath);

            passangers.WriteData(filePathPassangers);
            trucks.WriteData(filePathTrucks);

            Console.WriteLine("Data analysis completed.");
        }
        class SpeedingEventArgs : EventArgs
        {
            public DateTime Date { get; }
            public string CarNumber { get; }
            public string Category { get; }
            public uint Speed { get; }

            public SpeedingEventArgs(DateTime date, string carNumber, string category, uint speed)
            {
                Date = date;
                CarNumber = carNumber;
                Category = category;
                Speed = speed;
            }
        }
        class Detector
        {
            public event EventHandler<SpeedingEventArgs> SpeedingDetected;
            
            public void Analize(string filePath)
            {
                var doc = XElement.Load(filePath);
                foreach(var i in doc.Descendants("vehicle"))
                {
                    if ((uint)i.Element("speed") > 50)
                    {
                        DateTime date = (DateTime)i.Element("date");
                        string carNumber = (string)i.Element("number");
                        string category = (string)i.Element("category");
                        uint speed = (uint)i.Element("speed");
                        OnSpeedingDetected(new SpeedingEventArgs(date, carNumber, category, speed));
                    }
                }
            }

            protected virtual void OnSpeedingDetected(SpeedingEventArgs e)
            {
                var handler = SpeedingDetected;

                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }
        interface IVehicle
        {
            bool VehicleType(string category);
            void Update(object sender, SpeedingEventArgs e);
            void WriteData(string filePath);
        }

        class Passanger : IVehicle 
        {
            public bool VehicleType(string category)
            {
                return category.Equals("car") || category.Equals("bus");
            }
            public Passanger(Detector detector)
            {
                detector.SpeedingDetected += Update;
            }
            private List<SpeedingEventArgs> speedEvents = new List<SpeedingEventArgs>();

            public void Update(object sender, SpeedingEventArgs e)
            {
                if (VehicleType(e.Category))
                {
                    speedEvents.Add(e);
                }
            }
            public void WriteData(string filePath)
            {
                var result = from i in speedEvents
                             select new
                             {
                                 Time = i.Date,
                                 CarNum = i.CarNumber,
                                 Speed = i.Speed
                             };

                var doc = new XElement("fine_passanger",
                    from item in result
                    select new XElement("fine",
                    new XElement("date", item.Time),
                    new XElement("number", item.CarNum),
                    new XElement("speed", item.Speed)));
                doc.Save(filePath);
            }
        }
        class Truck : IVehicle
        {
            public bool VehicleType(string category)
            {
                return category.Equals("truck");
            }
            private List<SpeedingEventArgs> speedEvents = new List<SpeedingEventArgs>();
            public Truck(Detector detector)
            {
                detector.SpeedingDetected += Update;
            }

            public void Update(object sender, SpeedingEventArgs e)
            {
                if (VehicleType(e.Category))
                {
                    speedEvents.Add(e);
                }
            }
            public void WriteData(string filePath)
            {
                var result = from i in speedEvents
                             select new
                             {
                                 Time = i.Date,
                                 CarNum = i.CarNumber,
                                 Speed = i.Speed
                             };

                var doc = new XElement("fine_passanger",
                    from item in result
                    select new XElement("fine",
                    new XElement("date", item.Time),
                    new XElement("number", item.CarNum),
                    new XElement("speed", item.Speed)));
                doc.Save(filePath);
            }
        }
    }
}