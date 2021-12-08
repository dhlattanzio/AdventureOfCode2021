using System;
using System.IO;
using System.Linq;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Day 1 --");
            string input = File.ReadAllText("input/input.txt");

            int[] deep = input.Split("\n").Select(int.Parse).ToArray();

            int prev = 0;
            bool first = true;
            int counter = 0;
            foreach(int value in deep) {
                if (!first && prev < value) counter++;
                prev = value;
                first = false;
            }

            Console.WriteLine("Total (part 1): " + counter);

            counter = 0;
            for(int i=3;i<deep.Length;i++) {
                int sum1 = deep[i-3] + deep[i-2] + deep[i-1];
                int sum2 = deep[i-2] + deep[i-1] + deep[i];
                if (sum1 < sum2) counter++;
            }
            Console.WriteLine("Total (part 2): " + counter);
        }
    }
}
