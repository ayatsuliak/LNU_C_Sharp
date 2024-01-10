using System;

/*Тип Computer, який характеризується швидкодiєю процесора i обсягами оперативної пам’ятi та диску. 
 
Також утворити похiднi типи Server, який має додатковий диск, а також WorkStation i Notebook, 

якi додатково характеризується маркою та розмiром дiагоналi монiтора, а Notebook – ще й вагою. 

Кожен тип комп’ютера може повернути його повний опис.*/

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
    class HardWare
    {
        public string Mark { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public double P
        {
            get { return 0.02; }
            set { P = value; }
        }
        public HardWare(string m, DateTime d, double pr)
        {
            Mark = m;
            Date = d;
            if ((DateTime.Now.Subtract(d)).Days >= 182)
                Price = pr * P;
            else
                Price = pr;
        }
        public override string ToString()
        {
            return $"Mark: {Mark} | Date: {Date} | Price: {Price}";
        }
    }
    class Computer : HardWare
    {
        public float RAM { get; set; }
        public float DiskVolume { get; set; }
        public Computer(string m, DateTime d, double pr, float ram, float dv) : base(m, d, pr)
        {
            RAM = ram;
            DiskVolume = dv;
        }
        public override string ToString()
        {
            return base.ToString() + $" RAM: {RAM} | Disk volume: {DiskVolume}";
        }
    }
    class Server : Computer
    {
        public float AdditionalDiskValue { get; set; }
        public Server(string m, DateTime d, double pr, float ram, float dv, float adv) : base(m, d, pr, ram, dv)
        {
            AdditionalDiskValue = adv;
        }
        public override string ToString()
        {
            return base.ToString() + $" Additional disk value: {AdditionalDiskValue}";
        }
    }
    class WorkStation : Computer
    {
        public string Model { get; set; }
        public float Diagonal { get; set; }
        public WorkStation(string m, DateTime d, double pr, float ram, float dv, string mdl, float dgnl) : base(m, d, pr, ram, dv)
        {
            Model = mdl;
            Diagonal = dgnl;
        }
        public override string ToString()
        {
            return base.ToString() + $" Model: {Model} | Diagonal: {Diagonal}";
        }
    }

    class Notebook : WorkStation
    {
        public float Weight { get; set; }
        public Notebook(string m, DateTime d, double pr, float ram, float dv, string mdl, float dgnl, float w) : base(m, d, pr, ram, dv, mdl, dgnl)
        {
            Weight = w;
        }
        public override string ToString()
        {
            return base.ToString() + $" Weight: {Weight}";
        }
    }
}