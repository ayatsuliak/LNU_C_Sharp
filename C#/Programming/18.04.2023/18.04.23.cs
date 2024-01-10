using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Linq;

/*Розробити засоби для облiку продаж комп’ютерної технiки рiзних типiв.
Тип комп’ютера характеризуються числовим iдентифiкатором, маркою (компанiя - виробник), а також цiною.
Додатково на комп’ютери може бути встановлена операцiйна система. Вона характеризується числовим iдентифiкатором,
назвою та цiною. Якщо система не встановлена, то формально комп’ютеру
приписують операцiйну систему, для якої числовий iдентифiкатор рiвний нулевi i цiна є нульовою.
Товарний чек мiстить дату, iдентифiкатор типу комп’ютера, iдентифiкатор операцiйної системи та
кiлькiсть проданих комп’ютерiв цього типу.
Товарнi чеки задано у кiлькох csv-файлах. Також в окремих файлах подано iнформацiю про типи
комп’ютерiв та операцiйних систем*/

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPathOperSys = @"D:\C#\Programming\18.04.2023\operSys.txt";
            string rootPathComputer = @"D:\C#\Programming\18.04.2023\computer.txt";
            string rootPathReceipt = @"D:\C#\Programming\18.04.2023\receipt.txt";
            string rootPathReceipt2 = @"D:\C#\Programming\18.04.2023\receipt2.txt";

            var computers = Data.readComputers(rootPathComputer);
            var operationSystems = Data.readOS(rootPathOperSys);
            var rec1 = Data.readReceipts(rootPathReceipt);
            var rec2 = Data.readReceipts(rootPathReceipt2);
            var receipts = rec1.Concat(rec2).ToList();

            /*foreach (var i in computers)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("--------------------------------");
            foreach (var i in operationSystems)
            {
                Console.WriteLine(i.ToString());
            }
            Console.WriteLine("--------------------------------");
            foreach (var i in receipts)
            {
                Console.WriteLine(i);
            }*/



            Console.WriteLine("-------------------------------Task a-------------------------------");
            //(а) сумарну вартiсть проданої за весь час комп’ютерної технiки;

            var computersWithNewPrice = from computer in computers
                                        join operSys in operationSystems on
                                        computer.OperationSystemId equals operSys.Id
                                        join receipt in receipts on
                                        computer.Id equals receipt.ComputerId
                                        select new
                                        {
                                            totalSum = (computer.Price + operSys.Price) * receipt.Count
                                        };

            var total = computersWithNewPrice.Sum(s => s.totalSum);
            Console.WriteLine($"Total price: {total}");


            Console.WriteLine("-------------------------------Task b-------------------------------");
            //(б)кiлькiсть проданих комп’ютерiв кожної марки;

            var computerCompanyNameQuery = from computer in computers
                                           group computer by computer.CompanyName into cN
                                           orderby cN.Key
                                           select new
                                           {
                                               Mark = cN.Key,
                                               Count = cN.Count()
                                           };

            foreach (var i in computerCompanyNameQuery)
            {
                Console.WriteLine($"{i.Mark}: Count: {i.Count}");
            }


            Console.WriteLine("-------------------------------Task c-------------------------------");
            //(в) вартiсть комп’ютерiв, проданих кожного дня;

            var soldСomputers = from computer in computers
                                join operSys in operationSystems on
                                computer.OperationSystemId equals operSys.Id
                                join receipt in receipts on
                                computer.Id equals receipt.ComputerId
                                group new { receipt, computer, operSys } by receipt.Date into d
                                select new
                                {
                                    Date = d.Key,
                                    totalPrice = d.Sum(sum => (sum.computer.Price + sum.operSys.Price) * sum.receipt.Count)
                                };

            foreach (var i in soldСomputers)
            {
                Console.WriteLine($"{i.Date}: Total sum: {i.totalPrice}");
            }


            Console.WriteLine("-------------------------------Task d-------------------------------");
            //(г) для кожної операцiйної системи сумарну вартiсть проданих комп’ютерiв з цiєю системою.

            var OSSum = from computer in computers
                        join operSys in operationSystems on
                        computer.OperationSystemId equals operSys.Id
                        join receipt in receipts on
                        computer.Id equals receipt.ComputerId
                        group new { receipt, computer, operSys } by operSys.Name into d
                        select new
                        {
                            Name = d.Key,
                            totalPrice = d.Sum(sum => (sum.computer.Price + sum.operSys.Price) * sum.receipt.Count)
                        };

            foreach (var i in OSSum)
            {
                Console.WriteLine($"{i.Name}: Total sum: {i.totalPrice}");
            }
        }

        class Computer
        {
            public uint Id { get; set; }
            public string CompanyName { get; set; }
            public double Price { get; set; }
            public uint OperationSystemId { get; set; }
            public Computer(uint id, string cN, double p, uint oSI)
            {
                Id = id;
                CompanyName = cN;
                Price = p;
                OperationSystemId = oSI;
            }

            public override string ToString()
            {
                return $"Computer Id: {Id} | Company name: {CompanyName} | Price: {Price} | Operation system: {OperationSystemId}";
            }
        }

        class OS
        {
            public uint Id { get; }
            public string Name { get; }
            public double Price { get; }
            public OS(uint id, string n, double p)
            {
                Id = id;
                Name = n;
                Price = p;
            }
            public override string ToString()
            {
                return $"OS Id: {Id} | OS name: {Name} | Price: {Price}";
            }
        }

        class Receipt
        {
            public DateTime Date { get; set; }
            public uint ComputerId { get; set; }
            public uint OperationSystemId { get; set; }
            public uint Count { get; set; }
            public Receipt(DateTime d, uint cI, uint oSI, uint count)
            {
                Date = d;
                ComputerId = cI;
                OperationSystemId = oSI;
                Count = count;
            }
            public override string ToString()
            {
                return $"Date: {Date} | Computer Id: {ComputerId} | OS Id: {OperationSystemId} | Count: {Count}";
            }
        }
        class Data
        {
            public static List<Receipt> readReceipts(string path)
            {
                List<Receipt> rec = new List<Receipt>();

                using (StreamReader file = new StreamReader(path))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] entries = line.Split(',');

                        DateTime date = Convert.ToDateTime(entries[0]);
                        uint computerId = Convert.ToUInt32(entries[1]);
                        uint operSysId = Convert.ToUInt32(entries[2]);
                        uint count = Convert.ToUInt32(entries[3]);

                        Receipt r = new Receipt(date, computerId, operSysId, count);
                        rec.Add(r);
                    }
                }
                return rec;
            }
            public static List<Computer> readComputers(string path)
            {
                List<Computer> comp = new List<Computer>();

                using (StreamReader file = new StreamReader(path))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] entries = line.Split(',');

                        uint id = Convert.ToUInt32(entries[0]);
                        string companyName = entries[1];
                        double price = Convert.ToDouble(entries[2]);
                        uint operatingSystem = Convert.ToUInt32(entries[3]);

                        Computer c = new Computer(id, companyName, price, operatingSystem);
                        comp.Add(c);
                    }
                }
                return comp;
            }
            public static List<OS> readOS(string path)
            {
                List<OS> os = new List<OS>();

                using (StreamReader file = new StreamReader(path))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] entries = line.Split(',');

                        uint id = Convert.ToUInt32(entries[0]);
                        string name = entries[1];
                        double price = Convert.ToDouble(entries[2]);

                        OS o = new OS(id, name, price);

                        os.Add(o);
                    }
                }
                return os;
            }
        }
    }
}