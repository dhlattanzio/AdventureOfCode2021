using System;
using System.IO;
using System.Linq;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }
        
        public Program()
        {
            Console.WriteLine("-- Day 6 --");
            string input = File.ReadAllText("input/input.txt").Trim();
            int[] startFish = input.Split(",").Select(int.Parse).ToArray();
            
            // Start
            long[] fish = new long[9];
            foreach (var f in startFish)
            {
                fish[f]++;
            }

            long[] fishCopy = new long[9];
            fish.CopyTo(fishCopy, 0);

            // 80 Days
            for (int i = 0; i < 80; i++)
            {
                fish = Tick(fish);
            }
            
            Console.WriteLine("Result part 1: " + GetTotalFish(fish));

            fish = fishCopy;
            // 256 Days
            for (int i = 0; i < 256; i++)
            {
                fish = Tick(fish);
            }
            
            Console.WriteLine("Result part 2: " + GetTotalFish(fish));
        }

        public long GetTotalFish(long[] fish)
        {
            return fish.Aggregate<long, long>(0, (acc, f) => acc + f);
        }

        public long[] Tick(long[] fish)
        {
            long[] newFish = new long[fish.Length];
            for (int i = fish.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    newFish[8] += fish[i];
                    newFish[6] += fish[0];
                }
                else
                {
                    newFish[i - 1] += fish[i];
                }
            }

            return newFish;
        }
    }
}
