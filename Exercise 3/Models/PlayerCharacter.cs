using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Managers;

namespace WarehouseWorker.Models
{
    /// <summary>
    /// A character that the player can control.
    /// </summary>
    internal interface IPlayerCharacter : IEntity
    {
        public string Name { get; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public ICarryable? HeldItem { get; set; }

        void SetDirection(string direction);
        void PickUpItem(ICarryable item);
        ICarryable? PutDownItem(int x, int y);
    }

    internal class PlayerCharacter : IPlayerCharacter
    {
        public IScreen ContainerScreen { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public string Name { get; private set; }

        public ICarryable? HeldItem { get; set; }

        public ColoredSymbol Symbol { get; private set; }

        public PlayerCharacter(string name, char symbol, ConsoleColor color, IScreen container)
        {
            Name = name;
            Symbol = new ColoredSymbol(symbol, color);
            ContainerScreen = container;
        }
        public PlayerCharacter(string name, ColoredSymbol symbol, IScreen container)
        {
            Name = name;
            Symbol = symbol;
            ContainerScreen = container;
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

        public void SetDirection(string direction)
        {
            switch(direction)
            {
                case "left":
                    TargetX = X - 1;
                    TargetY = Y;
                    break;
                case "right":
                    TargetX = X + 1;
                    TargetY = Y;
                    break;
                case "up":
                    TargetX = X;
                    TargetY = Y - 1;
                    break;
                case "down":
                    TargetX = X;
                    TargetY = Y + 1;
                    break;
            }
        }

        public void PickUpItem(ICarryable item)
        {
            HeldItem = item;
            HeldItem.Undraw();
        }

        public ICarryable? PutDownItem(int x, int y)
        {
            ICarryable? item = HeldItem;
            if (item != null)
                item.MoveTo(x, y);
            HeldItem = null;
            return item;
        }
    }
}
