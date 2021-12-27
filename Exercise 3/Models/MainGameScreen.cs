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
        private bool _firstDraw = true;
        private DateTime _startTime;
        private bool _exit = false;

        public int RoomWidth { get; private set; }
        public int RoomHeight { get; private set; }
        public bool OrderingItems { get; private set; } = false;

        public IEntityManager EntityManager { get; private set; }

        public MainGameScreen(int roomWidth, int roomHeight, string playerName, ColoredSymbol playerSymbol)
        {
            RoomWidth = roomWidth;
            RoomHeight = roomHeight;
            _computerLocation = (7, RoomHeight - 1);
            _conveyorLocation = (0, RoomHeight - 1);
            _incineratorLocation = (RoomWidth / 2, 0);
            EntityManager = new EntityManager(this);
            BuildRoom();
            EntityManager.AddPlayer(new PlayerCharacter(playerName, playerSymbol, this), _computerLocation.x, _computerLocation.y+1, "down");
            Console.Clear();
        }

        internal void ExitGame()
        {
            _exit = true;
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
            if(_exit)
            {
                return new ScoreScreen(_startTime, EntityManager.CountTotalItems(), EntityManager.CountUniqueItems(), EntityManager.GetPlayer().Name);
            }

            if (_firstDraw)
            {
                Console.Clear();
                _startTime = DateTime.Now;
                _firstDraw = false;
            }

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

                //Get the held item, if any, and list its details
                IStorageItem? heldItem = EntityManager.GetHeldItem();
                if(heldItem != null)
                {
                    ClearSideBar();
                    ShowItemDetails(heldItem, "HELD ITEM");
                }
                else
                {
                    //Get the item that the player is targeting, and list its details if it is a StorageItem
                    IStorageItem? targetItem = EntityManager.GetItemAt(cursorTarget.X, cursorTarget.Y);
                    if (targetItem != null)
                    {
                        ClearSideBar();
                        ShowItemDetails(targetItem, "TARGETED ITEM"); 
                    }
                    else
                    {
                        //Show a list of all unique items in storage
                        ClearSideBar();
                        ShowItemList();
                    }
                }

                Console.SetCursorPosition(cursorTarget.X, cursorTarget.Y);

                //Read player input
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
                    case ConsoleKey.Enter:
                        EntityManager.InteractAt(Console.CursorLeft, Console.CursorTop);
                        break;
                    case ConsoleKey.Spacebar:
                        EntityManager.InteractAt(Console.CursorLeft, Console.CursorTop);
                        break;
                }
            }

            return this;
        }

        private void ShowItemList()
        {
            int x = RoomWidth + 2;
            int maxWidth = Console.BufferWidth - RoomWidth - 2;
            List<IStorageItem>? items = EntityManager.GetUniqueItems().ToList();

            if (items != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(x, 0);
                Console.Write(CenteredString("ITEMS IN STORAGE", '#', maxWidth));
                for (int i = 0; i < items.Count(); i++)
                {
                    IStorageItem item = items[i];
                    int count = EntityManager.CountItemsByID(item.ID);
                    Console.SetCursorPosition(x, i + 1);
                    Console.Write(TruncatedString($"{count} x {item.ID}: {item.Name}, {item.Description}", maxWidth));
                } 
            }
        }

        private void ShowItemDetails(IStorageItem? item, string header)
        {
            if(item != null)
            {
                int x = RoomWidth + 2;
                int maxWidth = Console.BufferWidth - RoomWidth - 2;
                Console.ForegroundColor = item.Symbol.Color;
                Console.SetCursorPosition(x, 0);
                Console.Write(CenteredString(header, '#', maxWidth));
                Console.SetCursorPosition(x, 1);
                Console.Write(TruncatedString("ID: " + item.ID, maxWidth));
                Console.SetCursorPosition(x, 2);
                Console.Write(TruncatedString("Name: " + item.Name, maxWidth));
                Console.SetCursorPosition(x, 3);
                Console.Write(TruncatedString("Cat: " + item.Category, maxWidth));
                Console.SetCursorPosition(x, 4);
                Console.Write(TruncatedString("Desc: " + item.Description, maxWidth));
                Console.SetCursorPosition(x, 5);
                Console.Write(TruncatedString("Price: " + item.Price, maxWidth));
            }
        }

        private void ClearSideBar()
        {
            int x = RoomWidth + 2;
            int maxWidth = Console.BufferWidth - RoomWidth - 2;
            for(int i = 0; i < Console.BufferHeight; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write("".PadRight(maxWidth, ' '));
            }
        }

        private string TruncatedString(string str, int maxWidth)
        {
            if(str.Length > maxWidth)
                return str.Substring(0, maxWidth-3) + "...";
            return str;
        }

        private string CenteredString(string str, char padding, int width)
        {
            str = TruncatedString(str, width - 2);
            int left = (width - str.Length - 2) / 2;
            return $" {str} ".PadRight(width - left, padding).PadLeft(width, padding);
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
                    else if(EntityManager.ItemExists(name))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nProduct name must be unique.\nTry again.");
                        name = "";
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
