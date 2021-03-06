using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class StorageComputer : IInteractable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IScreen ContainerScreen { get; private set; }

        public ColoredSymbol Symbol { get; private set; }

        public StorageComputer(char symbol, ConsoleColor color, IScreen screen, int x = 0, int y = 0)
        {
            Symbol = new ColoredSymbol(symbol, color);
            ContainerScreen = screen;
            X = x;
            Y = y;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Symbol.Color;
            Console.Write(Symbol.Symbol);
        }

        public void Interact()
        {
            if(ContainerScreen is MainGameScreen screen)
            {
                screen.StartOrdering();
            }
        }

        public void MoveTo(int x, int y)
        {
            Undraw();
            X = x;
            Y = y;
            ContainerScreen.MarkForRedraw(this);
        }

        public void Undraw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(' ');
        }
    }
}
