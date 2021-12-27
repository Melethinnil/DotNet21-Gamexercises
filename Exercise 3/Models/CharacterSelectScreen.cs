using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Managers;

namespace WarehouseWorker.Models
{
    internal class CharacterSelectScreen : IScreen
    {
        private List<ColoredSymbol> _playerSymbols = new List<ColoredSymbol>();
        private string _playerName = "";
        private ColoredSymbol? _playerSymbol = null;
        private int _cursorPos = 0;

        public CharacterSelectScreen()
        {
        }

        public void MarkForRedraw(IDrawable drawable)
        {
        }

        public IScreen Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            if (_playerName == "")
            {
                Console.Write("Please enter your name: ");
                _playerName = Console.ReadLine() ?? "";
                if(_playerName != "")
                {
                    char symbol = _playerName.ToUpper()[0];
                    _playerSymbols.Add(new ColoredSymbol(symbol, ConsoleColor.Red));
                    _playerSymbols.Add(new ColoredSymbol(symbol, ConsoleColor.Green));
                    _playerSymbols.Add(new ColoredSymbol(symbol, ConsoleColor.Blue));
                    _playerSymbols.Add(new ColoredSymbol(symbol, ConsoleColor.DarkYellow));
                    _playerSymbols.Add(new ColoredSymbol(symbol, ConsoleColor.Magenta));
                    _playerSymbols.Add(new ColoredSymbol(symbol, ConsoleColor.Cyan));
                    _playerSymbols.Add(new ColoredSymbol(symbol, ConsoleColor.White));
                }
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Choose your character colour");
                for(int i = 0; i < _playerSymbols.Count; i++)
                {
                    ColoredSymbol symbol = _playerSymbols[i];
                    Console.ForegroundColor = symbol.Color;
                    Console.SetCursorPosition(i * 2, 4);
                    Console.Write(symbol.Symbol);
                }

                Console.SetCursorPosition(_cursorPos*2, 5);

                //Read player input
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        _cursorPos--;
                        if (_cursorPos < 0)
                            _cursorPos = _playerSymbols.Count - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        _cursorPos++;
                        if (_cursorPos > _playerSymbols.Count - 1)
                            _cursorPos = 0;
                        break;
                    case ConsoleKey.Enter:
                        _playerSymbol = _playerSymbols[_cursorPos];
                        break;
                }

                if(_playerSymbol != null)
                    return new IntroScreen(_playerName, _playerSymbol);
            }

            return this;
        }
    }
}
