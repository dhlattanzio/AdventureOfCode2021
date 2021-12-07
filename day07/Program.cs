using System;
using System.IO;
using System.Linq;

namespace day07
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 07 --");
            string input = File.ReadAllText("input/input.txt").Trim();
            
            var crabs = input.Split(",").Select(int.Parse).ToList();
            crabs.Sort();

            int bestTotalFuel = -1;
            for (int i = 1; i < crabs[^1]; i++)
            {
                int pos = i;
                int totalFuel = 0;
                for (int j = 0; j < crabs.Count; j++)
                {
                    totalFuel += Math.Abs(crabs[j] - pos);
                }

                if (bestTotalFuel < 0 || totalFuel < bestTotalFuel)
                {
                    bestTotalFuel = totalFuel;
                }
            }
            
            Console.WriteLine($"Result part 1: {bestTotalFuel}");
            
            bestTotalFuel = -1;
            for (int i = 1; i < crabs[^1]; i++)
            {
                int pos = i;
                int totalFuel = 0;
                for (int j = 0; j < crabs.Count; j++)
                {
                    totalFuel += CalculateFuel(Math.Abs(crabs[j] - pos));
                    if (bestTotalFuel >= 0 && bestTotalFuel < totalFuel) break;
                }

                if (bestTotalFuel < 0 || totalFuel < bestTotalFuel)
                {
                    bestTotalFuel = totalFuel;
                }
            }
            
            Console.WriteLine($"Result part 2: {bestTotalFuel}");
        }
        
        public int CalculateFuel(int f)
        {
            int step = 1;
            int total = 0;
            while (f > 0)
            {
                f--;
                total += step;
                step++;
            }
            return total;
        }
    }
}
