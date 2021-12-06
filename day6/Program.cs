using System;
using System.IO;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Day 4 --");
            string input = File.ReadAllText("input/input.txt").Trim();
            string[] lines = input.Split("\r\n\r\n");
        }
    }
}
