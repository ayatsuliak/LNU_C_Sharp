using System;
using System.Collections.Generic;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        string filePath = @"D:\C#\Programming\09.05.2023\violators.xml";

        var detector = new Detector();

        var passengerObserver = new PassengerTransportObserver();
        var freightObserver = new FreightTransportObserver();
        var carObserver = new CarTransportObserver();
        var busObserver = new BusTransportObserver();

        detector.RegisterObserver(passengerObserver);
        detector.RegisterObserver(freightObserver);
        detector.RegisterObserver(carObserver);
        detector.RegisterObserver(busObserver);

        detector.AnalyzeFile(filePath);

        Console.WriteLine("Data analysis completed.");

    }
    class Detector
    {
        private List<IObserver> observers = new List<IObserver>();
        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }
        public void AnalyzeFile(string filePath)
        {
            var doc = XElement.Load(filePath);

            foreach (var violatorElement in doc.Descendants("violator"))
            {
                string date = violatorElement.Element("date").Value;
                string time = violatorElement.Element("time").Value;
                string carNumber = violatorElement.Element("car_number").Value;
                string category = violatorElement.Element("category").Value;
                int speed = int.Parse(violatorElement.Element("speed").Value);

                foreach (IObserver observer in observers)
                {
                    if (observer.CanHandleCategory(category))
                    {
                        observer.HandleViolation(date, time, carNumber, speed);
                    }
                }
            }
        }
    }
    interface IObserver
    {
        bool CanHandleCategory(string category);
        void HandleViolation(string date, string time, string carNumber, int speed);
    }
    class PassengerTransportObserver : IObserver
    {
        public bool CanHandleCategory(string category)
        {
            return category.Equals("car");
        }

        public void HandleViolation(string date, string time, string carNumber, int speed)
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", date),
                new XElement("time", time),
                new XElement("car_number", carNumber),
                new XElement("speed", speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\09.05.2023\passengers.xml");
        }
    }
    class FreightTransportObserver : IObserver
    {
        public bool CanHandleCategory(string category)
        {
            return category.Equals("truck");
        }

        public void HandleViolation(string date, string time, string carNumber, int speed)
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", date),
                new XElement("time", time),
                new XElement("car_number", carNumber),
                new XElement("speed", speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\09.05.2023\freights.xml");
        }
    }
    class CarTransportObserver : IObserver
    {
        public bool CanHandleCategory(string category)
        {
            return category.Equals("car");
        }

        public void HandleViolation(string date, string time, string carNumber, int speed)
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", date),
                new XElement("time", time),
                new XElement("car_number", carNumber),
                new XElement("speed", speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\09.05.2023\passengers.xml");
        }
    }
    class BusTransportObserver : IObserver
    {
        public bool CanHandleCategory(string category)
        {
            return category.Equals("bus");
        }

        public void HandleViolation(string date, string time, string carNumber, int speed)
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", date),
                new XElement("time", time),
                new XElement("car_number", carNumber),
                new XElement("speed", speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\09.05.2023\buses.xml");
        }
    }
}