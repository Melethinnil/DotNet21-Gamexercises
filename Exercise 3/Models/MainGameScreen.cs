using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Managers;

namespace WarehouseWorker.Models
{
    /// <summary>
    /// The main screen of the game loop, showing the storage room.
    /// </summary>
    internal class MainGameScreen : IScreen
    {
        private (int x, int y) _computerLocation;
        public IStorageItem? ItemToOrder { get; private set; }
        private List<IDrawable> _toRedraw = new List<IDrawable>();
        public int RoomWidth { get; private set; }
        public int RoomHeight { get; private set; }
        public bool OrderingItems { get; private set; } = false;

        public IEntityManager EntityManager { get; private set; }

        public MainGameScreen(int roomWidth, int roomHeight)
        {
            RoomWidth = roomWidth;
            RoomHeight = roomHeight;
            _computerLocation = (roomWidth/2, RoomHeight-1);
            EntityManager = new EntityManager(this);
            Console.SetWindowSize(50, 50);
            Console.SetBufferSize(50, 50);
            BuildRoom();
            EntityManager.AddPlayer(new PlayerCharacter("Amanda", 'A', ConsoleColor.Green, this), _computerLocation.x, _computerLocation.y+1, "down");
        }

        internal void OrderItem(IStorageItem storageItem)
        {
            ItemToOrder = storageItem;
            OrderingItems = true;
        }

        private void BuildRoom()
        {
            //Symbols list: https://en.wikipedia.org/wiki/List_of_Unicode_characters#Block_Elements

            //Horizontal walls
            for(int i = 1; i < RoomWidth + 2; i++)
            {
                EntityManager.AddEntity(new Wall('═', i, 0, this));
                EntityManager.AddEntity(new Wall('═', i, RoomHeight + 1, this));
            }

            //Vertical walls
            for (int i = 1; i < RoomHeight + 2; i++)
            {
                EntityManager.AddEntity(new Wall('║', 0, i, this));
                EntityManager.AddEntity(new Wall('║', RoomWidth + 1, i, this));
            }

            //Corners
            EntityManager.AddEntity(new Wall('╔', 0, 0, this));
            EntityManager.AddEntity(new Wall('╗', RoomWidth + 1, 0, this));
            EntityManager.AddEntity(new Wall('╚', 0, RoomHeight + 1, this));
            EntityManager.AddEntity(new Wall('╝', RoomWidth + 1, RoomHeight + 1, this));

            //Walls around computer area
            EntityManager.AddEntity(new Wall('╧', _computerLocation.x - 1, _computerLocation.y + 2, this));
            EntityManager.AddEntity(new Wall('│', _computerLocation.x - 1, _computerLocation.y + 1, this));
            EntityManager.AddEntity(new Wall('│', _computerLocation.x - 1, _computerLocation.y, this));
            EntityManager.AddEntity(new Wall('┌', _computerLocation.x - 1, _computerLocation.y - 1, this));
            EntityManager.AddEntity(new Wall('─', _computerLocation.x, _computerLocation.y - 1, this));
            EntityManager.AddEntity(new Wall('┐', _computerLocation.x + 1, _computerLocation.y - 1, this));
            EntityManager.AddEntity(new Wall('│', _computerLocation.x + 1, _computerLocation.y, this));

            //Item ordering computer
            EntityManager.AddEntity(new StorageComputer('@', ConsoleColor.Yellow, this, _computerLocation.x, _computerLocation.y));
        }

        public void Show()
        {
            foreach (IDrawable drawable in _toRedraw)
                drawable.Draw();

            _toRedraw.Clear();

            if(OrderingItems)
            {
                Console.SetCursorPosition(0, RoomHeight + 3);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("### ORDER ITEM ###");
                Console.Write("ID: ");
            }
            else
            {
                (int X, int Y) cursorTarget = GetCursorTarget();
                Console.SetCursorPosition(cursorTarget.X, cursorTarget.Y);

                ConsoleKey key = Console.ReadKey(true).Key;
                int x = 0;
                int y = 0;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        y = -1;
                        EntityManager.MovePlayer(x, y, RoomWidth + 1, RoomHeight + 1, 1, 1);
                        break;
                    case ConsoleKey.DownArrow:
                        y = 1;
                        EntityManager.MovePlayer(x, y, RoomWidth + 1, RoomHeight + 1, 1, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        x = -1;
                        EntityManager.MovePlayer(x, y, RoomWidth + 1, RoomHeight + 1, 1, 1);
                        break;
                    case ConsoleKey.RightArrow:
                        x = 1;
                        EntityManager.MovePlayer(x, y, RoomWidth + 1, RoomHeight + 1, 1, 1);
                        break;
                    case ConsoleKey.Spacebar:
                        EntityManager.InteractAt(Console.CursorLeft, Console.CursorTop);
                        break;
                }
            }
        }
        public void MarkForRedraw(IDrawable drawable)
        {
            _toRedraw.Add(drawable);
        }

        public (int X, int Y) GetCursorTarget()
        {
            IPlayerCharacter player = EntityManager.GetPlayer();
            return (player.TargetX, player.TargetY);
        }
    }
}
