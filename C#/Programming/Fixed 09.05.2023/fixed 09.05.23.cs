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

        detector.SpeedingDetected += passengerObserver.HandleViolation;
        detector.SpeedingDetected += freightObserver.HandleViolation;
        detector.SpeedingDetected += carObserver.HandleViolation;
        detector.SpeedingDetected += busObserver.HandleViolation;

        detector.AnalyzeFile(filePath);

        Console.WriteLine("Data analysis completed.");
    }
}

class Detector
{
    public event EventHandler<SpeedingEventArgs> SpeedingDetected;

    public void AnalyzeFile(string filePath)
    {
        var doc = XElement.Load(filePath);

        foreach (var violatorElement in doc.Descendants("violator"))
        {
            if((uint)violatorElement.Element("speed") > 50)
            {
                string date = violatorElement.Element("date").Value;
                string time = violatorElement.Element("time").Value;
                string carNumber = violatorElement.Element("car_number").Value;
                string category = violatorElement.Element("category").Value;
                int speed = int.Parse(violatorElement.Element("speed").Value);
                SpeedingDetected?.Invoke(this, new SpeedingEventArgs(date, time, carNumber, category, speed));
            }
        }
    }
}

class SpeedingEventArgs : EventArgs
{
    public string Date { get; }
    public string Time { get; }
    public string CarNumber { get; }
    public string Category { get; }
    public int Speed { get; }

    public SpeedingEventArgs(string date, string time, string carNumber, string category, int speed)
    {
        Date = date;
        Time = time;
        CarNumber = carNumber;
        Category = category;
        Speed = speed;
    }
}

interface IObserver
{
    bool CanHandleCategory(string category);
    void HandleViolation(object sender, SpeedingEventArgs e);
}

class PassengerTransportObserver : IObserver
{
    public bool CanHandleCategory(string category)
    {
        return category.Equals("car");
    }

    public void HandleViolation(object sender, SpeedingEventArgs e)
    {
        if (CanHandleCategory(e.Category))
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", e.Date),
                new XElement("time", e.Time),
                new XElement("car_number", e.CarNumber),
                new XElement("speed", e.Speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\Fixed 09.05.2023\passengers.xml");
        }
    }
}

class FreightTransportObserver : IObserver
{
    public bool CanHandleCategory(string category)
    {
        return category.Equals("truck");
    }

    public void HandleViolation(object sender, SpeedingEventArgs e)
    {
        if (CanHandleCategory(e.Category))
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", e.Date),
                new XElement("time", e.Time),
                new XElement("car_number", e.CarNumber),
                new XElement("speed", e.Speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\Fixed 09.05.2023\freights.xml");
        }
    }
}

class CarTransportObserver : IObserver
{
    public bool CanHandleCategory(string category)
    {
        return category.Equals("car");
    }

    public void HandleViolation(object sender, SpeedingEventArgs e)
    {
        if (CanHandleCategory(e.Category))
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", e.Date),
                new XElement("time", e.Time),
                new XElement("car_number", e.CarNumber),
                new XElement("speed", e.Speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\Fixed 09.05.2023\cars.xml");
        }
    }
}

class BusTransportObserver : IObserver
{
    public bool CanHandleCategory(string category)
    {
        return category.Equals("bus");
    }

    public void HandleViolation(object sender, SpeedingEventArgs e)
    {
        if (CanHandleCategory(e.Category))
        {
            XElement violatorElement = new XElement("violator",
                new XElement("date", e.Date),
                new XElement("time", e.Time),
                new XElement("car_number", e.CarNumber),
                new XElement("speed", e.Speed)
            );

            var doc = new XElement(violatorElement);
            doc.Save(@"D:\C#\Programming\FIxed 09.05.2023\buses.xml");
        }
    }
}
