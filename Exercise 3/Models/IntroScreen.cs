using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class IntroScreen : IScreen
    {
        private string _playerName;
        private ColoredSymbol _playerSymbol;

        public IntroScreen(string playerName, ColoredSymbol playerSymbol)
        {
            _playerName = playerName;
            _playerSymbol = playerSymbol;
        }

        public void MarkForRedraw(IDrawable drawable)
        {
            throw new NotImplementedException();
        }

        public IScreen Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Greetings, ");
            Console.ForegroundColor = _playerSymbol.Color;
            Console.Write(_playerName);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("!");

            Console.WriteLine("\nYou are a worker in a storage facility.\n" +
                "You are responsible for ordering various items and\n" +
                "storing them as efficiently as possible.");
            Console.ReadLine();

            Console.Clear();
            Console.Write("You order items by interacting with the storage\n" +
                "computer (");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write('@');
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(") and filling in the required information.\n" +
                "To order another one of an item that already is in storage,\n" +
                "you only need to enter the same ID.\n\n" +
                "The item you order will then be shipped to the conveyor\n" +
                "belt that leads into the room. Pick the item up from the\n" +
                "conveyor belt and put it down somewhere in the room.\n\n" +
                "Items can only be placed next to other items with the same\n" +
                "ID. To remove unwanted items, you can throw them in the\n" +
                "incinerator (");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write('X');
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(").");
            Console.ReadLine();

            Console.Clear();
            Console.Write("Your task is to order and store as many items\n" +
                "as possible with as many different IDs as possible. When\n" +
                "you feel finished, exit the room through the door (");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write('\\');
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(") to see your score.");
            Console.ReadLine();

            Console.Clear();
            Console.WriteLine("To make sure the game is displayed as\n" +
                "intended, please right-click on the header of the\n" +
                "console window and choose \"Properties\". In the\n" +
                "Font tab, select \"Raster Fonts\" in the Font list,\n" +
                "and set the Size to 12x16.");

            Console.WriteLine("\n\nPress enter to start the game.");
            Console.ReadLine();

            return new MainGameScreen(16, 12, _playerName, _playerSymbol);
        }
    }
}
