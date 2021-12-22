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
    }

    internal class EntityManager : IEntityManager
    {
        private IPlayerCharacter _player;
        private IScreen _container;
        private List<IEntity> _entities = new List<IEntity>();

        public EntityManager(IScreen container)
        {
            _container = container;
            AddTestEntities();
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
            if (_entities.Where(e => e.X == x && e.Y == y).Count() > 0)
                return true;
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
            IEntity? item;
            if (_player.HeldItem != null)
            {
                if (!CheckCollision(x, y) && !CheckInvalidAdjacency(_player.HeldItem, x, y))
                {
                    item = _player.PutDownItem(x, y);
                    if (item != null)
                        _entities.Add(item); 
                }
            }
            else
            {
                item = _entities.Find(e => e.X == x && e.Y == y);
                if (item != null)
                {
                    if (item is ICarryable)
                    {
                        _player.PickUpItem((ICarryable)item);
                        _entities.Remove(item);
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
    }
}
