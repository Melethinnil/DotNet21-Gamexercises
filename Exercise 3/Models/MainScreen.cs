using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker
{
    internal class MainScreen : IScreen
    {
        private ScreenSpace _roomBounds = new ScreenSpace()
        {
            X = 0,
            Y = 0,
            Width = 20,
            Height = 20
        };
        private char _horizontalWall = '=';
        private string _verticalWall = "||";
        private ConsoleColor _wallColor = ConsoleColor.DarkGray;

        public void Draw()
        {
            Console.ForegroundColor = _wallColor;
            //Draw the top wall
            Console.SetCursorPosition(_roomBounds.X, _roomBounds.Y);
            Console.WriteLine("".PadRight(_roomBounds.Width, _horizontalWall));
            //Draw the side walls
            for(int i = 0; i < _roomBounds.Height; i++)
            {
                Console.SetCursorPosition(_roomBounds.X, _roomBounds.Y+1+i);
                Console.WriteLine(_verticalWall + _verticalWall.ToString().PadLeft(_roomBounds.Width-2, ' '));
            }
            //Draw the bottom wall
            Console.SetCursorPosition(_roomBounds.X, _roomBounds.Y + _roomBounds.Height);
            Console.WriteLine("".PadRight(_roomBounds.Width, _horizontalWall));
        }
    }
}
