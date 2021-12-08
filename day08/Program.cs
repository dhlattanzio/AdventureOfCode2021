using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day08
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine("-- Day 08 --");
            var input = File.ReadAllText("input/input.txt").Trim();
            var lines = input.Split("\r\n");

            int total = 0;
            foreach (var line in lines)
            {
                var parts = line.Split(" | ");
                var digits = parts[1].Split(" ");

                total += digits.Aggregate<string, int>(0, (acc, digit) =>
                {
                    acc += (digit.Length is <= 4 or 7) ? 1 : 0;
                    return acc;
                });
            }
            
            Console.WriteLine($"Result part 1: {total}");

            var wireCombinations = new Dictionary<int, int>();
            wireCombinations.Add(0b1110111, 0);
            wireCombinations.Add(0b0100100, 1);
            wireCombinations.Add(0b1011101, 2);
            wireCombinations.Add(0b1101101, 3);
            wireCombinations.Add(0b0101110, 4);
            wireCombinations.Add(0b1101011, 5);
            wireCombinations.Add(0b1111011, 6);
            wireCombinations.Add(0b0100101, 7);
            wireCombinations.Add(0b1111111, 8);
            wireCombinations.Add(0b1101111, 9);

            total = 0;
            foreach (var line in lines)
            {
                var numberMap = new Dictionary<int, char[]>();
                for (int i = 0; i < 7; i++)
                {
                    numberMap.Add(i, new [] {'a', 'b', 'c', 'd', 'e', 'f', 'g'});
                }

                var digitLetterMap = new Dictionary<int, int[]>();
                digitLetterMap.Add(0, new[] {0, 1, 2, 4, 5, 6});
                digitLetterMap.Add(1, new[] {2, 5});
                digitLetterMap.Add(2, new[] {0, 2, 3, 4, 6});
                digitLetterMap.Add(3, new[] {0, 2, 3, 5, 6});
                digitLetterMap.Add(4, new[] {1, 2, 3, 5});
                digitLetterMap.Add(5, new[] {0, 1, 3, 5, 6});
                digitLetterMap.Add(6, new[] {0, 1, 3, 4, 5, 6});
                digitLetterMap.Add(7, new[] {0, 2, 5});
                digitLetterMap.Add(8, new[] {0, 1, 2, 3, 4, 5, 6});
                digitLetterMap.Add(9, new[] {0, 1, 2, 3, 5, 6});

                var digitLetterReverseMap = ReverseDigitLetterMap(digitLetterMap);
                
                var parts = line.Split(" | ");
                var signal = parts[0].Split(" ").Select(x => x.ToCharArray());
                var digits = parts[1].Split(" ");

                foreach(var wire in signal)
                {
                    int number = ParseSignal(wire);
                    if (number >= 0)
                    {
                        var indexes = digitLetterMap[number];
                        var reverseIndexes = digitLetterReverseMap[number];
                        foreach (var index in reverseIndexes)
                        {
                            numberMap[index] = numberMap[index].Except(wire).ToArray();
                        }

                        foreach (var index in indexes)
                        {
                            numberMap[index] = numberMap[index].Where(x => wire.Contains(x)).ToArray();
                        }
                    }
                }

                // Find wire 2 and 5
                var wires069Plus1 = signal.Where(x => x.Length == 6).ToList();
                wires069Plus1.Add(signal.Single(x => x.Length == 2));

                var wireCounter = wires069Plus1.Aggregate(new Dictionary<char, int>(), (acc, wire) =>
                    {
                        foreach (var l in wire)
                        {
                            acc[l] = acc.GetValueOrDefault(l, 0) + 1;
                        }
                        return acc;
                    });
                var wire5 = wireCounter.Where(x => x.Value == 4)
                    .Select(x => x.Key).ToArray();

                for (int i = 0; i < numberMap.Count; i++)
                {
                    numberMap[i] = i == 5 ? wire5 : numberMap[i].Except(wire5).ToArray();
                }
                
                // Find wire 4
                var wires069Plus4 = signal.Where(x => x.Length == 6).ToList();
                wires069Plus4.Add(signal.Single(x => x.Length == 4));
                
                wireCounter = wires069Plus4.Aggregate(new Dictionary<char, int>(), (acc, wire) =>
                {
                    foreach (var l in wire)
                    {
                        acc[l] = acc.GetValueOrDefault(l, 0) + 1;
                    }
                    return acc;
                });
                var wire4 = wireCounter.Where(x => x.Value == 2)
                    .Select(x => x.Key).ToArray();
                
                for (int i = 0; i < numberMap.Count; i++)
                {
                    numberMap[i] = i == 4 ? wire4 : numberMap[i].Except(wire4).ToArray();
                }

                // Find wire 1 and 3
                var wires1and5 = wireCounter.Where(x => x.Value == 4)
                    .Select(x => x.Key).ToArray();
                var wire1 = new[] {numberMap[5].Contains(wires1and5[0]) ? wires1and5[1] : wires1and5[0]};
                for (int i = 0; i < numberMap.Count; i++)
                {
                    numberMap[i] = i == 1 ? wire1 : numberMap[i].Except(wire1).ToArray();
                }

                var numberArray = numberMap.Select(x => x.Value[0]).ToArray();
                
                // Console.WriteLine($"Dictionary: {JsonSerializer.Serialize(numberArray)}");

                int result = 0;
                foreach (var digit in digits)
                {
                    var digitArray = digit.ToCharArray();
                    int numberCode = 0;
                    foreach (var digitChar in digitArray)
                    {
                        var index = Array.FindIndex(numberArray, x => x == digitChar);
                        numberCode += 1 << index;
                    }

                    result = result * 10 + wireCombinations[numberCode];
                }
                
                // Console.WriteLine($"Code: {result}");
                total += result;
            }
            
            Console.WriteLine($"Result part 2: {total}");
        }

        public int ParseSignal(char[] signal)
        {
            switch (signal.Length)
            {
                case 2:
                    return 1;
                case 3:
                    return 7;
                case 4:
                    return 4;
                case 7:
                    return 8;
                default:
                    return -1;
            }
        }

        public Dictionary<int, int[]> ReverseDigitLetterMap(Dictionary<int, int[]> dict)
        {
            var tmp = new Dictionary<int, int[]>();
            for (int i = 0; i < 10; i++)
            {
                var arr = new[] {0, 1, 2, 3, 4, 5, 6};
                tmp.Add(i, arr.Except(dict[i]).ToArray());
            }
            return tmp;
        }
    }
}
