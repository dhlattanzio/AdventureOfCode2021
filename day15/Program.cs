using System;
using System.IO;

namespace day15
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 15 --");
            var input = File.ReadAllText("input/test.txt").Trim();
            var parts = input.Split("\r\n");
        }
    }
}
