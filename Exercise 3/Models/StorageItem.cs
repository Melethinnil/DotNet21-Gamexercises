using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal interface IStorageItem : ICarryable
    {
        int ID { get; }
        string Name { get; }
        string Category { get; }
        string Description { get; }
        int Price { get; }
    }
    internal class StorageItem : IStorageItem
    {
        public int X { get; set; }
        public int Y { get; set; }

        public char Symbol { get; private set; }

        public ConsoleColor Color { get; private set; }

        public IScreen ContainerScreen { get; private set; }

        public int ID { get; private set; }

        public string Name { get; private set; }

        public string Category { get; private set; }

        public string Description { get; private set; }

        public int Price { get; private set; }

        public StorageItem(int id, string name, string category, string description, int price, char symbol, ConsoleColor color, IScreen screen, int x = 0, int y = 0)
        {
            ID = id;
            Name = name;
            Category = category;
            Description = description;
            Price = price;
            Symbol = symbol;
            Color = color;
            ContainerScreen = screen;
            X = x;
            Y = y;
        }
        public StorageItem(IScreen screen)
        {
            ID = 0;
            Name = String.Empty;
            Category = String.Empty;
            Description = String.Empty;
            Price = 0;
            Symbol = ' ';
            Color = ConsoleColor.White;
            ContainerScreen= screen;
            X = 0;
            Y = 0;
        }

        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);
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

        public ICarryable PickUp()
        {
            Undraw();
            return this;
        }

        public void PutDown(int x, int y)
        {
            X = x;
            Y = y;
            ContainerScreen.MarkForRedraw(this);
        }
    }
}
