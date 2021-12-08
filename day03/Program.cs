using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 3 --");
            string input = File.ReadAllText("input/input.txt").Trim();
            List<string> lines = input.Split("\r\n").ToList();
            
            int[] counts = GetCount(lines);

            int maxBinary = (int)Math.Pow(2, counts.Length) - 1;
            int gamma = 0;
            int epsilon;
            for (int i = 0; i < counts.Length; i++)
            {
                gamma <<= 1;
                gamma += counts[i] > lines.Count/2 ? 1 : 0;
            }
            epsilon = gamma ^ maxBinary;
            
            Console.WriteLine($"Result part 1: " + (gamma * epsilon));
            
            List<string> listNums = new(lines);
            for (int i = 0; i < counts.Length; i++)
            {
                int size = listNums.Count;
                counts = GetCount(listNums);
                listNums = listNums.Where(x => x[i] == (counts[i] >= Math.Ceiling(size / 2f) ? '1' : '0')).ToList();
                if (listNums.Count == 1) break;
            }
            gamma = Convert.ToInt32(listNums[0], 2);
            
            listNums = new(lines);
            for (int i = 0; i < counts.Length; i++)
            {
                int size = listNums.Count;
                counts = GetCount(listNums);
                listNums = listNums.Where(x => x[i] == (counts[i] >= Math.Ceiling(size / 2f) ? '0' : '1')).ToList();
                if (listNums.Count == 1) break;
            }
            epsilon = Convert.ToInt32(listNums[0], 2);

            Console.WriteLine($"Result part 2: "+ (gamma * epsilon));
        }

        public int[] GetCount(List<string> lines)
        {
            int[] counts = new int[lines[0].Length];
            foreach (var line in lines)
            {
                char[] chars = line.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    counts[i] += chars[i] == '0' ? 0 : 1;
                }
            }
            return counts;
        }
    }
}
