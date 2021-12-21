using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class Wall : IDrawable, IEntity
    {
        private int _xOffset;
        private int _yOffset;
        public bool IsHorizontal { get; private set; }
        public string Symbol { get; private set; }
        public ConsoleColor Color { get; private set; } = ConsoleColor.DarkGray;
        public int X { get; set; }
        public int Y { get; set; }

        public IScreen ContainerScreen { get; private set; }

        public Wall(string symbol, int x, int y, IScreen container, bool horizontal = false)
        { 
            Symbol = symbol;
            X = x;
            Y = y;
            IsHorizontal = horizontal;
            ContainerScreen = container;
        }

        public void Draw(int xOffset, int yOffset)
        {
            _xOffset = xOffset;
            _yOffset = yOffset;
            Console.ForegroundColor = Color;
            if (IsHorizontal)
            {
                Console.SetCursorPosition(X, Y);
                Console.Write(Symbol); 
            }
            else
            {
                for(int i = 0; i < Symbol.Length; i++)
                {
                    Console.SetCursorPosition(X, Y + i);
                    Console.Write(Symbol[i]);
                }
            }

        }

        public void Undraw()
        {
            throw new NotImplementedException();
        }

        public void MoveTo(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
