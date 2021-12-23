using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class Wall : IEntity
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IScreen ContainerScreen { get; private set; }

        public ColoredSymbol Symbol { get; private set; }

        public Wall(char symbol, int x, int y, IScreen container)
        { 
            Symbol = new ColoredSymbol(symbol, ConsoleColor.DarkGray);
            X = x;
            Y = y;
            ContainerScreen = container;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Symbol.Color;
            Console.Write(Symbol.Symbol);
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
