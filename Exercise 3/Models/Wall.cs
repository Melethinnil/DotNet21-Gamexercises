using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class Wall : IDrawable, IEntity
    {
        public char Symbol { get; private set; }
        public ConsoleColor Color { get; private set; } = ConsoleColor.DarkGray;
        public int X { get; set; }
        public int Y { get; set; }

        public IScreen ContainerScreen { get; private set; }

        public Wall(char symbol, int x, int y, IScreen container)
        { 
            Symbol = symbol;
            X = x;
            Y = y;
            ContainerScreen = container;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);
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
