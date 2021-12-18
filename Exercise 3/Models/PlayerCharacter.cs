using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker
{
    internal class PlayerCharacter : IControllable, IDrawable, IEntity
    {
        public string Name { get; }
        public char Symbol { get; }
        public ICarryable? HeldItem { get; private set; } = null;

        public ScreenSpace Position { get; private set; }
        public ConsoleColor Color { get; set; }

        public PlayerCharacter(string name, char symbol, ConsoleColor color)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
        }

        /// <summary>
        /// Moves the character 1 step in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to move, either up, right, down or left.</param>
        public void Move(Direction direction)
        {
            UnDraw();
            ScreenSpace pos = Position;
            switch(direction)
            {
                case Direction.Left:
                    pos.X--;
                    break;
                    case Direction.Right:
                    pos.X++;
                    break;
                    case Direction.Up:
                    pos.Y--;
                    break;
                    case Direction.Down:
                    pos.Y++;
                    break;
            }
            Position = pos;
        }
        public void MoveTo(int x, int y)
        {
            UnDraw();
            ScreenSpace pos = Position;
            pos.X = x;
            pos.Y = y;
            Position = pos;
        }

        public void PickUp(ICarryable item)
        {
            throw new NotImplementedException();
        }

        public ICarryable PutDown()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);
        }

        public void UnDraw()
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(" ");
        }
    }
}
