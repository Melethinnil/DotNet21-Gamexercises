using WarehouseWorker.Managers;

namespace WarehouseWorker.Models
{
    internal class Incinerator : IInteractable
    {
        private IEntityManager _entityManager;
        public int X { get; set; }
        public int Y { get; set; }
        public ColoredSymbol Symbol {get; private set;}
        public IScreen ContainerScreen { get; private set; }
        public Incinerator(char symbol, int x, int y, IScreen screen, IEntityManager entityManager)
        {
            Symbol = new ColoredSymbol(symbol, ConsoleColor.DarkRed);
            X = x;
            Y = y;
            ContainerScreen = screen;
            _entityManager = entityManager;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Symbol.Color;
            Console.Write(Symbol.Symbol);
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

        public void Interact()
        {
            _entityManager.DestroyHeldItem();
        }
    }
}