using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseWorker.Models;

namespace WarehouseWorker.Managers
{
    /// <summary>
    /// Handles the various entities in the game, such as the player and objects in storage.
    /// </summary>
    internal interface IEntityManager
    {
        void MovePlayer(int x, int y, int maxX, int maxY, int minX, int minY);
        void AddEntity(IEntity entity);
        IPlayerCharacter GetPlayer();
        void InteractAt(int x, int y);
        void AddPlayer(IPlayerCharacter playerCharacter, int x, int y, string direction);
        void OrderItem(IStorageItem item);
        StorageItem? GetItem(int iD);
        bool ItemExists(int iD);
        ColoredSymbol RandomSymbol();
        void DestroyHeldItem();
        IEntity? GetItemAt(int x, int y);
        IStorageItem? GetHeldItem();
    }

    internal class EntityManager : IEntityManager
    {
        private IPlayerCharacter _player;
        private IScreen _container;
        private List<ColoredSymbol> _uniqueSymbols = new List<ColoredSymbol>();
        private List<IEntity> _entities = new List<IEntity>();
        private List<IEntity> _orderQueue = new List<IEntity>();
        private List<IStorageItem> _uniqueItems = new List<IStorageItem>();

        public EntityManager(IScreen container)
        {
            _container = container;
            //AddTestEntities();
            SetupRandomSymbol();
        }

        private void SetupRandomSymbol()
        {
            List<char> symbols = new List<char>()
            {
                '#',
                '£',
                '¤',
                '$',
                '%',
                '&',
                '=',
                '+',
                '€',
                '©',
                '®',
                '+',
                '*'
            };
            List <ConsoleColor> colors = new List<ConsoleColor>()
            { 
                ConsoleColor.Green,
                ConsoleColor.Red,
                ConsoleColor.DarkYellow,
                ConsoleColor.Blue,
                ConsoleColor.Magenta,
                ConsoleColor.Cyan,
            };

            foreach (char symbol in symbols)
            {
                foreach(ConsoleColor color in colors)
                {
                    _uniqueSymbols.Add(new ColoredSymbol(symbol, color));
                }
            }
        }

        private void AddTestEntities()
        {
            AddEntity(new StorageItem(1, "TestItem", "TestCategory", "TestDescription", 10, '#', ConsoleColor.Blue, _container, 2, 2));
            AddEntity(new StorageItem(2, "TestItem", "TestCategory", "TestDescription", 10, '¤', ConsoleColor.Red, _container, 8, 2));
            AddEntity(new StorageItem(1, "TestItem", "TestCategory", "TestDescription", 10, '#', ConsoleColor.Blue, _container, 8, 5));
        }

        public void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            _container.MarkForRedraw(entity);
        }

        public IPlayerCharacter GetPlayer()
        {
            return _player;
        }

        public void MovePlayer(int x, int y, int maxX, int maxY, int minX, int minY)
        {
            //Update the player's direction
            string direction;
            if (x < 0)
                direction = "left";
            else if (x > 0)
                direction = "right";
            else if (y < 0)
                direction = "up";
            else
                direction = "down";

            int newX = _player.X + x;
            int newY = _player.Y + y;

            if (!CheckCollision(newX, newY))
            {
                _player.MoveTo(newX, newY);
            }

            _player.SetDirection(direction);
        }

        private bool CheckCollision(int x, int y)
        {
            List<IEntity> items = _entities.Where(e => e.X == x && e.Y == y).ToList();
            items.AddRange(_orderQueue.Where(e => e.X == x && e.Y == y).ToList());
            if (items.Count() > 0)
            {
                //ADD CHECK TO ALLOW WALKING INTO THE EXIT DOOR
                return true;
            }
            return false;
        }

        private bool CheckInvalidAdjacency(ICarryable item, int x, int y)
        {
            if(item is IStorageItem)
            {
                List<IStorageItem> adjacentItems = new List<IStorageItem>();
                foreach(IEntity entity in _entities)
                {
                    if(entity is IStorageItem)
                    {
                        if (Math.Abs(x - entity.X) <= 1 && Math.Abs(y - entity.Y) <= 1)
                            adjacentItems.Add((IStorageItem)entity);
                    }
                }

                if (adjacentItems.Where(i => i.ID != ((IStorageItem)item).ID).Count() > 0)
                    return true;
            }
            return false;
        }

        public void InteractAt(int x, int y)
        {
            if (_player.HeldItem != null)
            {
                if (!CheckCollision(x, y) && !CheckInvalidAdjacency(_player.HeldItem, x, y))
                {
                    IEntity? item = _player.PutDownItem(x, y);
                    if (item != null)
                        _entities.Add(item); 
                }
                else
                {
                    IEntity? item = _entities.FindLast(e => e.X == x && e.Y == y);
                    if (item is IInteractable)
                        ((IInteractable)item).Interact();
                }
            }
            else
            {
                List<IEntity> items = _entities.Where(e => e.X == x && e.Y == y).ToList();
                items.AddRange(_orderQueue.Where(e => e.X == x && e.Y == y).ToList());
                IEntity? item = items.LastOrDefault();
                if (item != null)
                {
                    if (item is ICarryable && _player.HeldItem == null)
                    {
                        _player.PickUpItem((ICarryable)item);
                        _entities.Remove(item);
                        _orderQueue.Remove(item);
                    }
                    else if (item is IInteractable)
                    {
                        ((IInteractable)item).Interact();
                    }
                } 
            }
        }

        public void AddPlayer(IPlayerCharacter playerCharacter, int x, int y, string direction)
        {
            _player = playerCharacter;
            _player.MoveTo(x, y);
            _player.SetDirection(direction);
            _container.MarkForRedraw(_player);
        }

        public void OrderItem(IStorageItem item)
        {
            if (_orderQueue.Count < 3)
            {
                _orderQueue.Add(item);
                if (!_uniqueItems.Contains(item))
                    _uniqueItems.Add(item);
                foreach (IStorageItem queueItem in _orderQueue)
                    queueItem.MoveTo(queueItem.X + 1, queueItem.Y);
                _container.MarkForRedraw(item); 
            }
        }

        public StorageItem? GetItem(int iD)
        {
            return _uniqueItems.Find(x => x.ID == iD) as StorageItem;
        }

        public bool ItemExists(int id)
        {
            return _uniqueItems.Where(x => x.ID == id).Count() > 0;
        }

        public ColoredSymbol RandomSymbol()
        {
            Random random = new Random(DateTime.Now.Second);
            ColoredSymbol symbol = _uniqueSymbols[random.Next(0, _uniqueSymbols.Count)];
            _uniqueSymbols.Remove(symbol);
            return symbol;
        }

        public void DestroyHeldItem()
        {
            IStorageItem? item = _player.HeldItem as IStorageItem;
            if(item != null)
            {
                _player.HeldItem = null;
                _entities.Remove(item);

                List<IStorageItem> items = _entities.Where(x => x.GetType() == typeof(IStorageItem)).Select(x => (IStorageItem)x).ToList();
                items.AddRange(_orderQueue.Select(x => (IStorageItem)x));
                if(items.Where(x => x.ID == item.ID).Count() <1)
                    _uniqueItems.Remove(_uniqueItems.Find(x => x.ID == item.ID));
            }
        }

        public IEntity? GetItemAt(int x, int y)
        {
            List<IEntity> items = new List<IEntity>(_entities);
            items.AddRange(_orderQueue);
            return items.FindLast(e => e.X == x && e.Y == y);
        }

        public IStorageItem? GetHeldItem()
        {
            return _player.HeldItem as IStorageItem;
        }
    }
}
