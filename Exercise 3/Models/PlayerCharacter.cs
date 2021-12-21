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
    internal interface IPlayerCharacter : IDrawable, IEntity
    {
        public string Name { get; }
    }

    internal class PlayerCharacter : IPlayerCharacter
    {
        private int _xOffset;
        private int _yOffset;
        public IScreen ContainerScreen { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }

        public string Name { get; private set; }

        public string Symbol { get; private set; }

        public ConsoleColor Color { get; private set; }

        public PlayerCharacter(string name, string symbol, ConsoleColor color, IScreen container)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
            ContainerScreen = container;
        }

        public void Draw(int xOffset, int yOffset)
        {
            _xOffset = xOffset;
            _yOffset = yOffset;
            Console.SetCursorPosition(X * ContainerScreen.XMultiplier + _xOffset, Y + _yOffset);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);
        }

        public void MoveTo(int x, int y)
        {
            Undraw();
            X = x;
            Y = y;
        }

        public void Undraw()
        {
            Console.SetCursorPosition(X * ContainerScreen.XMultiplier + _xOffset, Y + _yOffset);
            Console.Write(' ');
            ContainerScreen.MarkForRedraw(this);
        }
    }
}
