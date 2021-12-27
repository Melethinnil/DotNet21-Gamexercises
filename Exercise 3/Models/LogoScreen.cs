using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class LogoScreen : IScreen
    {
        public void MarkForRedraw(IDrawable drawable)
        {
            throw new NotImplementedException();
        }

        public IScreen Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 7);

            Console.WriteLine(CenteredString(" _ _ _ _____ _____ _____ _____ _____ _____ _____ _____ ", ' ', Console.BufferWidth));
            Console.WriteLine(CenteredString("| | | |  _  | __  |   __|  |  |     |  |  |   __|   __|", ' ', Console.BufferWidth));
            Console.WriteLine(CenteredString("| | | |     |    -|   __|     |  |  |  |  |__   |   __|", ' ', Console.BufferWidth));
            Console.WriteLine(CenteredString("|_____|__|__|__|__|_____|__|__|_____|_____|_____|_____|", ' ', Console.BufferWidth));
            Console.WriteLine(CenteredString("         _ _ _ _____ _____ _____ _____ _____           ", ' ', Console.BufferWidth));
            Console.WriteLine(CenteredString("        | | | |     | __  |  |  |   __| __  |          ", ' ', Console.BufferWidth));
            Console.WriteLine(CenteredString("        | | | |  |  |    -|    -|   __|    -|          ", ' ', Console.BufferWidth));
            Console.WriteLine(CenteredString("        |_____|_____|__|__|__|__|_____|__|__|          ", ' ', Console.BufferWidth));
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("START GAME".PadLeft(Console.BufferWidth / 2 + 5, ' '));
            Console.ReadLine();

            return new CharacterSelectScreen();
        }
        private string CenteredString(string str, char padding, int width)
        {
            int left = (width - str.Length - 2) / 2;
            return $" {str} ".PadRight(width - left, padding).PadLeft(width, padding);
        }
    }
}
