using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        private Program()
        {
            Console.WriteLine("-- Day 4 --");
            string input = File.ReadAllText("input/input.txt").Trim();
            string[] lines = input.Split("\r\n\r\n");

            List<int> numbers = lines[0].Split(",").Select(int.Parse).ToList();
            List<Board> boards = new List<Board>();

            for (int i = 1; i < lines.Length; i++)
            {
                boards.Add(new Board(lines[i]));
            }

            Board boardWin = null;
            List<Board.BoardNumber> winList = null;
            List<int> numsCalled = new();
            foreach(int num in numbers)
            {
                numsCalled.Add(num);
                foreach(Board board in boards)
                {
                    boardWin = board;
                    winList = board.checkHorizontalWin(numsCalled);
                    if (winList.Count > 0) break;
                    winList = board.checkVerticalWin(numsCalled);
                    if (winList.Count > 0) break;
                    boardWin = null;
                }

                if (winList != null && winList.Count > 0) break;
            }
            
            int result = boardWin.GetAllUnmarked().Aggregate(0, (acc, number) => acc += number.Number) * numsCalled[^1];
            Console.WriteLine($"Result part 1: "+result);
            
            
            boardWin = null;
            List<Board> winners = new();
            numsCalled = new();
            foreach(int num in numbers)
            {
                numsCalled.Add(num);
                foreach(Board board in boards)
                {
                    if (board.checkHorizontalWin(numsCalled).Count > 0 
                        || board.checkVerticalWin(numsCalled).Count > 0)
                    {
                        winners.Add(board);
                    }
                }

                boards = boards.Except(winners).ToList();
                if (boards.Count == 0)
                {
                    boardWin = winners[^1];
                    break;
                }
                winners.Clear();
            }
            
            result = boardWin.GetAllUnmarked().Aggregate(0, (acc, number) => acc += number.Number) * numsCalled[^1];
            Console.WriteLine($"Result part 2: " + result);
        }

        public class Board
        {
            private readonly List<BoardNumber> _numbers;
            private readonly int _numsPerLine;
            
            public Board(string board)
            {
                _numbers = new();

                string[] lines = board.Split("\r\n");
                _numsPerLine = lines[0].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray().Length;
                foreach (var line in lines)
                {
                    string[] nums = line.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    _numbers.AddRange(nums.Select(x => new BoardNumber {Number = int.Parse(x)}).ToList());
                }
            }

            public List<BoardNumber> checkVerticalWin(List<int> numbers)
            {
                List<BoardNumber> list = new();
                for (int i = 0; i < _numsPerLine; i++)
                {
                    for (int j = 0; j < _numbers.Count / _numsPerLine; j++)
                    {
                        BoardNumber boardNumber = _numbers[(j * _numsPerLine) + i];
                        if (numbers.Contains(boardNumber.Number))
                        {
                            boardNumber.Marked = true;
                            list.Add(boardNumber);
                        }
                    }
                    if (list.Count == _numsPerLine) break;
                    list.Clear();
                }
                
                return list;
            }

            public List<BoardNumber> checkHorizontalWin(List<int> numbers)
            {
                List<BoardNumber> list = new();
                for (int i = 0; i < _numbers.Count / _numsPerLine; i++)
                {
                    for (int j = 0; j < _numsPerLine; j++)
                    {
                        BoardNumber boardNumber = _numbers[(i * _numsPerLine) + j];
                        if (numbers.Contains(boardNumber.Number))
                        {
                            boardNumber.Marked = true;
                            list.Add(boardNumber);
                        }
                    }
                    if (list.Count == _numsPerLine) break;
                    list.Clear();
                }
                
                return list;
            }

            public List<BoardNumber> GetAllUnmarked()
            {
                return _numbers.Where(x => !x.Marked).ToList();
            }

            public class BoardNumber
            {
                public int Number { get; set; }
                public bool Marked { get; set; }
            }
        }
    }
}
