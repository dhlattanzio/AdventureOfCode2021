using System;
using System.IO;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Day 2 --");
            string input = File.ReadAllText("input/input.txt").Trim();
            string[] lines = input.Split("\n");

            int x = 0;
            int z = 0;
            foreach (var line in lines)
            {
                var command = line.Split(" ");
                int units = int.Parse(command[1]);
                
                switch (command[0])
                { 
                    case "forward":
                        x += units;
                        break;
                    case "down":
                        z += units;
                        break;
                    case "up":
                        z -= units;
                        break;
                }
            }
            
            Console.WriteLine($"Result part 1: x={x} z={z} => {x*z}");

            int aim = 0;
            x = z = 0;
            foreach (var line in lines)
            {
                var command = line.Split(" ");
                int units = int.Parse(command[1]);
                
                switch (command[0])
                { 
                    case "forward":
                        x += units;
                        z += units * aim;
                        break;
                    case "down":
                        aim += units;
                        break;
                    case "up":
                        aim -= units;
                        break;
                }
            }
            
            Console.WriteLine($"Result part 2: x={x} z={z} aim={aim} => {x*z}");
        }
    }
}
