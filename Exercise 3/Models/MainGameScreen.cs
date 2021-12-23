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
        private (int x, int y) _conveyorLocation;
        private (int x, int y) _incineratorLocation;

        public int RoomWidth { get; private set; }
        public int RoomHeight { get; private set; }
        public bool OrderingItems { get; private set; } = false;

        public IEntityManager EntityManager { get; private set; }

        public MainGameScreen(int roomWidth, int roomHeight)
        {
            RoomWidth = roomWidth;
            RoomHeight = roomHeight;
            _computerLocation = (7, RoomHeight - 1);
            _conveyorLocation = (0, RoomHeight - 1);
            _incineratorLocation = (RoomWidth / 2, 0);
            EntityManager = new EntityManager(this);
            Console.SetWindowSize(50, 50);
            Console.SetBufferSize(50, 50);
            BuildRoom();
            EntityManager.AddPlayer(new PlayerCharacter("Amanda", 'A', ConsoleColor.Green, this), _computerLocation.x, _computerLocation.y+1, "down");
        }

        internal void StartOrdering()
        {
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

            //Conveyor belt
            EntityManager.AddEntity(new Conveyor('╩', _conveyorLocation.x, _conveyorLocation.y - 1, this));
            EntityManager.AddEntity(new Conveyor(' ', _conveyorLocation.x, _conveyorLocation.y, this));
            EntityManager.AddEntity(new Conveyor('╦', _conveyorLocation.x, _conveyorLocation.y + 1, this));
            EntityManager.AddEntity(new Conveyor('═', _conveyorLocation.x+1, _conveyorLocation.y+1, this));
            EntityManager.AddEntity(new Conveyor('═', _conveyorLocation.x + 2, _conveyorLocation.y+1, this));
            EntityManager.AddEntity(new Conveyor('═', _conveyorLocation.x + 3, _conveyorLocation.y+1, this));
            EntityManager.AddEntity(new Conveyor('═', _conveyorLocation.x + 4, _conveyorLocation.y+1, this));
            EntityManager.AddEntity(new Conveyor(':', _conveyorLocation.x + 4, _conveyorLocation.y, this));
            EntityManager.AddEntity(new Conveyor('╝', _conveyorLocation.x + 5, _conveyorLocation.y+1, this));
            EntityManager.AddEntity(new Conveyor('║', _conveyorLocation.x + 5, _conveyorLocation.y, this));

            //Incinerator
            EntityManager.AddEntity(new Incinerator('X', _incineratorLocation.x, _incineratorLocation.y, this, EntityManager));

            //Exit door
            EntityManager.AddEntity(new Wall('╠', RoomWidth + 1, RoomHeight - 1, this));
            EntityManager.AddEntity(new Door('\\', RoomWidth + 1, RoomHeight, this));
            EntityManager.AddEntity(new Wall('═', RoomWidth + 1, RoomHeight + 1, this));
        }

        public IScreen Show()
        {
            foreach (IDrawable drawable in _toRedraw)
                drawable.Draw();

            _toRedraw.Clear();

            if(OrderingItems)
            {
                ClearInputArea();
                OrderItem();
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

            return this;
        }

        private void OrderItem()
        {
            //Get product ID from user
            int ID = 0;
            while (ID < 1)
            {
                ClearInputArea();
                Console.WriteLine("### ORDER ITEM ###");
                Console.Write("ID: ");
                int.TryParse(Console.ReadLine(), out ID);
                if (ID < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid ID.\nTry again.");
                    Console.ReadLine();
                }
            }

            StorageItem? item = null;
            //If ID has been ordered before, order another one, otherwise fill in the rest of the information
            if (EntityManager.ItemExists(ID))
            {
                StorageItem existingItem = EntityManager.GetItem(ID);
                item = new StorageItem(existingItem.ID, existingItem.Name, existingItem.Category, existingItem.Description, existingItem.Price, existingItem.Symbol, this, _conveyorLocation.x, _conveyorLocation.y);
            }
            else
            {
                //Get product name from user
                string name = "";
                while (name == "")
                {
                    ClearInputArea();
                    Console.WriteLine("### ORDER ITEM ###");
                    Console.Write("Name: ");
                    name = Console.ReadLine() ?? "";
                    if (name == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nYou must enter a product name.");
                        Console.ReadLine();
                    }
                }
                //Get product category from user
                string category = "";
                while (category == "")
                {
                    ClearInputArea();
                    Console.WriteLine("### ORDER ITEM ###");
                    Console.Write("Category: ");
                    category = Console.ReadLine() ?? "";
                    if (category == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nYou must enter a category.");
                        Console.ReadLine();
                    }
                }
                //Get product description from user
                ClearInputArea();
                Console.WriteLine("### ORDER ITEM ###");
                Console.Write("Description: ");
                string description = Console.ReadLine() ?? "";
                //Get product price from user
                int price = 0;
                while (price < 1)
                {
                    ClearInputArea();
                    Console.WriteLine("### ORDER ITEM ###");
                    Console.Write("Price: ");
                    int.TryParse(Console.ReadLine(), out price);
                    if (price < 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nPrice must be greater than 0.");
                        Console.ReadLine();
                    }
                }
                //Get a random unique symbol/color combination
                ColoredSymbol symbol = EntityManager.RandomSymbol();

                item = new StorageItem(ID, name, category, description, price, symbol, this, _conveyorLocation.x, _conveyorLocation.y);
            }

            if(item != null)
                EntityManager.OrderItem(item);

            ClearInputArea();
            OrderingItems = false;
        }

        private void ClearInputArea()
        {
            Console.SetCursorPosition(0, RoomHeight + 3);
            Console.WriteLine("".PadRight(Console.BufferWidth));
            Console.WriteLine("".PadRight(Console.BufferWidth));
            Console.WriteLine("".PadRight(Console.BufferWidth));
            Console.WriteLine("".PadRight(Console.BufferWidth));
            Console.WriteLine("".PadRight(Console.BufferWidth));

            Console.SetCursorPosition(0, RoomHeight + 3);
            Console.ForegroundColor = ConsoleColor.Yellow;
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
