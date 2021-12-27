using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class ScoreScreen : IScreen
    {
        private TimeSpan _totalTime;
        private int _totalItems;
        private int _numUniqueItems;
        private int _finalScore;
        private string _playerName;
        private string _saveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WarehouseWorker";

        public ScoreScreen(DateTime startTime, int totalItems, int numUniqueItems, string playerName)
        {
            _totalTime = DateTime.Now - startTime;
            _totalItems = totalItems;
            _numUniqueItems = numUniqueItems;
            _playerName = playerName;
            CalculateFinalScore();
        }

        private void CalculateFinalScore()
        {
            //_finalScore = _totalItems * (_numUniqueItems * 4) / _totalTime.Seconds;
            _finalScore = (_totalItems * 100) * (int)Math.Pow(_numUniqueItems / 2, 2) / _totalTime.Seconds;
        }

        public void MarkForRedraw(IDrawable drawable)
        {
            throw new NotImplementedException();
        }

        public IScreen Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(CenteredString("GAME OVER", '#', Console.BufferWidth));
            Console.WriteLine("\n");

            Console.WriteLine($"You managed to store a total of {_totalItems} items.\n" +
                $"There were {_numUniqueItems} unique item IDs in storage.\n" +
                $"You took {_totalTime.Minutes} minutes and {_totalTime.Seconds} seconds.");
            Console.WriteLine($"\nYour final score is {_finalScore}.");

            Console.WriteLine("\n\nPress enter to quit the game.");
            Console.ReadLine();

            Directory.CreateDirectory(_saveFilePath);
            File.AppendAllLines(_saveFilePath + @"\scores.txt", new string[] { $"{_playerName}:{_finalScore}" });
            Environment.Exit(0);

            return this;
        }
        private string CenteredString(string str, char padding, int width)
        {
            int left = (width - str.Length - 2) / 2;
            return $" {str} ".PadRight(width - left, padding).PadLeft(width, padding);
        }
    }
}
