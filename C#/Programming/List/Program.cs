using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main(string[] args)
        {
            var wc1 = new WaterCounter();
            var wc2 = new WaterCounter(23, 2, 32, 5);
            var wc3 = new WaterCounter(4, 3, 5, 7);
            var wc4 = new WaterCounter(345, 7, 349, 7);
            var wc5 = new WaterCounter(349, 7, 351, 9);

            var waterCounters = new WaterCounter[] { wc1, wc2, wc3, wc4, wc5 };

            int monthNumber = 7;
            for (int i = 0; i < waterCounters.Length; i++)
            {
                if (waterCounters[i].MonthNumber == monthNumber)
                    Console.WriteLine(waterCounters[i].Room + ": " + waterCounters[i].usedWater());
            }

            Array.Sort(waterCounters, new WatercounterComparer<WaterCounter>());
            foreach (var i in waterCounters) { Console.WriteLine(i.usedWater()); }
        }
    }

    public class WaterCounter
    {
        public WaterCounter(int curr = 0, int prev = 0, int r = 0, int month = 0)
        {
            CurrentCounter = curr;
            PreviousCounter = prev;
            Room = r;
            MonthNumber = month;
        }
        public int Room { get; set; } 
        public int CurrentCounter { get; set; }
        public int PreviousCounter { get; set; }
        public int MonthNumber { get; set; }

        public int usedWater()
        {
            return CurrentCounter - PreviousCounter;
        }
    }
    public class WatercounterComparer<T> : IComparer<T> where T : WaterCounter
    {
        public int Compare(T first, T second)
        {
            return first.usedWater().CompareTo(second.usedWater());
        }
    }
}