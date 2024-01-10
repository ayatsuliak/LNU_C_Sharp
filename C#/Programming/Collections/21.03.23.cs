using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Collections;

/*Розробити типи для облiку видачi пацiєнтам лiкiв в медзакладi вiдповiдно до призначень лiкаря.
 * 
Пацiєнт характеризується iменем i прiзвищем та реєстрацiйним номером. Призначення характеризується 

реєстрацiйним номером пацiєнта, датою, назвою лiкiв, дозою в мг i кiлькiстю прийомiв у день.

Iнформацiя про пацiєнтiв подана окремим масивом. Призначення також подано кiлькома окремими масивами.

(а)для кожного пацiєнта(за iменем i прiзвищем) повний перелiк(без повторень) отриманих лiкiв iз вказанням 
    сумарної кiлькостi кожного препарату в мг;

(б)для кожного дня розхiд виданих препаратiв у мг;

(в)для кожного препарату загальний розхiд за усi днi.*/

namespace Collection
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var patients = new Patient[]
            {
                new Patient ("Іван", "Франко", 1),
                new Patient ("Тарас", "Шевченко", 2),
                new Patient ("Леся", "Українка", 3)
            };

            var prescriptions = new Appointment[]
            {
                new Appointment (1, new DateTime(2023, 3, 21), "Аспiрин", 500, 2),
                new Appointment (1, new DateTime(2023, 3, 22), "Кодепрон", 200, 1),
                new Appointment (2, new DateTime(2023, 3, 21), "Амоксиклав", 875, 2),
                new Appointment (2, new DateTime(2023, 3, 22), "Парацетамол", 500, 3),
                new Appointment (3, new DateTime(2023, 3, 21), "Аспiрин", 1000, 2),
                new Appointment (3, new DateTime(2023, 3, 22), "Кодепрон", 400, 1)
            };

            //(а)для кожного пацiєнта(за iменем i прiзвищем) повний перелiк(без повторень) отриманих лiкiв iз вказанням
            //сумарної кiлькостi кожного препарату в мг;      

            Dictionary<uint, Dictionary<string, float>> dict_for_a = new Dictionary<uint, Dictionary<string, float>>();

            foreach(var i in patients)
            {
                dict_for_a.TryAdd(i.Id, new Dictionary<string, float>());
                foreach(var j in prescriptions)
                {
                    if(i.Id == j.PatientId)
                    {
                        if (dict_for_a[i.Id].ContainsKey(j.MedicineName))
                        {
                            dict_for_a[i.Id][j.MedicineName] += j.GetAmountPerDay;
                        }
                        else 
                        {
                            dict_for_a[i.Id].Add(j.MedicineName, j.GetAmountPerDay);
                        }
                    }
                }
            }

            foreach (var patient in dict_for_a)
            {
                var new_patient = new Patient();
                foreach (var k in patients) 
                {
                    if(patient.Key == k.Id)
                    {
                        new_patient = k;
                    }
                }
                Console.WriteLine($"Пацієнт: {new_patient.Name} {new_patient.Surname}");

                foreach (var medication in patient.Value)
                {
                    Console.WriteLine($"Препарат: {medication.Key}, загальна кількість: {medication.Value}мг");
                }
                Console.WriteLine();
            }


            //(б)для кожного дня розхiд виданих препаратiв у мг;

            Dictionary<DateTime, Dictionary<string, float>> dict_for_b = new Dictionary<DateTime, Dictionary<string, float>>();

            foreach (var j in prescriptions)
            {
                dict_for_b.TryAdd(j.Date, new Dictionary<string, float>());
                if (dict_for_b[j.Date].ContainsKey(j.MedicineName))
                {
                    dict_for_b[j.Date][j.MedicineName] += j.GetAmountPerDay;
                }
                else
                {
                    dict_for_b[j.Date].Add(j.MedicineName, j.GetAmountPerDay);
                }
            }

            foreach (var day in dict_for_b)
            {
                Console.WriteLine($"День: {day.Key}");

                foreach (var medication in day.Value)
                {
                    Console.WriteLine($"Препарат: {medication.Key}, загальна кількість: {medication.Value}мг");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            //(в)для кожного препарату загальний розхiд за усi днi.

            Dictionary<string, float> dict_for_c = new Dictionary<string, float>();
            foreach(var i in prescriptions)
            {
                dict_for_c.TryAdd(i.MedicineName, 0);
                dict_for_c[i.MedicineName] += i.GetAmountPerDay;
            }

            foreach (var med in dict_for_c)
            {
                Console.WriteLine($"Препарат: {med.Key}, загальна кількість: {med.Value}мг");
            }
        }
        class Patient
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public uint Id { get; set; }

            public Patient(string name = "", string surname = "", uint id = 0)
            {
                Name = name;
                Surname = surname;
                Id = id;
            }
        }
        class Appointment
        {
            public uint PatientId { get; set; }
            public DateTime Date { get; set; }
            public string MedicineName { get; set; }
            public float Dose { get; set; }
            public uint NumberOfReceptions { get; set; }
            public float GetAmountPerDay { get => Dose * NumberOfReceptions; }

            public Appointment(uint patientId = 0, DateTime date = new DateTime(), string medicineName = "", float dose = 0, uint numberOfReceptions = 0)
            {
                PatientId = patientId;
                Date = date;
                MedicineName = medicineName;
                Dose = dose;
                NumberOfReceptions = numberOfReceptions;
            }
        }
    }
}

